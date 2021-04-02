using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using palikohrnne_web_app.Api;
using palikohrnne_web_app.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Controllers
{
    public class HomeController : Controller
    {
        private readonly CubesService _cubesService;

        public HomeController(CubesService cubesService)
        {
            _cubesService = cubesService;
        }

        public async Task<IActionResult> Index()
        {
            await _cubesService.CreateTypeRelation(new TypeRelation { 
                Nom = "Relation"
            });
            //_cubesService.CreateRessource(new Ressource
            //{
            //    Titre = "Nouvelle ressource",
            //    Contenu = "Bla bla c'est pas intéressant",
            //    CitoyenID = 1,
            //    TypeRelationID = 1

            //});
            //var result = await _cubesService.GetAllRessources();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
