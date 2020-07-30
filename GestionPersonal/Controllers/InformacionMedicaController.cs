using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPSInformation;
using GPSInformation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using static Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary;

namespace GestionInformacionMedical.Controllers
{
    public class InformacionMedicaController : Controller
    {
        private DarkManager darkManager;
        private SelectList Alergias;
        private SelectList TiposSangre;



        public InformacionMedicaController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.InformacionMedica);
            darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);

            
        }

        ~InformacionMedicaController()
        {

        }

        public ActionResult Get(int id)
        {
            var result = darkManager.InformacionMedica.Get(id);
            if (result == null)
                return BadRequest("No se encontro");
            return Ok(result);
        }

       // POST: InformacionMedica/Create
       [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(InformacionMedica InformacionMedica)
        {
            try
            {
                Alergias = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 5, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
                TiposSangre = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 4, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
                if (!ModelState.IsValid)
                {
                    ViewData["Alergias"] = Alergias;
                    ViewData["TiposSangre"] = TiposSangre;
                    return PartialView(InformacionMedica);
                }

                darkManager.InformacionMedica.Element = InformacionMedica;
                bool result = darkManager.InformacionMedica.Add();
                if (result)
                {
                    ViewData["Alergias"] = Alergias;
                    ViewData["TiposSangre"] = TiposSangre;
                    return PartialView("Edit",InformacionMedica);
                }
                else
                {
                    ViewData["Alergias"] = Alergias;
                    ViewData["TiposSangre"] = TiposSangre;
                    return PartialView(InformacionMedica);
                }
                
            }
            catch(GPSInformation.Exceptions.GpExceptions ex)
            {
                ViewData["Alergias"] = Alergias;
                ViewData["TiposSangre"] = TiposSangre;
                ModelState.AddModelError("Error", ex.Message);
                return PartialView(InformacionMedica);
            }
        }

        // POST: InformacionMedica/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(InformacionMedica InformacionMedica)
        {
            try
            {
                Alergias = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 5, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion", InformacionMedica.Alergias);
                TiposSangre = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 4, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion", InformacionMedica.TipoSangre);

                if (!ModelState.IsValid)
                {
                    ViewData["Alergias"] = Alergias;
                    ViewData["TiposSangre"] = TiposSangre;
                    return PartialView(InformacionMedica);
                }

                darkManager.InformacionMedica.Element = InformacionMedica;
                bool result = darkManager.InformacionMedica.Update();
                if (result)
                {
                    ViewData["Alergias"] = Alergias;
                    ViewData["TiposSangre"] = TiposSangre;
                    return PartialView(InformacionMedica);
                }
                else
                {
                    ViewData["Alergias"] = Alergias;
                    ViewData["TiposSangre"] = TiposSangre;
                    return PartialView(InformacionMedica);
                }

            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ViewData["Alergias"] = Alergias;
                ViewData["TiposSangre"] = TiposSangre;
                ModelState.AddModelError("Error", ex.Message);
                return PartialView(InformacionMedica);
            }
        }

    }
}