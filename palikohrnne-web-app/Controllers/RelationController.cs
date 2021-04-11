using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
                Relations = relations.ToList(),
                InRelation = inRelations.ToList()
            });
        }
    }
}
