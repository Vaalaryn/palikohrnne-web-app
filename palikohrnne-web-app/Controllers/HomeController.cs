﻿using Microsoft.AspNetCore.Authorization;
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
    public class HomeController : Controller
    {
        private readonly CubesService _cubesService;

        public HomeController(CubesService cubesService)
        {
            _cubesService = cubesService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _cubesService.GetAllCategoriesWithStats());
        }
    }
}
