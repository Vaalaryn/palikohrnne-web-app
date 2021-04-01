using Microsoft.AspNetCore.Mvc;
using palikohrnne_web_app.Api;
using palikohrnne_web_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Controllers
{
    public class ConnexionController : Controller
    {
        
        public IActionResult Index()
        {
            List<Citoyen> citoyens = ApiCube.GetCitoyens().Result;
            return View(citoyens);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(Citoyen obj)
        {
            obj.ID = ApiCube.GetCitoyens().Result.Count + 1;
            obj.CreatedAt = DateTime.Now;
            obj.UpdatedAt = DateTime.Now;
            obj.DeletedAt = null;
            obj.Rang = ApiCube.GetRang(obj.RangID).Result;
            obj.Ressource = null;
            await ApiCube.PostCitoyen(obj);
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string UserEmail, string UserPwd)
        {
            return View();
        }
    }
}
