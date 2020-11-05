using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionPersonal.Models;
using GPSInformation;
using GPSInformation.Exceptions;
using GPSInformation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class ProspectoController : Controller
    {
        private DarkManager darkManager;
        private SelectList Generos;
        private SelectList EstadosCiviles;
        private SelectList EstusPros;
        private SelectList Puestos;

        public ProspectoController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.Persona);
            darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);
            darkManager.LoadObject(GpsManagerObjects.Puesto);
        }

        // GET: ProspectoController
        public ActionResult Index()
        {
            var personas = darkManager.Persona.Get("4", "Empleado");
            personas.ForEach(a => { 
            
            });
            return View(personas);
        }

        // GET: ProspectoController/Details/5
        public ActionResult Details(int id)
        {
            var Persona = darkManager.Persona.Get(id);
            AddSelects(Persona.IdGenero, Persona.IdEstadoCivil, Persona.IdEstatusPros, Persona.IdPuesto);
            return View(Persona);
        }

        // GET: ProspectoController/Create
        public ActionResult Create()
        {
            AddSelects(0, 0, 0, 0);
            return View();
        }

        // POST: ProspectoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Persona Persona)
        {
            darkManager.StartTransaction();
            try
            {
                if (!ModelState.IsValid)
                {
                    AddSelects(Persona.IdGenero, Persona.IdEstadoCivil, Persona.IdEstatusPros, Persona.IdPuesto);
                    return View(Persona);
                }

                if(Persona.IdPuesto == 0)
                {
                    ModelState.AddModelError("IdPuesto", "Por favor selecciona un puesto a aplicar");
                    AddSelects(Persona.IdGenero, Persona.IdEstadoCivil, Persona.IdEstatusPros, Persona.IdPuesto);
                    return View(Persona);
                }

                if (Persona.IdEstatusPros == 0)
                {
                    ModelState.AddModelError("IdEstatusPros", "Por favor selecciona una opción");
                    AddSelects(Persona.IdGenero, Persona.IdEstadoCivil, Persona.IdEstatusPros, Persona.IdPuesto);
                    return View(Persona);
                }
                Persona.Creado = DateTime.Now;
                Persona.Actualizado = DateTime.Now;
                Persona.Empleado = 4;
                darkManager.Persona.Element = Persona;

                if (!darkManager.Persona.Add())
                {
                    throw new GpExceptions("error al guardar");
                }
                darkManager.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                AddSelects(Persona.IdGenero, Persona.IdEstadoCivil, Persona.IdEstatusPros, Persona.IdPuesto);
                darkManager.RolBack();
                return View(Persona);
            }
        }

        // GET: ProspectoController/Edit/5
        public ActionResult Edit(int id)
        {
            var Persona = darkManager.Persona.Get(id);
            AddSelects(Persona.IdGenero, Persona.IdEstadoCivil, Persona.IdEstatusPros, Persona.IdPuesto);
            return View(Persona);
        }

        // POST: ProspectoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Persona Persona)
        {
            darkManager.StartTransaction();
            try
            {
                if (!ModelState.IsValid)
                {
                    AddSelects(Persona.IdGenero, Persona.IdEstadoCivil, Persona.IdEstatusPros, Persona.IdPuesto);
                    return View(Persona);
                }

                if (Persona.IdPuesto == 0)
                {
                    ModelState.AddModelError("IdPuesto", "Por favor selecciona un puesto a aplicar");
                    AddSelects(Persona.IdGenero, Persona.IdEstadoCivil, Persona.IdEstatusPros, Persona.IdPuesto);
                    return View(Persona);
                }

                if (Persona.IdEstatusPros == 0)
                {
                    ModelState.AddModelError("IdEstatusPros", "Por favor selecciona una opción");
                    AddSelects(Persona.IdGenero, Persona.IdEstadoCivil, Persona.IdEstatusPros, Persona.IdPuesto);
                    return View(Persona);
                }
                Persona.Creado = DateTime.Now;
                Persona.Actualizado = DateTime.Now;
                darkManager.Persona.Element = Persona;

                if (!darkManager.Persona.Update())
                {
                    throw new GpExceptions("error al guardar");
                }
                darkManager.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                AddSelects(Persona.IdGenero, Persona.IdEstadoCivil, Persona.IdEstatusPros, Persona.IdPuesto);
                darkManager.RolBack();
                return View(Persona);
            }
        }

        // GET: ProspectoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProspectoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        private void AddSelects(int IdGeneros, int IdEstadosCiviles, int IDEstusPros, int IdPuestos)
        {
            Generos = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 2, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion", IdGeneros);
            EstadosCiviles = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 3, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion", IdEstadosCiviles);
            EstusPros = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 1019, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion", IDEstusPros);
            Puestos = new SelectList(darkManager.Puesto.Get().OrderBy(a => a.Nombre).ToList(), "IdPuesto", "Nombre", IdPuestos);
            ViewData["Generos"] = Generos;
            ViewData["EstusPros"] = EstusPros;
            ViewData["EstadosCiviles"] = EstadosCiviles;
            ViewData["Puestos"] = Puestos;

        }
    }
}
