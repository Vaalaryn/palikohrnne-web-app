using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using palikohrnne_web_app.Api;
using palikohrnne_web_app.Models;
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

        public async Task<IActionResult> CitoyenInfo(int id)
        {
            var model = await _cubesService.GetCitoyenById(id);
            ViewBag.Rangs = new SelectList(await _cubesService.GetAllRangs(),"ID","Nom",model.RangID);
            return View(model);
        }

        public async Task<IActionResult> ModifierRang(int id,int rangId)
        {
            Citoyen citoyen = await _cubesService.GetCitoyenById(id);
            citoyen.RangID = rangId;
            await _cubesService.UpdateCitoyen(citoyen);

            return RedirectToAction("CitoyenInfo", new { id = id });
        }

        public async Task<IActionResult> DeleteActivateCitoyen(int id)
        {
            Citoyen citoyen = await _cubesService.GetCitoyenById(id);

            await _cubesService.DeleteCitoyen(id);
            return RedirectToAction("CitoyenInfo", new { id = id });
        }
    }
}
