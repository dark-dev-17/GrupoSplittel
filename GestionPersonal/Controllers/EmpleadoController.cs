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
    public class EmpleadoController : Controller
    {
        private DarkManager darkManager;
        private SelectList Generos;
        private SelectList EstadosCiviles;
        private SelectList Alergias;
        private SelectList TiposSangre;
        private SelectList TipoNomina;
        private SelectList EstatusEmpleado;
        private SelectList Puestos;
        private SelectList Sociedades;
        private SelectList Departamentos;
        private SelectList Parentezcos;

        public EmpleadoController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.Persona);
            darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);
            darkManager.LoadObject(GpsManagerObjects.InformacionMedica);
            darkManager.LoadObject(GpsManagerObjects.Puesto);
            darkManager.LoadObject(GpsManagerObjects.Sociedad);
            darkManager.LoadObject(GpsManagerObjects.Departamento);
            darkManager.LoadObject(GpsManagerObjects.Empleado);
            darkManager.LoadObject(GpsManagerObjects.PersonaContacto);
            darkManager.LoadObject(GpsManagerObjects.View_empleado);
        }

        ~EmpleadoController()
        {

        }

        // GET: Empleado
        [AccessMultipleView(IdAction = new int[] { 19,20 })]
        public ActionResult Index()
        {
            var result = darkManager.View_empleado.Get().OrderBy(a => a.NombreCompleto).ToList();
            //ViewData["Puestos"] = darkManager.Puesto.Get().OrderBy(a => a.Nombre).ToList();
            //ViewData["Empleados"] = darkManager.Empleado.Get().OrderBy(a => a.NumeroNomina).ToList();
            return View(result);
        }

        // GET: Empleado/Create
        [AccessMultipleView(IdAction = new int[] { 20 })]
        public ActionResult Create()
        {
            GetSelects();
            ViewData["Generos"] = Generos;
            ViewData["EstadosCiviles"] = EstadosCiviles;
            ViewData["Alergias"] = Alergias;
            ViewData["TiposSangre"] = TiposSangre;
            ViewData["TipoNomina"] = TipoNomina;
            ViewData["EstatusEmpleado"] = EstatusEmpleado;
            ViewData["Departamentos"] = Departamentos;
            ViewData["Puestos"] = Puestos;
            ViewData["Sociedades"] = Sociedades;
            ViewData["Parentezcos"] = Parentezcos;
            return View();
        }
        // POST: Persona/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 20 })]
        public ActionResult Create(Persona Persona)
        {
            GetSelects();
            try
            {

                if (!ModelState.IsValid)
                {
                    ViewData["Generos"] = Generos;
                    ViewData["EstadosCiviles"] = EstadosCiviles;
                    return View(Persona);
                }
                // agregar validacion cuando numero de nomina este en 0 o vacio

                Persona.Creado = DateTime.Now;
                Persona.Actualizado = DateTime.Now;
                darkManager.Persona.Element = Persona;
                bool result = darkManager.Persona.Add();
                if (result)
                {
                    return RedirectToAction("Edit", "Empleado", new { id = darkManager.Persona.GetLastId() });
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
        // GET: Empleado/Edit
        [AccessMultipleView(IdAction = new int[] { 20 })]
        public ActionResult Edit(int id)
        {
            GetSelects();
            var result = darkManager.Persona.Get(id);
            if (result == null)
                return NotFound();
            ViewData["Generos"] = Generos;
            ViewData["EstadosCiviles"] = EstadosCiviles;
            ViewData["Alergias"] = Alergias;
            ViewData["TiposSangre"] = TiposSangre;
            ViewData["TipoNomina"] = TipoNomina;
            ViewData["EstatusEmpleado"] = EstatusEmpleado;
            ViewData["Departamentos"] = Departamentos;
            ViewData["Puestos"] = Puestos;
            ViewData["Sociedades"] = Sociedades;
            ViewData["Parentezcos"] = Parentezcos;
            var InforMedica = darkManager.InformacionMedica.GetByColumn("" + result.IdPersona, "IdPersona");
            ViewData["InfoMedica"] = InforMedica;

            var InforEmpleado = darkManager.Empleado.GetByColumn("" + result.IdPersona, "IdPersona");
            ViewData["InforEmpleado"] = InforEmpleado;

            var PersonaContacto = darkManager.PersonaContacto.Get("" + result.IdPersona, "IdPersona");
            ViewData["PersonaContacto"] = PersonaContacto;

            return View(result);
        }
        private void GetSelects()
        {
            Generos = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 2, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            EstadosCiviles = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 3, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            Alergias = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 5, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            TiposSangre = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 4, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            TipoNomina = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 6, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            EstatusEmpleado = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 7, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            Departamentos = new SelectList(darkManager.Departamento.Get().OrderBy(a => a.Nombre).ToList(), "IdDepartamento", "Nombre");
            Puestos = new SelectList(darkManager.Puesto.Get().OrderBy(a => a.Nombre).ToList(), "IdPuesto", "Nombre");
            Sociedades = new SelectList(darkManager.Sociedad.Get().OrderBy(a => a.Descripcion).ToList(), "IdSociedad", "Descripcion");
            Parentezcos = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 9, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
        }
    }
}