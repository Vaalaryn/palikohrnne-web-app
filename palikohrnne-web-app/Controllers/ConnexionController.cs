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
    public class ConnexionController : Controller
    {

        private readonly CubesService _cubesService;

        public ConnexionController(CubesService cubesService)
        {
            _cubesService = cubesService;
        }
        
        public async Task<IActionResult> IndexAsync()
        {
            var citoyens = await _cubesService.GetAllCitoyens();
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
            
            await _cubesService.CreateCitoyen(obj);
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
