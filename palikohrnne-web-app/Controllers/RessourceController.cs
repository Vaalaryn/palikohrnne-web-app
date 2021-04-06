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
        public async Task<IActionResult> Index(int id)
        {
            var ressources = await _cubeService.GetAllRessources();
            return View(ressources.Where(x => x.Categorie.ID == id).ToList());
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await _cubeService.GetRessourceById(id));
        }

        public async Task<IActionResult> PublierRessource()
        {
            IEnumerable<Tag> tags = await _cubeService.GetAllTags();
            ViewBag.Tags = new SelectList(tags, "Nom", "Nom");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PublierRessource(Ressource ressource)
        {
            List<Tag> tagsList = new List<Tag>();

            IEnumerable<string> tags = Request.Form["Tags[]"].Select(x => x.ToLower()).ToList();
            IEnumerable<Tag> tagsFromApi = await _cubeService.GetAllTags();
            IEnumerable<string> tagsStrFromApi = tagsFromApi.Select(x => x.Nom).ToList();

            IEnumerable<string> tagsAreNotInApi = tags.Except(tagsStrFromApi);
            IEnumerable<Tag> tagsAreInApi = tagsFromApi.Where(x => tags.Contains(x.Nom.ToLower()));


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
