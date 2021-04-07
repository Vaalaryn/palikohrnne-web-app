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
        public async Task<IActionResult> Index(int id, string filtre)
        {
            IEnumerable<Ressource> ressources = await _cubeService.GetAllRessources();
            switch (filtre)
            {
                case "recents":
                    {
                        ressources = ressources.OrderBy(x => x.CreatedAt);
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
            ViewBag.Filtre = filtre;
            return View(ressources.Where(x => x.Categorie.ID == id).ToList());
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
