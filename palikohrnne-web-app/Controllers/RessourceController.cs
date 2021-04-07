using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using palikohrnne_web_app.Api;
using palikohrnne_web_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Controllers
{
    public class RessourceController : Controller
    {
        private readonly CubesService _cubeService;
        public RessourceController(CubesService cubesService)
        {
            _cubeService = cubesService;
        }
        public async Task<IActionResult> Index(int id, string order, FiltresModelRessources filtres)
        {
            IEnumerable<Ressource> ressources = await _cubeService.GetAllRessources();
            IEnumerable<TypeRelation> typesRelations = await _cubeService.GetAllTypeRelations();
            IEnumerable<Tag> tags = await _cubeService.GetAllTags();
            if(string.IsNullOrEmpty(filtres.AnswersFilter))
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
            if((filtres.TypeRelationID != null) && (filtres.TypeRelationID.Any()))
            {
                ressources = ressources.Where(x => filtres.TypeRelationID.Contains(x.TypeRelationID));
            }
            else
            {
                ressources = new List<Ressource>();
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
            ViewBag.Tags = new SelectList(tags,"ID","Nom");

            //Création du model
            ListeRessourceModel model = new ListeRessourceModel
            {
                Ressources = ressources.Where(x => x.Categorie.ID == id).ToList(),
                Filtres = filtres
            };
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await _cubeService.GetRessourceById(id));
        }

        public async Task<IActionResult> PublierRessource(int? id)
        {
            IEnumerable<Tag> tags = await _cubeService.GetAllTags();
            IEnumerable<CategorieWithStats> categorieWithStats = await _cubeService.GetAllCategoriesWithStats();
            IEnumerable<TypeRelation> typesRelations = await _cubeService.GetAllTypeRelations();
            IEnumerable<TypeRessource> typesRessources = await _cubeService.GetAllTypeRessources();
            ViewBag.Tags = new SelectList(tags, "Nom", "Nom");
            ViewBag.Categories = new SelectList(categorieWithStats.Select(x => x.Categorie), "ID", "Nom", id);
            ViewBag.Diffusions = new SelectList(typesRelations.ToList(), "ID", "Nom", typesRelations.Where(x => x.Nom == "Publique").FirstOrDefault().ID);
            ViewBag.TypesRessources = new SelectList(typesRessources.ToList(), "ID", "Nom");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PublierRessource(Ressource ressource)
        {
            IEnumerable<Tag> tags = await _cubeService.GetAllTags();
            IEnumerable<CategorieWithStats> categorieWithStats = await _cubeService.GetAllCategoriesWithStats();
            IEnumerable<TypeRelation> typesRelations = await _cubeService.GetAllTypeRelations();
            ViewBag.Tags = new SelectList(tags, "Nom", "Nom");
            ViewBag.Categories = new SelectList(categorieWithStats.Select(x => x.Categorie), "ID", "Nom", ressource.CategorieID);
            ViewBag.Diffusions = new SelectList(typesRelations.Select(x => x.Nom).ToList(), "ID", "Nom");

            List<Tag> tagsList = new List<Tag>();

            IEnumerable<string> tagsFrom = Request.Form["Tags[]"].Select(x => x.ToLower()).ToList();
            IEnumerable<Tag> tagsFromApi = await _cubeService.GetAllTags();
            IEnumerable<string> tagsStrFromApi = tagsFromApi.Select(x => x.Nom).ToList();

            IEnumerable<string> tagsAreNotInApi = tagsFrom.Except(tagsStrFromApi);
            IEnumerable<Tag> tagsAreInApi = tagsFromApi.Where(x => tagsFrom.Contains(x.Nom.ToLower()));


            foreach (string tag in tagsAreNotInApi)
            {
                Tag tagObject = await _cubeService.CreateTag(new Tag
                {
                    Nom = tag
                });
                tagsList.Add(tagObject);
            }
            tagsList.AddRange(tagsAreInApi);


            if (ModelState.IsValid)
            {
                ressource.Tags = tagsList;
                Ressource ressourceCreated = await _cubeService.CreateRessource(ressource);
                return RedirectToAction("Details", new { id = ressourceCreated.ID });
            }

            return View(ressource);
        }

        [HttpPost]
        public async Task<IActionResult> CommenterRessource(Commentaire commentaire)
        {
            await _cubeService.CreateCommentaire(commentaire);

            return RedirectToAction("Details", new { id = commentaire.RessourceID });
        }
    }
}
