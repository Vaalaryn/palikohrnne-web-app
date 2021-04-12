using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using palikohrnne_web_app.Api;
using palikohrnne_web_app.Extensions;
using palikohrnne_web_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Controllers
{
    [Authorize]
    public class BackOfficeController : Controller
    {
        private readonly CubesService _cubesService;

        public BackOfficeController(CubesService cubesService)
        {
            _cubesService = cubesService;
        }


        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> Statistiques()
        {
            _cubesService.Authorize(User);
            var ressources = await _cubesService.GetAllRessources();
            StatistiqueModel model = new StatistiqueModel
            {
                Citoyens = await _cubesService.GetAllCitoyens(),
                Ressources = ressources,
                RelationCitoyens = await _cubesService.GetAllRelations(),
                Commentaires = await _cubesService.GetAllCommentaires(),
                CitoyenVoteds = ressources.SelectMany(x => x.CitoyenVoted),
            };

            return View(model);
        }

        public async Task<IActionResult> ListeRessource(string order, FiltresModelRessources filtres)
        {
            IEnumerable<Ressource> ressources = await _cubesService.Authorize(User).GetAllRessources();
            IEnumerable<TypeRelation> typesRelations = await _cubesService.Authorize(User).GetAllTypeRelations();
            IEnumerable<Tag> tags = await _cubesService.Authorize(User).GetAllTags();

            if (string.IsNullOrEmpty(filtres.AnswersFilter))
            {
                filtres.AnswersFilter = "all-answers";
            }

            //Filtres -------------------------------------
            //Filtre des réponses
            switch (filtres.AnswersFilter)
            {
                case "no-answers":
                    {
                        ressources = ressources.Where(x => x.Commentaires.Count == 0);
                        break;
                    }
                case "only-answers":
                    {
                        ressources = ressources.Where(x => x.Commentaires.Count > 0);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            //Filtre ancienneté
            switch (filtres.AgeMax)
            {
                case "lastWeek":
                    {
                        var monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
                        ressources = ressources.Where(x => x.CreatedAt >= monday);
                        break;
                    }
                case "lastMonth":
                    {
                        ressources = ressources.Where(x => x.CreatedAt.Month == DateTime.Now.Month);
                        break;
                    }
                case "lastYear":
                    {
                        ressources = ressources.Where(x => x.CreatedAt.Year == DateTime.Now.Year);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            //Filtre cercle social
            if ((filtres.TypeRelationID != null) && (filtres.TypeRelationID.Any()))
            {
                ressources = ressources.Where(x => filtres.TypeRelationID.Contains(x.TypeRelationID));
            }
            else
            {
            }
            //Filtre tags
            if ((filtres.TagsID != null) && (filtres.TagsID.Any()))
            {
                ressources = ressources.Where(x => x.Tags.Select(x => x.ID).Where(x => filtres.TagsID.Contains(x)).Any());
            }
            //FIN Filtres -------------------------------------


            //Tri
            switch (order)
            {
                case "recents":
                    {
                        ressources = ressources.OrderByDescending(x => x.CreatedAt);
                        break;
                    }
                case "reponsesDesc":
                    {
                        ressources = ressources.OrderByDescending(x => x.Commentaires.Count);
                        break;
                    }
                case "reponsesAsc":
                    {
                        ressources = ressources.OrderBy(x => x.Commentaires.Count);
                        break;
                    }
                default:
                    {
                        ressources = ressources.OrderBy(x => x.CreatedAt);
                        break;
                    }
            }

            //Cache
            ViewBag.Order = order;
            ViewBag.TypesRelations = typesRelations;
            ViewBag.Tags = new SelectList(tags, "ID", "Nom");

            //Création du model
            ListeRessourceModel model = new ListeRessourceModel
            {
                Ressources = ressources.Where(x => x.ValidationAdmin == true).ToList(),
                Filtres = filtres
            };
            return View(model);
        }

        public async Task<IActionResult> ListCitoyens()
        {
            return View(await _cubesService.Authorize(User).GetAllCitoyens());
        }

        public async Task<IActionResult> CitoyenInfo(int id)
        {
            var model = await _cubesService.Authorize(User).GetCitoyenById(id);
            ViewBag.Rangs = new SelectList(await _cubesService.Authorize(User).GetAllRangs(), "ID", "Nom", model.RangID);
            return View(model);
        }

        public async Task<IActionResult> ModifierRang(int id, int rangId)
        {
            Citoyen citoyen = await _cubesService.Authorize(User).GetCitoyenById(id);
            citoyen.RangID = rangId;
            await _cubesService.Authorize(User).UpdateCitoyen(citoyen);

            return RedirectToAction("CitoyenInfo", new { id = id });
        }

        public async Task<IActionResult> DeleteActivateCitoyen(int id)
        {
            Citoyen citoyen = await _cubesService.Authorize(User).GetCitoyenById(id);

            await _cubesService.Authorize(User).DeleteCitoyen(id);
            return RedirectToAction("CitoyenInfo", new { id = id });
        }

        public async Task<IActionResult> ValidationRessourceList()
        {
            var model = await _cubesService.Authorize(User).GetAllRessources();
            return View(model.Where(x => !x.ValidationAdmin.HasValue));
        }

        public async Task<IActionResult> DetailsRessourceNonValide(int id)
        {
            var model = await _cubesService.Authorize(User).GetRessourceById(id);

            if (model.ValidationAdmin.HasValue)
            {
                return NotFound();
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ValidationRessource(int id, string decision)
        {
            Ressource ressource = await _cubesService.Authorize(User).GetRessourceById(id);
            ressource.ValidationAdmin = decision == "Valider";
            await _cubesService.Authorize(User).UpdateRessource(ressource);

            return RedirectToAction("ValidationRessourceList");
        }
    }
}
