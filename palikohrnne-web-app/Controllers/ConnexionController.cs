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
            List<CitoyenModel> citoyens = ApiCube.GetCitoyens().Result;
            return View(citoyens);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(CitoyenModel obj)
        {
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
