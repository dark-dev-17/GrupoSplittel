﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionPersonal.Models;
using GPSInformation;
using GPSInformation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using static Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary;

namespace GestionPersonaContactol.Controllers
{
    public class PersonaContactoController : Controller
    {
        private DarkManager darkManager;
        private SelectList Parentezcos;

        public PersonaContactoController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.PersonaContacto);
            darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);
        }

        ~PersonaContactoController()
        {

        }
        [AccessMultipleView(IdAction = new int[] { 19,20 })]
        public ActionResult List(int id)
        {
            var result = darkManager.PersonaContacto.Get("" + id, "IdPersona");
            return PartialView(result);
        }

        // POST: PersonaContacto/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 20 })]
        public ActionResult Create(PersonaContacto PersonaContacto)
        {
            Parentezcos = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 9, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["Parentezcos"] = Parentezcos;
            

            try
            {
                if (!ModelState.IsValid)
                {
                    return PartialView(PersonaContacto);
                }

                darkManager.PersonaContacto.Element = PersonaContacto;

                bool result = darkManager.PersonaContacto.Add();
                if (result)
                {
                    return PartialView();
                }
                else
                {
                    return PartialView(PersonaContacto);
                }
            }
            catch(GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView(PersonaContacto);
            }
        }

        // POST: PersonaContacto/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 20 })]
        public ActionResult Edit(PersonaContacto PersonaContacto)
        {
            Parentezcos = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 9, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion", PersonaContacto.IdParentezco);
            ViewData["Parentezcos"] = Parentezcos;

            try
            {
                if (!ModelState.IsValid)
                {
                    return PartialView(PersonaContacto);
                }

                darkManager.PersonaContacto.Element = PersonaContacto;
                bool result = darkManager.PersonaContacto.Update();
                if (result)
                {
                    return PartialView(PersonaContacto);
                }
                else
                {
                    return PartialView(PersonaContacto);
                }
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView(PersonaContacto);
            }
        }

    }
}