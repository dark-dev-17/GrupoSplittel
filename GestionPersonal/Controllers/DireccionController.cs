using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPSInformation;
using GPSInformation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class DireccionController : Controller
    {
        private DarkManager darkManager;
        private SelectList sociedads;
        private SelectList Direcciones;


        public DireccionController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.Direccion);
            darkManager.LoadObject(GpsManagerObjects.Sociedad);

            sociedads = new SelectList(darkManager.Sociedad.Get().OrderBy(a => a.Descripcion).ToList(), "IdSociedad", "Descripcion");
            Direcciones = new SelectList(darkManager.Direccion.Get().OrderBy(a => a.Nombre).ToList(), "IdDireccion", "Nombre");
        }

        ~DireccionController()
        {

        }

        // GET: Direccion
        public ActionResult Index()
        {
            var result = darkManager.Direccion.Get().OrderBy(a => a.Nombre).ToList();
            return View(result);
        }

        // GET: Direccion/Details/5
        public ActionResult Details(int id)
        {
            var result = darkManager.Direccion.Get(id);

            if(result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // GET: Direccion/Create
        public ActionResult Create()
        {
            ViewData["Sociedades"] = sociedads;
            ViewData["Direcciones"] = Direcciones;
            return View();
        }

        // POST: Direccion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Direccion Direccion)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    ViewData["Sociedades"] = sociedads;
                    ViewData["Direcciones"] = Direcciones;
                    return View(Direccion);
                }

                darkManager.Direccion.Element = Direccion;
                bool result = darkManager.Direccion.Add();
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["Sociedades"] = sociedads;
                    ViewData["Direcciones"] = Direcciones;
                    ModelState.AddModelError("", darkManager.GetLastMessage());
                    return View(Direccion);
                }
                
            }
            catch(GPSInformation.Exceptions.GpExceptions ex)
            {
                ViewData["Sociedades"] = sociedads;
                ViewData["Direcciones"] = Direcciones;
                ModelState.AddModelError("", ex.Message);
                return View(Direccion);
            }
        }

        // GET: Direccion/Edit/5
        public ActionResult Edit(int id)
        {
            ViewData["Sociedades"] = sociedads;
            ViewData["Direcciones"] = Direcciones;
            var result = darkManager.Direccion.Get(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // POST: Direccion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Direccion Direccion)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    ViewData["Sociedades"] = sociedads;
                    ViewData["Direcciones"] = Direcciones;
                    return View(Direccion);
                }

                darkManager.Direccion.Element = Direccion;
                bool result = darkManager.Direccion.Update();
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["Sociedades"] = sociedads;
                    ViewData["Direcciones"] = Direcciones;
                    ModelState.AddModelError("", darkManager.GetLastMessage());
                    return View(Direccion);
                }

            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ViewData["Sociedades"] = sociedads;
                ViewData["Direcciones"] = Direcciones;
                ModelState.AddModelError("", ex.Message);
                return View(Direccion);
            }
        }

        // GET: Direccion/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Direccion/Delete/5
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