using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionPersonal.Models;
using GPSInformation;
using GPSInformation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class SociedadController : Controller
    {
        private DarkManager darkManager;
        public SociedadController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.Sociedad);
        }

        ~SociedadController()
        {

        }

        // GET: Sociedad
        [AccessMultipleView(IdAction = new int[] { 10,11 })]
        public ActionResult Index()
        {
            var result = darkManager.Sociedad.Get().OrderBy(a => a.Descripcion).ToList();
            return View(result);
        }

        // GET: Sociedad/Details/5
        [AccessMultipleView(IdAction = new int[] { 10, 11 })]
        public ActionResult Details(int id)
        {
            var result = darkManager.Sociedad.Get(id);
            if(result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // GET: Sociedad/Create
        [AccessMultipleView(IdAction = new int[] { 11 })]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sociedad/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 11 })]
        public ActionResult Create(Sociedad Sociedad)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View(Sociedad);
                }

                darkManager.Sociedad.Element = Sociedad;
                bool result = darkManager.Sociedad.Add();
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", darkManager.GetLastMessage());
                    return View(Sociedad);
                }
                
            }
            catch(GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(Sociedad);
            }
        }

        // GET: Sociedad/Edit/5
        [AccessMultipleView(IdAction = new int[] { 11 })]
        public ActionResult Edit(int id)
        {
            var result = darkManager.Sociedad.Get(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // POST: Sociedad/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 11 })]
        public ActionResult Edit(Sociedad Sociedad)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View(Sociedad);
                }

                darkManager.Sociedad.Element = Sociedad;
                bool result = darkManager.Sociedad.Update();
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", darkManager.GetLastMessage());
                    return View(Sociedad);
                }

            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(Sociedad);
            }
        }

        // GET: Sociedad/Delete/5
        [AccessMultipleView(IdAction = new int[] { 11 })]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Sociedad/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 11 })]
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