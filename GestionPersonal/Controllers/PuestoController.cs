using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionPersonal.Models;
using GPSInformation;
using GPSInformation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class PuestoController : Controller
    {
        private DarkManager darkManager;
        private SelectList Departamentos;
        private SelectList Puestos;
        private SelectList Ubicaciones;



        public PuestoController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.Puesto);
            darkManager.LoadObject(GpsManagerObjects.Departamento);
            darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);

            Departamentos = new SelectList(darkManager.Departamento.Get().OrderBy(a => a.Nombre).ToList(), "IdDepartamento", "Nombre");
            Puestos = new SelectList(darkManager.Puesto.Get().OrderBy(a => a.Nombre).ToList(), "IdPuesto", "Nombre");
            Ubicaciones = new SelectList(darkManager.CatalogoOpcionesValores.Get(""+1, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
        }

        ~PuestoController()
        {

        }

        // GET: Puesto
        [AccessMultipleView(IdAction = new int[] { 16,17 })]
        public ActionResult Index()
        {
            var result = darkManager.Puesto.Get().OrderBy(a => a.Nombre).ToList();
            result.ForEach(a => {
                a.Departamento = darkManager.Departamento.Get(a.IdDepartamento);
            });
            return View(result);
        }

        // GET: Puesto/Details/5
        [AccessMultipleView(IdAction = new int[] { 16, 17 })]
        public ActionResult Details(int id)
        {
            var result = darkManager.Puesto.Get(id);

            if(result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // GET: Puesto/Create
        [AccessMultipleView(IdAction = new int[] { 17 })]
        public ActionResult Create()
        {
            ViewData["Departamentos"] = Departamentos;
            ViewData["Puestos"] = Puestos;
            ViewData["Ubicaciones"] = Ubicaciones;
            return View();
        }

        // POST: Puesto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 17 })]
        public ActionResult Create(Puesto Puesto)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    ViewData["Departamentos"] = Departamentos;
                    ViewData["Puestos"] = Puestos;
                    ViewData["Ubicaciones"] = Ubicaciones;
                    return View(Puesto);
                }

                darkManager.Puesto.Element = Puesto;
                darkManager.Puesto.Element.RequisicionPersonal = 1;
                bool result = darkManager.Puesto.Add();
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["Departamentos"] = Departamentos;
                    ViewData["Puestos"] = Puestos;
                    ViewData["Ubicaciones"] = Ubicaciones;
                    ModelState.AddModelError("", darkManager.GetLastMessage());
                    return View(Puesto);
                }
                
            }
            catch(GPSInformation.Exceptions.GpExceptions ex)
            {
                ViewData["Departamentos"] = Departamentos;
                ViewData["Puestos"] = Puestos;
                ViewData["Ubicaciones"] = Ubicaciones;
                ModelState.AddModelError("", ex.Message);
                return View(Puesto);
            }
        }

        // GET: Puesto/Edit/5
        [AccessMultipleView(IdAction = new int[] { 17 })]
        public ActionResult Edit(int id)
        {
            ViewData["Departamentos"] = Departamentos;
            ViewData["Puestos"] = Puestos;
            ViewData["Ubicaciones"] = Ubicaciones;
            var result = darkManager.Puesto.Get(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // POST: Puesto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 17 })]
        public ActionResult Edit(Puesto Puesto)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    ViewData["Departamentos"] = Departamentos;
                    ViewData["Puestos"] = Puestos;
                    ViewData["Ubicaciones"] = Ubicaciones;
                    return View(Puesto);
                }

                darkManager.Puesto.Element = Puesto;
                bool result = darkManager.Puesto.Update();
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["Departamentos"] = Departamentos;
                    ViewData["Puestos"] = Puestos;
                    ViewData["Ubicaciones"] = Ubicaciones;
                    ModelState.AddModelError("", darkManager.GetLastMessage());
                    return View(Puesto);
                }

            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ViewData["Departamentos"] = Departamentos;
                ViewData["Puestos"] = Puestos;
                ViewData["Ubicaciones"] = Ubicaciones;
                ModelState.AddModelError("", ex.Message);
                return View(Puesto);
            }
        }

        // GET: Puesto/Delete/5
        [AccessMultipleView(IdAction = new int[] { 17 })]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Puesto/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 17 })]
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