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


        }

        ~PersonaController()
        {

        }

        // POST: Persona/Create
        [HttpPost]
        public ActionResult CreateData(Persona Persona)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                darkManager.Persona.Element = Persona;
                bool result = darkManager.Persona.Add();
                if (result)
                {
                    return Ok(darkManager.GetLastMessage());
                }
                else
                {
                    return BadRequest(darkManager.GetLastMessage());
                }

            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: Persona/Edit/5
        [HttpPost]
        public ActionResult Edit(Persona Persona)
        {
            try
            {
                Generos = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 2, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion", Persona.IdGenero);
                EstadosCiviles = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 3, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion", Persona.IdEstadoCivil);

                if (!ModelState.IsValid)
                {
                    ViewData["Generos"] = Generos;
                    ViewData["EstadosCiviles"] = EstadosCiviles;
                    return PartialView(Persona);
                }

                darkManager.Persona.Element = Persona;
                bool result = darkManager.Persona.Update();
                if (result)
                {
                    ViewData["Generos"] = Generos;
                    ViewData["EstadosCiviles"] = EstadosCiviles;
                    return PartialView(Persona);
                }
                else
                {
                    ViewData["Generos"] = Generos;
                    ViewData["EstadosCiviles"] = EstadosCiviles;
                    return PartialView(Persona);
                }

            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ViewData["Generos"] = Generos;
                ViewData["EstadosCiviles"] = EstadosCiviles;
                ModelState.AddModelError("Error", ex.Message);
                return PartialView(Persona);
            }
        }
    }
}