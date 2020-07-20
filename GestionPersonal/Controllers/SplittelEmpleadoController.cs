using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPDataInformation;
using GPDataInformation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class SplittelEmpleadoController : Controller
    {
        private GPDataInformation.GpsManager GpsManager;

        private List<CatalogoOpcionesValores> Generos;
        private List<CatalogoOpcionesValores> EstadosCiviles;
        private List<CatalogoOpcionesValores> TipoSangre;
        private List<CatalogoOpcionesValores> Alergia;
        private List<CatalogoOpcionesValores> TipoNomina;
        private List<Sociedad> Sociedades;
        private List<Departamento> Departamentos;
        private List<Puesto> Puestos;

        public SplittelEmpleadoController(IConfiguration configuration)
        {
            GpsManager = new GPDataInformation.GpsManager(configuration);
            GpsManager.OpenConnection();
            GpsManager.LoadObject(GPDataInformation.GpsManagerObjects.Persona);
            GpsManager.LoadObject(GPDataInformation.GpsManagerObjects.InformacionMedica);
            GpsManager.LoadObject(GPDataInformation.GpsManagerObjects.Empleado);
            GpsManager.LoadObject(GPDataInformation.GpsManagerObjects.PersonaContacto);
            GpsManager.LoadObject(GPDataInformation.GpsManagerObjects.Sociedad);
            GpsManager.LoadObject(GPDataInformation.GpsManagerObjects.Departamento);
            GpsManager.LoadObject(GPDataInformation.GpsManagerObjects.Puesto);

            Generos = GpsManager.CatalogoOpcionesValores.Get(a => a.IdCatalogoOpciones == 2);
            EstadosCiviles = GpsManager.CatalogoOpcionesValores.Get(a => a.IdCatalogoOpciones == 3);
            TipoSangre = GpsManager.CatalogoOpcionesValores.Get(a => a.IdCatalogoOpciones == 4);
            Alergia = GpsManager.CatalogoOpcionesValores.Get(a => a.IdCatalogoOpciones == 5);
            TipoNomina = GpsManager.CatalogoOpcionesValores.Get(a => a.IdCatalogoOpciones == 6);

            Sociedades = GpsManager.Sociedad.Get().OrderBy(a => a.Descripcion).ToList();
            Departamentos = GpsManager.Departamento.Get().OrderBy(a => a.Nombre).ToList();
            Puestos = GpsManager.Puesto.Get().OrderBy(a => a.Nombre).ToList();

        }
        ~SplittelEmpleadoController()
        {
            SomeMethod();
        }

        // GET: SplittelEmpleado
        public ActionResult Index()
        {
            return View(GpsManager.Persona.Get().OrderBy(a => a.ApellidoPaterno));
        }

        public ActionResult Chech()
        {
            return Ok(GpsManager.CatalogoOpciones.Get());
        }

        // GET: SplittelEmpleado/Create
        public ActionResult Create()
        {
            ViewData["Generos"] = new SelectList(Generos, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["EstadosCiviles"] = new SelectList(EstadosCiviles, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["TipoSangre"] = new SelectList(TipoSangre, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["Alergia"] = new SelectList(Alergia, "IdCatalogoOpcionesValores", "Descripcion");
            return View(new SplittelEmpleado());
        }

        // GET: SplittelEmpleado/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewData["Generos"] = new SelectList(Generos, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["EstadosCiviles"] = new SelectList(EstadosCiviles, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["TipoSangre"] = new SelectList(TipoSangre, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["Alergia"] = new SelectList(Alergia, "IdCatalogoOpcionesValores", "Descripcion");

            ViewData["TipoNomina"] = new SelectList(Alergia, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["Sociedades"] = new SelectList(Sociedades, "IdSociedad", "Descripcion");
            ViewData["Departamentos"] = new SelectList(Departamentos, "IdDepartamento", "Nombre");
            ViewData["Puestos"] = new SelectList(Puestos, "IdPuesto", "Nombre");
            if (id == null)
            {
                return NotFound();
            }
            var respuesta = GpsManager.Persona.Get(id);
            if(respuesta == null)
            {
                return NotFound();
            }
            SplittelEmpleado splittelEmpleado = new SplittelEmpleado();
            splittelEmpleado.persona = respuesta;
            var respuestaMed = GpsManager.InformacionMedica.GetByColumn(respuesta.IdPersona+"", "IdPersona");
            splittelEmpleado.informacionMedica = (respuestaMed == null) ? new InformacionMedica() { IdPersona = respuesta.IdPersona } : respuestaMed;
            var respuestaEmp = GpsManager.Empleado.GetByColumn(respuesta.IdPersona + "", "IdPersona");
            splittelEmpleado.empleado = (respuestaEmp == null) ? new Empleado() { IdPersona = respuesta.IdPersona } : respuestaEmp;
            var respuestaPersonas = GpsManager.PersonaContacto.Get(respuesta.IdPersona + "", "IdPersona");
            splittelEmpleado.PersonaContacto = (respuestaPersonas == null) ? new List<PersonaContacto>() : respuestaPersonas;
            return View(splittelEmpleado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePersona(Persona Persona)
        {
            ViewData["Generos"] = new SelectList(Generos, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["EstadosCiviles"] = new SelectList(EstadosCiviles, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["TipoSangre"] = new SelectList(TipoSangre, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["Alergia"] = new SelectList(Alergia, "IdCatalogoOpcionesValores", "Descripcion");

            ViewData["TipoNomina"] = new SelectList(Alergia, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["Sociedades"] = new SelectList(Sociedades, "IdSociedad", "Descripcion");
            ViewData["Departamentos"] = new SelectList(Departamentos, "IdDepartamento", "Nombre");
            ViewData["Puestos"] = new SelectList(Puestos, "IdPuesto", "Nombre");
            try
            {
                if (ModelState.IsValid)
                {
                    GpsManager.Persona.Element = Persona;
                    var result = GpsManager.Persona.Add();
                    if(result == false)
                    {
                        ModelState.AddModelError("", GpsManager.GetLastMessage());
                        return View("Create", new SplittelEmpleado { persona = Persona });
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View("Create", new SplittelEmpleado { persona = Persona });
                }
            }
            catch (GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Create", new SplittelEmpleado { persona = Persona });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPersona(Persona Persona)
        {
            ViewData["Generos"] = new SelectList(Generos, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["EstadosCiviles"] = new SelectList(EstadosCiviles, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["TipoSangre"] = new SelectList(TipoSangre, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["Alergia"] = new SelectList(Alergia, "IdCatalogoOpcionesValores", "Descripcion");

            ViewData["TipoNomina"] = new SelectList(Alergia, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["Sociedades"] = new SelectList(Sociedades, "IdSociedad", "Descripcion");
            ViewData["Departamentos"] = new SelectList(Departamentos, "IdDepartamento", "Nombre");
            ViewData["Puestos"] = new SelectList(Puestos, "IdPuesto", "Nombre");
            try
            {
                if (ModelState.IsValid)
                {
                    GpsManager.Persona.Element = Persona;
                    var result = GpsManager.Persona.Update();
                    if (result == false)
                    {
                        ModelState.AddModelError("", GpsManager.GetLastMessage());
                        return View("Edit", new SplittelEmpleado { persona = Persona });
                    }
                    return RedirectToAction(nameof(Edit), new { id = Persona.IdPersona });
                }
                else
                {
                    return View("Edit", new SplittelEmpleado { persona = Persona });
                }
            }
            catch (GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Edit", new SplittelEmpleado { persona = Persona });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateInformacionMedica(InformacionMedica InformacionMedica)
        {
            ViewData["Generos"] = new SelectList(Generos, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["EstadosCiviles"] = new SelectList(EstadosCiviles, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["TipoSangre"] = new SelectList(TipoSangre, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["Alergia"] = new SelectList(Alergia, "IdCatalogoOpcionesValores", "Descripcion");


            ViewData["TipoNomina"] = new SelectList(Alergia, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["Sociedades"] = new SelectList(Sociedades, "IdSociedad", "Descripcion");
            ViewData["Departamentos"] = new SelectList(Departamentos, "IdDepartamento", "Nombre");
            ViewData["Puestos"] = new SelectList(Puestos, "IdPuesto", "Nombre");
            try
            {
                if (ModelState.IsValid)
                {
                    GpsManager.InformacionMedica.Element = InformacionMedica;
                    var result = GpsManager.InformacionMedica.Add();
                    if (result == false)
                    {
                        ModelState.AddModelError("", GpsManager.GetLastMessage());
                        return View("Edit", new SplittelEmpleado { persona = GpsManager.Persona.Get(InformacionMedica.IdPersona), informacionMedica = InformacionMedica });
                    }
                    return RedirectToAction(nameof(Edit), new { id = InformacionMedica.IdPersona });
                }
                else
                {
                    return View("Edit", new SplittelEmpleado { persona = GpsManager.Persona.Get(InformacionMedica.IdPersona), informacionMedica = InformacionMedica });
                }
            }
            catch (GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Create", new SplittelEmpleado { persona = GpsManager.Persona.Get(InformacionMedica.IdPersona), informacionMedica = InformacionMedica });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditInformacionMedica(InformacionMedica InformacionMedica)
        {
            ViewData["Generos"] = new SelectList(Generos, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["EstadosCiviles"] = new SelectList(EstadosCiviles, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["TipoSangre"] = new SelectList(TipoSangre, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["Alergia"] = new SelectList(Alergia, "IdCatalogoOpcionesValores", "Descripcion");

            ViewData["TipoNomina"] = new SelectList(Alergia, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["Sociedades"] = new SelectList(Sociedades, "IdSociedad", "Descripcion");
            ViewData["Departamentos"] = new SelectList(Departamentos, "IdDepartamento", "Nombre");
            ViewData["Puestos"] = new SelectList(Puestos, "IdPuesto", "Nombre");
            try
            {
                if (ModelState.IsValid)
                {
                    GpsManager.InformacionMedica.Element = InformacionMedica;
                    var result = GpsManager.InformacionMedica.Update();
                    if (result == false)
                    {
                        ModelState.AddModelError("", GpsManager.GetLastMessage());
                        return View("Edit", new SplittelEmpleado { persona = GpsManager.Persona.Get(InformacionMedica.IdPersona), informacionMedica = InformacionMedica });
                    }
                    return RedirectToAction(nameof(Edit), new {  id = InformacionMedica.IdPersona });
                }
                else
                {
                    return View("Edit", new SplittelEmpleado { persona = GpsManager.Persona.Get(InformacionMedica.IdPersona), informacionMedica = InformacionMedica });
                }
            }
            catch (GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Edit", new SplittelEmpleado { persona = GpsManager.Persona.Get(InformacionMedica.IdPersona), informacionMedica = InformacionMedica });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmpleado(Empleado Empleado)
        {
            ViewData["Generos"] = new SelectList(Generos, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["EstadosCiviles"] = new SelectList(EstadosCiviles, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["TipoSangre"] = new SelectList(TipoSangre, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["Alergia"] = new SelectList(Alergia, "IdCatalogoOpcionesValores", "Descripcion");

            ViewData["TipoNomina"] = new SelectList(Alergia, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["Sociedades"] = new SelectList(Sociedades, "IdSociedad", "Descripcion");
            ViewData["Departamentos"] = new SelectList(Departamentos, "IdDepartamento", "Nombre");
            ViewData["Puestos"] = new SelectList(Puestos, "IdPuesto", "Nombre");
            try
            {
                if (ModelState.IsValid)
                {
                    GpsManager.Empleado.Element = Empleado;
                    var result = GpsManager.InformacionMedica.Update();
                    if (result == false)
                    {
                        ModelState.AddModelError("", GpsManager.GetLastMessage());
                        return View("Edit", new SplittelEmpleado { persona = GpsManager.Persona.Get(Empleado.IdPersona), empleado = Empleado });
                    }
                    return RedirectToAction(nameof(Edit), new { id = Empleado.IdPersona });
                }
                else
                {
                    return View("Edit", new SplittelEmpleado { persona = GpsManager.Persona.Get(Empleado.IdPersona), empleado = Empleado });
                }
            }
            catch (GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Edit", new SplittelEmpleado { persona = GpsManager.Persona.Get(Empleado.IdPersona), empleado = Empleado });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEmpleado(Empleado Empleado)
        {
            ViewData["Generos"] = new SelectList(Generos, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["EstadosCiviles"] = new SelectList(EstadosCiviles, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["TipoSangre"] = new SelectList(TipoSangre, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["Alergia"] = new SelectList(Alergia, "IdCatalogoOpcionesValores", "Descripcion");

            ViewData["TipoNomina"] = new SelectList(Alergia, "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["Sociedades"] = new SelectList(Sociedades, "IdSociedad", "Descripcion");
            ViewData["Departamentos"] = new SelectList(Departamentos, "IdDepartamento", "Nombre");
            ViewData["Puestos"] = new SelectList(Puestos, "IdPuesto", "Nombre");
            try
            {
                if (ModelState.IsValid)
                {
                    GpsManager.Empleado.Element = Empleado;
                    var result = GpsManager.InformacionMedica.Add();
                    if (result == false)
                    {
                        ModelState.AddModelError("", GpsManager.GetLastMessage());
                        return View("Edit", new SplittelEmpleado { persona = GpsManager.Persona.Get(Empleado.IdPersona), empleado = Empleado });
                    }
                    return RedirectToAction(nameof(Edit), new { id = Empleado.IdPersona });
                }
                else
                {
                    return View("Edit", new SplittelEmpleado { persona = GpsManager.Persona.Get(Empleado.IdPersona), empleado = Empleado });
                }
            }
            catch (GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Edit", new SplittelEmpleado { persona = GpsManager.Persona.Get(Empleado.IdPersona), empleado = Empleado });
            }
        }

        public void SomeMethod()
        {
            Console.WriteLine("Begin SomeMethod");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Console.WriteLine("End SomeMethod");
        }
    }


    class Chuchin
    {
        public string Edad { get; set; }
    }
}