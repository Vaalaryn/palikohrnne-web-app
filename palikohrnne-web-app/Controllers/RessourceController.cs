using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Controllers
{
    public class RessourceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
