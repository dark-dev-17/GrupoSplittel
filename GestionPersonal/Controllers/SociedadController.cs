using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using GPDataInformation.Models;
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
            Sociedad sociedad = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Sociedad,null);
            IEnumerable<Sociedad> sociedads = sociedad.Get();
            gestionPersonal.CloseConnection();
            return View(sociedads);
        }

        // GET: Sociedad/Details/5
        public ActionResult Details(int id)
        {
            gestionPersonal.OpenConnection();
            Sociedad sociedad = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Sociedad, null);
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
        public ActionResult Create(Sociedad Sociedad)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    gestionPersonal.OpenConnection();
                    Sociedad sociedad = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Sociedad, Sociedad);
                    bool result = sociedad.Add();
                    gestionPersonal.CloseConnection();
                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", gestionPersonal.GetLastMessage());
                        return View(Sociedad);
                    }
                }
                else
                {
                    return View(Sociedad);
                }
            }
            catch (GPDataInformation.GpExceptions ex)
            {
                if(gestionPersonal != null)
                {
                    gestionPersonal.CloseConnection();
                    gestionPersonal = null;
                }
                ModelState.AddModelError("", ex.Message);
                return View(Sociedad);
            }
        }

        // GET: Sociedad/Edit/5
        public ActionResult Edit(int id)
        {
            gestionPersonal.OpenConnection();
            Sociedad sociedad = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Sociedad, null);
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
        public ActionResult Edit(Sociedad Sociedad)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    gestionPersonal.OpenConnection();
                    Sociedad sociedad = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Sociedad, Sociedad);
                    bool result = sociedad.Update();
                    gestionPersonal.CloseConnection();
                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", gestionPersonal.GetLastMessage());
                        return View(Sociedad);
                    }
                }
                else
                {
                    return View(Sociedad);
                }
            }
            catch (GPDataInformation.GpExceptions ex)
            {
                if (gestionPersonal != null)
                {
                    gestionPersonal.CloseConnection();
                    gestionPersonal = null;
                }
                ModelState.AddModelError("", ex.Message);
                return View(Sociedad);
            }
        }

        // GET: Sociedad/Delete/5
        public ActionResult Delete(int id)
        {
            gestionPersonal.OpenConnection();
            Sociedad sociedad = gestionPersonal.GetObject(GPDataInformation.ObjectsCompany.Sociedad, null);
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