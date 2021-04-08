using Microsoft.AspNetCore.Mvc;
using palikohrnne_web_app.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Controllers
{
    public class BackOfficeController : Controller
    {
        private readonly CubesService _cubesService;

        public BackOfficeController(CubesService cubesService)
        {
            _cubesService = cubesService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ListCitoyens()
        {
            return View(await _cubesService.GetAllCitoyens());
        }
    }
}
