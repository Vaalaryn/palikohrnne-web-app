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
    public class ExempleCrudController : Controller
    {
        private readonly CubesService _cubesService;

        public ExempleCrudController(CubesService cubesService)
        {
            _cubesService = cubesService;
        }

        public async Task<IActionResult> Index()
        {
            //Ajouter un citoyen
            await _cubesService.CreateCitoyen(new Citoyen
            {
                Adresse = "598 rue des toto",
                CodePostal = "76450",
                Genre = "M",
                Mail = "toto@gmail.toto",
                MotDePasse = "1234",
                Nom = "Toto",
                Prenom = "Tutu",
                Pseudo = "totolerigolo",
                RangID = 1,
                Telephone = "025653535",
                Ville = "Rouen"
            });
            //Récupérer le citoyen


            //Récupérer tous les citoyens
            var citoyens = await _cubesService.GetAllCitoyens();
            //Supprimer le citoyen
            await _cubesService.DeleteCitoyen(5);

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
