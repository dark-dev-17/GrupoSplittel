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

namespace GestionEmpleadol.Controllers
{
    public class SplittelEmpleadoController : Controller
    {
        private DarkManager darkManager;
        private SelectList TipoNomina;
        private SelectList EstatusEmpleado;
        private SelectList Puestos;
        private SelectList Sociedades;
        private SelectList Departamentos;



        public SplittelEmpleadoController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.Empleado);
            darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);
            darkManager.LoadObject(GpsManagerObjects.Puesto);
            darkManager.LoadObject(GpsManagerObjects.Sociedad);
            darkManager.LoadObject(GpsManagerObjects.Departamento);
        }

        ~SplittelEmpleadoController()
        {

        }

        public ActionResult Get(int id)
        {
            var result = darkManager.Empleado.Get(id);
            if (result == null)
                return BadRequest("No se encontro");
            return Ok(result);
        }

       // POST: Empleado/Create
       [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Empleado Empleado)
        {
            TipoNomina = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 6, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            EstatusEmpleado = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 7, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");

            Departamentos = new SelectList(darkManager.Departamento.Get().OrderBy(a => a.Nombre).ToList(), "IdDepartamento", "Nombre");
            Puestos = new SelectList(darkManager.Puesto.Get().OrderBy(a => a.Nombre).ToList(), "IdPuesto", "Nombre");
            Sociedades = new SelectList(darkManager.Sociedad.Get().OrderBy(a => a.Descripcion).ToList(), "IdSociedad", "Descripcion");
            ViewData["TipoNomina"] = TipoNomina;
            ViewData["EstatusEmpleado"] = EstatusEmpleado;
            ViewData["Departamentos"] = Departamentos;
            ViewData["Puestos"] = Puestos;
            ViewData["Sociedades"] = Sociedades;

            try
            {
                if (!ModelState.IsValid)
                {
                    return PartialView(Empleado);
                }

                darkManager.Empleado.Element = Empleado;
                darkManager.Empleado.Element.Egreso = DateTime.Now;

                bool result = darkManager.Empleado.Add();
                if (result)
                {
                    return PartialView("Edit",Empleado);
                }
                else
                {
                    return PartialView(Empleado);
                }
            }
            catch(GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return PartialView(Empleado);
            }
        }

        // POST: Empleado/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Empleado Empleado)
        {
            TipoNomina = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 6, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            EstatusEmpleado = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 7, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");

            Departamentos = new SelectList(darkManager.Departamento.Get().OrderBy(a => a.Nombre).ToList(), "IdDepartamento", "Nombre");
            Puestos = new SelectList(darkManager.Puesto.Get().OrderBy(a => a.Nombre).ToList(), "IdPuesto", "Nombre");
            Sociedades = new SelectList(darkManager.Sociedad.Get().OrderBy(a => a.Descripcion).ToList(), "IdSociedad", "Descripcion");
            ViewData["TipoNomina"] = TipoNomina;
            ViewData["EstatusEmpleado"] = EstatusEmpleado;
            ViewData["Departamentos"] = Departamentos;
            ViewData["Puestos"] = Puestos;
            ViewData["Sociedades"] = Sociedades;

            try
            {
                if (!ModelState.IsValid)
                {
                    return PartialView(Empleado);
                }

                darkManager.Empleado.Element = Empleado;
                bool result = darkManager.Empleado.Update();
                if (result)
                {
                    return PartialView(Empleado);
                }
                else
                {
                    return PartialView(Empleado);
                }
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return PartialView(Empleado);
            }
        }

    }
}