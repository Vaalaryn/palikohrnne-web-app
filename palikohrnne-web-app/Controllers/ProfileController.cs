using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using palikohrnne_web_app.Api;
using palikohrnne_web_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Controllers
{
    public class ProfileController : Controller
    {
        private readonly CubesService _cubesService;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(CubesService cubesService, ILogger<ProfileController> logger)
        {
            _cubesService = cubesService;
            _logger = logger;
        }

        
        public async Task<IActionResult> IndexAsync()
        {
            var citoyens = await _cubesService.GetAllCitoyens();
            return View(citoyens);
        }

        public async Task<IActionResult> DetailAsync(int id)
        {
            var citoyen = await _cubesService.GetCitoyenById(id);
            return View(citoyen);
        }

        [Authorize]
        public async Task<IActionResult> SelfDetailAsync()
        {
            var selfId = Int32.Parse(User.FindFirst("ID").Value);
            var citoyen = await _cubesService.GetCitoyenById(selfId);
            return View(citoyen);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SelfDetailAsync(Citoyen citoyen)
        {
            var selfId = Int32.Parse(User.FindFirst("ID").Value);
            var citoyenConnected = await _cubesService.GetCitoyenById(selfId);

            Citoyen modifiedCitoyen = new Citoyen()
            {
                ID = selfId,
                CreatedAt = citoyenConnected.CreatedAt,
                UpdatedAt = citoyenConnected.UpdatedAt,
                DeletedAt = citoyenConnected.DeletedAt,
                Adresse = citoyen.Adresse,
                CodePostal = citoyen.CodePostal,
                Genre = citoyen.Genre,
                Mail = citoyen.Mail,
                MotDePasse = citoyen.MotDePasse,
                Nom = citoyen.Nom,
                Prenom = citoyen.Prenom,
                Pseudo = citoyen.Pseudo,
                Telephone = citoyen.Telephone,
                Ville = citoyen.Ville,
                RangID = citoyenConnected.RangID
            };

            await _cubesService.UpdateCitoyen(modifiedCitoyen);
            return View();
        }

    }
}
