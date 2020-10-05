using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FibremexConfiArt;
using FibremexConfiArt.V1;
using Microsoft.AspNetCore.Mvc;

namespace Configurables.Controllers
{
    public class EditorController : Controller
    {
        private ManagerV1 ManagerV1;
        public EditorController()
        {
            ManagerV1 = new ManagerV1();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit(string id)
        {
            ViewBag["Configuracion"] = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Configurable Configurable)
        {
            ManagerV1.CreateNewConfiguracion(Configurable.Nombre, Configurable.Descripción);
            string path = string.Format(@"Confi_{0}.json", Configurable.Nombre );
            return RedirectToAction("Edit", new { id = path });
        }
    }
}
