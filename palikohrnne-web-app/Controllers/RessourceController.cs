using Microsoft.AspNetCore.Mvc;
using palikohrnne_web_app.Api;
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
    }
}
