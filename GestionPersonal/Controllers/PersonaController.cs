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
    public class PersonaController : Controller
    {
        private DarkManager darkManager;
        private SelectList Generos;
        private SelectList EstadosCiviles;



        public PersonaController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.Persona);
            darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);

            Generos = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 2, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            EstadosCiviles = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 3, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
        }

        ~PersonaController()
        {

        }

        // GET: Persona
        public ActionResult Index()
        {
            var result = darkManager.Persona.Get().OrderBy(a => a.Nombre).ToList();
            //result.ForEach(a => {
            //    a.Departamento = darkManager.Departamento.Get(a.IdDepartamento);
            //});
            return View(result);
        }

        // GET: Persona/Details/5
        public ActionResult Details(int id)
        {
            var result = darkManager.Persona.Get(id);

            if(result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // GET: Persona/Create
        public ActionResult Create()
        {
            ViewData["Generos"] = Generos;
            ViewData["EstadosCiviles"] = EstadosCiviles;
            return View();
        }

        // POST: Persona/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Persona Persona)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    ViewData["Generos"] = Generos;
                    ViewData["EstadosCiviles"] = EstadosCiviles;
                    return View(Persona);
                }

                darkManager.Persona.Element = Persona;
                bool result = darkManager.Persona.Add();
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["Generos"] = Generos;
                    ViewData["EstadosCiviles"] = EstadosCiviles;
                    ModelState.AddModelError("", darkManager.GetLastMessage());
                    return View(Persona);
                }
                
            }
            catch(GPSInformation.Exceptions.GpExceptions ex)
            {
                ViewData["Generos"] = Generos;
                ViewData["EstadosCiviles"] = EstadosCiviles;
                ModelState.AddModelError("", ex.Message);
                return View(Persona);
            }
        }

        // GET: Persona/Edit/5
        public ActionResult Edit(int id)
        {
            ViewData["Generos"] = Generos;
            ViewData["EstadosCiviles"] = EstadosCiviles;
            var result = darkManager.Persona.Get(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // POST: Persona/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Persona Persona)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    ViewData["Generos"] = Generos;
                    ViewData["EstadosCiviles"] = EstadosCiviles;
                    return View(Persona);
                }

                darkManager.Persona.Element = Persona;
                bool result = darkManager.Persona.Update();
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["Generos"] = Generos;
                    ViewData["EstadosCiviles"] = EstadosCiviles;
                    ModelState.AddModelError("", darkManager.GetLastMessage());
                    return View(Persona);
                }

            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ViewData["Generos"] = Generos;
                ViewData["EstadosCiviles"] = EstadosCiviles;
                ModelState.AddModelError("", ex.Message);
                return View(Persona);
            }
        }

        // GET: Persona/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Persona/Delete/5
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