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

            Generos = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 2, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            EstadosCiviles = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 3, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            Alergias = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 5, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            TiposSangre = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 4, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            TipoNomina = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 6, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            EstatusEmpleado = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 7, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            Departamentos = new SelectList(darkManager.Departamento.Get().OrderBy(a => a.Nombre).ToList(), "IdDepartamento", "Nombre");
            Puestos = new SelectList(darkManager.Puesto.Get().OrderBy(a => a.Nombre).ToList(), "IdPuesto", "Nombre");
            Sociedades = new SelectList(darkManager.Sociedad.Get().OrderBy(a => a.Descripcion).ToList(), "IdSociedad", "Descripcion");
        }

        ~EmpleadoController()
        {

        }

        // GET: Empleado
        public ActionResult Index()
        {
            var result = darkManager.Persona.Get().OrderBy(a => a.Nombre).ToList();
            return View(result);
        }

        // GET: Empleado/Create
        public ActionResult Create()
        {
            ViewData["Generos"] = Generos;
            ViewData["EstadosCiviles"] = EstadosCiviles;
            ViewData["Alergias"] = Alergias;
            ViewData["TiposSangre"] = TiposSangre;
            ViewData["TipoNomina"] = TipoNomina;
            ViewData["EstatusEmpleado"] = EstatusEmpleado;
            ViewData["Departamentos"] = Departamentos;
            ViewData["Puestos"] = Puestos;
            ViewData["Sociedades"] = Sociedades;
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
                ModelState.AddModelError("Error", ex.Message);
                return View(Persona);
            }
        }
        // GET: Empleado/Edit
        public ActionResult Edit(int id)
        {
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

            var InforMedica = darkManager.InformacionMedica.GetByColumn("" + result.IdPersona, "IdPersona");
            ViewData["InfoMedica"] = InforMedica;

            var InforEmpleado = darkManager.Empleado.GetByColumn("" + result.IdPersona, "IdPersona");
            ViewData["InforEmpleado"] = InforEmpleado;

            return View(result);
        }
    }
}