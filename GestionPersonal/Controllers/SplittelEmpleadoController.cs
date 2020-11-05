using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionPersonal.Models;
using GPSInformation;
using GPSInformation.Models;
using GPSInformation.Tools;
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
        [AccessMultipleView(IdAction = new int[] { 19,20 })]
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
        [AccessMultipleView(IdAction = new int[] { 20 })]
        public ActionResult Create(Empleado Empleado)
        {
            TipoNomina = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 6, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion", Empleado.TipoNomina);
            EstatusEmpleado = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 7, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion",Empleado.IdEstatus);

            Departamentos = new SelectList(darkManager.Departamento.Get().OrderBy(a => a.Nombre).ToList(), "IdDepartamento", "Nombre", Empleado.IdDepartamento);
            Puestos = new SelectList(darkManager.Puesto.Get().OrderBy(a => a.Nombre).ToList(), "IdPuesto", "Nombre", Empleado.IdPuesto);
            Sociedades = new SelectList(darkManager.Sociedad.Get().OrderBy(a => a.Descripcion).ToList(), "IdSociedad", "Descripcion", Empleado.IdSociedad);
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

                string emailValid = Funciones.ValidateEmail(Empleado.Email);
                if (emailValid != "")
                {
                    ModelState.AddModelError("Email", string.Format("El correo: '{0}', no es valido", emailValid));
                    return PartialView(Empleado);
                }

                if (Empleado.NumeroNomina == 0)
                {
                    int max = (int)darkManager.Empleado.GetMax("NumeroNomina", "NumeroNomina", Empleado.TipoNomina + "");
                    Empleado.NumeroNomina = max + 1;
                }
                else
                {
                    var emple = darkManager.Empleado.Get("NumeroNomina", Empleado.NumeroNomina + "", "TipoNomina", Empleado.TipoNomina + "");
                    if (emple != null)
                    {
                        int max = (int)darkManager.Empleado.GetMax("NumeroNomina", "NumeroNomina", Empleado.TipoNomina + "");
                        ModelState.AddModelError("NumeroNomina", string.Format("El número de nomina '{0}' ya esta siendo utilizado por otro empleado, número disponible: '{0}'",
                            Empleado.TipoNomina, max + 1));
                        return PartialView(Empleado);
                    }
                }

                darkManager.Empleado.Element = Empleado;
                darkManager.Empleado.Element.Egreso = DateTime.Now;
                darkManager.Empleado.Element.Actualizado = DateTime.Now;
                darkManager.Empleado.Element.Creado = DateTime.Now;
                bool result = darkManager.Empleado.Add();
                if (result)
                {
                    return PartialView("Edit", darkManager.Empleado.Get(darkManager.Empleado.GetLastId()));
                }
                else
                {
                    return PartialView(Empleado);
                }
            }
            catch(GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView(Empleado);
            }
        }

        // POST: Empleado/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 20 })]
        public ActionResult Edit(Empleado Empleado)
        {
            TipoNomina = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 6, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion", Empleado.TipoNomina);
            EstatusEmpleado = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 7, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion", Empleado.IdEstatus);

            Departamentos = new SelectList(darkManager.Departamento.Get().OrderBy(a => a.Nombre).ToList(), "IdDepartamento", "Nombre", Empleado.IdDepartamento);
            Puestos = new SelectList(darkManager.Puesto.Get().OrderBy(a => a.Nombre).ToList(), "IdPuesto", "Nombre", Empleado.IdPuesto);
            Sociedades = new SelectList(darkManager.Sociedad.Get().OrderBy(a => a.Descripcion).ToList(), "IdSociedad", "Descripcion", Empleado.IdSociedad);
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

                string emailValid = Funciones.ValidateEmail(Empleado.Email);
                if (emailValid != "")
                {
                    ModelState.AddModelError("Email", string.Format("El correo: '{0}', no es valido", emailValid));
                    return PartialView(Empleado);
                }
                if (Empleado.NumeroNomina == 0)
                {
                    int max = (int)darkManager.Empleado.GetMax("NumeroNomina", "TipoNomina", Empleado.TipoNomina + "");
                    Empleado.NumeroNomina = max + 1;
                }
                else
                {
                    var emple = darkManager.Empleado.Get("NumeroNomina", Empleado.NumeroNomina + "", "TipoNomina", Empleado.TipoNomina + "");
                    if (emple != null && emple.IdEmpleado != Empleado.IdEmpleado)
                    {
                        int max = (int)darkManager.Empleado.GetMax("NumeroNomina", "TipoNomina", Empleado.TipoNomina + "");
                        ModelState.AddModelError("NumeroNomina", string.Format("El número de nomina '{0}' ya esta siendo utilizado por otro empleado, número disponible: '{1}'",
                            Empleado.NumeroNomina, max + 1));
                        return PartialView(Empleado);
                    }
                }
                darkManager.Empleado.Element = Empleado;
                darkManager.Empleado.Element.Egreso = DateTime.Now;
                darkManager.Empleado.Element.Actualizado = DateTime.Now;
                darkManager.Empleado.Element.Creado = DateTime.Now;
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
                ModelState.AddModelError("", ex.Message);
                return PartialView(Empleado);
            }
        }

    }
}