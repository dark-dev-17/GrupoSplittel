using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class SociedadController : Controller
    {
        private GPDataInformation.GestionPersonal gestionPersonal;
        public SociedadController(IConfiguration configuration)
        {
            this.gestionPersonal = new GPDataInformation.GestionPersonal(configuration);
        }

        // GET: Sociedad
        public ActionResult Index()
        {
            gestionPersonal.OpenConnection();
            GPDataInformation.Models.Sociedad sociedad = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Sociedad);
            IEnumerable<GPDataInformation.Models.Sociedad> sociedads = sociedad.Get();
            gestionPersonal.CloseConnection();
            return View(sociedads);
        }

        // GET: Sociedad/Details/5
        public ActionResult Details(int id)
        {
            gestionPersonal.OpenConnection();
            GPDataInformation.Models.Sociedad sociedad = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Sociedad);
            var result = sociedad.Get(id);
            gestionPersonal.CloseConnection();
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // GET: Sociedad/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sociedad/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Sociedad/Edit/5
        public ActionResult Edit(int id)
        {
            gestionPersonal.OpenConnection();
            GPDataInformation.Models.Sociedad sociedad = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Sociedad);
            var result = sociedad.Get(id);
            gestionPersonal.CloseConnection();
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // POST: Sociedad/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Sociedad/Delete/5
        public ActionResult Delete(int id)
        {
            gestionPersonal.OpenConnection();
            GPDataInformation.Models.Sociedad sociedad = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Sociedad);
            var result = sociedad.Get(id);
            gestionPersonal.CloseConnection();
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // POST: Sociedad/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}