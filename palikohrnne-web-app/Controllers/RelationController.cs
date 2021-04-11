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
    public class RelationController : Controller
    {
        private readonly CubesService _cubeService;
        public RelationController(CubesService cubesService)
        {
            _cubeService = cubesService;
        }

        public async Task<IActionResult> Index()
        {
            int userId = Int32.Parse(((ClaimsIdentity)User.Identity).GetSpecificClaim("ID"));

            IEnumerable<RelationCitoyen> relations = await _cubeService.Authorize(User).GetRelation(userId);
            IEnumerable<RelationCitoyen> inRelations = await _cubeService.Authorize(User).GetRelationCitoyenIn(userId);


            return View(new RelationsModel
            {
                Relations = relations.Where(x => x.Approbation == true).ToList(),
                InRelation = inRelations.Where(x => !x.Approbation.HasValue).ToList()
            });
        }
        [Authorize] 
        public async Task<IActionResult> Ajouter()
        {
            ViewBag.TypeRelation = new SelectList(await _cubeService.GetAllTypeRelations(), "ID", "Nom");
            return View(new RelationCitoyen { 
                CitoyenID = Int32.Parse(((ClaimsIdentity)User.Identity).GetSpecificClaim("ID"))
            });
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Ajouter(RelationCitoyen relation)
        {
            await _cubeService.Authorize(User).AjouterRelation(relation);

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> DecisionRelation(string decision, int idCitoyen, int typeRelationId)
        {
            int userId = Int32.Parse(((ClaimsIdentity)User.Identity).GetSpecificClaim("ID"));

            await _cubeService.Authorize(User).UpdateRelation(new RelationCitoyen
            {
                Approbation = decision == "Accepter",
                CitoyenCibleID = userId,
                CitoyenID = idCitoyen
            });

            await _cubeService.Authorize(User).AjouterRelation(new RelationCitoyen
            {
                Approbation = true,
                CitoyenCibleID = idCitoyen,
                CitoyenID = userId,
                TypeRelationID = typeRelationId
            });

            return RedirectToAction("Index", "Relation");
        }

        public async Task<IActionResult> SearchCitoyen(string search = "")
        {
            int userID = Int32.Parse(((ClaimsIdentity)User.Identity).GetSpecificClaim("ID"));
            var citoyens = await _cubeService.GetAllCitoyens();
            var result = citoyens
                .Where(x => x.Nom.StartsWith(search) || x.Prenom.StartsWith(search) || x.Pseudo.StartsWith(search))
                .Where(x => x.ID != userID);
            return Json(new
            {
                results =
                result.Select(x => new
                {
                    id = x.ID,
                    text = x.Nom + " " + x.Prenom + " ("  + x.Pseudo +  ") "
                }).ToList()
            }); ;
        }
    }
}
