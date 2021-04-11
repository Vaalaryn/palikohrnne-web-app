using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using palikohrnne_web_app.Api;
using palikohrnne_web_app.Extensions;
using palikohrnne_web_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Controllers
{
    public class ProfilController : Controller
    {
        private readonly CubesService _cubesService;

        public ProfilController(CubesService cubesService)
        {
            _cubesService = cubesService;
        }
        [Authorize]
        public async Task<IActionResult> Profil(int id)
        {
            var citoyen = await _cubesService.Authorize(User).GetCitoyenById(id);
            var userConnected = Int32.Parse(User.FindFirst("ID").Value);
            if (userConnected != citoyen.ID)
            {
                var relationsCible = await _cubesService.Authorize(User).GetRelationCitoyenIn(userConnected);
                ViewBag.DemandeRelation = relationsCible.Where(x => 
                x.CitoyenID == citoyen.ID &&
                x.CitoyenCibleID == userConnected && 
                !x.Approbation.HasValue).Any();
                if(ViewBag.DemandeRelation == true)
                {
                    ViewBag.TypeRelationId = relationsCible.First().TypeRelationID;
                }
                
            }
            return View(citoyen);
        }

        [Authorize]
        public async Task<IActionResult> ProfilPerso(int id)
        {
            var citoyen = await _cubesService.Authorize(User).GetCitoyenById(id);
            return View(citoyen);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SelfDetailAsync(Citoyen citoyen)
        {
            var selfId = Int32.Parse(User.FindFirst("ID").Value);
            var citoyenConnected = await _cubesService.Authorize(User).GetCitoyenById(selfId);

            Citoyen modifiedCitoyen = new()
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
