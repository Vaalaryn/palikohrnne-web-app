using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Index()
        {
            var ressources = await _cubeService.GetAllRessources();
            return View(ressources);
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await _cubeService.GetRessourceById(id));
        }

        public IActionResult PublierRessource()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CommenterRessource(Commentaire commentaire)
        {
            await _cubeService.CreateCommentaire(commentaire);

            return RedirectToAction("Details",new { id = commentaire.RessourceID});
        }
    }
}
