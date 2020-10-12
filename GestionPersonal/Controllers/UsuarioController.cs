using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPSInformation.Models;
using GPSInformation.DBManagers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GPSInformation;
using GestionPersonal.Models;
using Microsoft.AspNetCore.Http;
using GPSInformation.Controllers;

namespace GestionPersonal.Controllers
{
    public class UsuarioController : Controller
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
        private SelectList Personas;

        private UsuarioCtrl usuarioCtrl;


        public UsuarioController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            //darkManager.OpenConnection();
            //darkManager.LoadObject(GpsManagerObjects.Usuario);
            //darkManager.LoadObject(GpsManagerObjects.Persona);
            //darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);
            //darkManager.LoadObject(GpsManagerObjects.InformacionMedica);
            //darkManager.LoadObject(GpsManagerObjects.Puesto);
            //darkManager.LoadObject(GpsManagerObjects.Sociedad);
            //darkManager.LoadObject(GpsManagerObjects.Departamento);
            //darkManager.LoadObject(GpsManagerObjects.Empleado);
            //darkManager.LoadObject(GpsManagerObjects.PersonaContacto);
            //darkManager.LoadObject(GpsManagerObjects.View_empleado);
            //darkManager.LoadObject(GpsManagerObjects.AccesosSistema);
            usuarioCtrl = new UsuarioCtrl(darkManager);
        }

        // GET: Direccion
        [AccessMultipleView(IdAction = new int[] { 20 })]
        public IActionResult Index()
        {
            var List = darkManager.Usuario.Get();
            darkManager.CloseConnection();
            return View(List);
        }


        #region Usuario
        [AccessMultipleView(IdAction = new int[] { 20 })]
        public IActionResult Create(int id)
        {
            var usuario = darkManager.Usuario.GetByColumn(""+  id, "IdPersona");
            if(usuario is null)
            {
                var empleado = darkManager.View_empleado.GetByColumn("" + id, "IdPersona"); ;
                Usuario ususario = new Usuario { 
                    IdPersona = empleado.IdPersona,
                    Activo = true,
                    UserName = empleado.NumeroNomina
                };
                return View(ususario);
            }
            else
            {
                return RedirectToAction("Edit", new { id = id });
            }
        }

        [AccessMultipleView(IdAction = new int[] { 20 })]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Usuario Usuario)
        {

            Personas = GetDictionary(1018, 0);
            ViewData["Roles"] = Personas;

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(Usuario);
                }
                if (Usuario.IdRol == 0)
                {
                    ModelState.AddModelError("IdRol", "Por favor selecciona un rol de acceso");
                    return View(Usuario);
                }
                usuarioCtrl.AddUsuario(Usuario);
                return RedirectToAction(nameof(Index),"Empleado");
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(Usuario);
            }
        }

        [AccessMultipleView(IdAction = new int[] { 20 })]
        public IActionResult Edit(int id)
        {

            var usuario = darkManager.Usuario.GetByColumn("" + id, "IdPersona");
            
            if (usuario is null)
            {
                return NotFound();
            }
            Personas = GetDictionary(1018, usuario.IdRol);
            ViewData["Roles"] = Personas;
            return View(usuario);
        }

        [AccessMultipleView(IdAction = new int[] { 20 })]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Usuario Usuario)
        {
            Personas = GetDictionary(1018, 0);
            ViewData["Roles"] = Personas;

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(Usuario);
                }
                if (Usuario.IdRol == 0)
                {
                    ModelState.AddModelError("IdRol", "Por favor selecciona un rol de acceso");
                    return View(Usuario);
                }
                usuarioCtrl.AddUsuario(Usuario);
                return RedirectToAction(nameof(Index), "Empleado");
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(Usuario);
            }
        }

        // GET: Empleado/Edit
        [AccessView]
        public ActionResult Perfil()
        {
            GetSelects();
            var result = darkManager.Persona.Get((int)HttpContext.Session.GetInt32("user_id"));
            if (result == null)
                return NotFound();
            ViewData["Generos"] = GetDictionary(2,result.IdGenero);
            ViewData["EstadosCiviles"] = GetDictionary(3, result.IdEstadoCivil); ;
            

            var InforMedica = darkManager.InformacionMedica.GetByColumn("" + result.IdPersona, "IdPersona");
            ViewData["InfoMedica"] = InforMedica;
            if(InforMedica == null)
            {
                ViewData["Alergias"] = GetDictionary(5, 0);
                ViewData["TiposSangre"] = GetDictionary(4, 0); ;
            }
            else
            {
                ViewData["Alergias"] = GetDictionary(5, InforMedica.Alergias);
                ViewData["TiposSangre"] = GetDictionary(4, InforMedica.TipoSangre); ;
            }
            

            var InforEmpleado = darkManager.Empleado.GetByColumn("" + result.IdPersona, "IdPersona");
            ViewData["InforEmpleado"] = InforEmpleado;


            if(InforEmpleado == null)
            {
                ViewData["TipoNomina"] = GetDictionary(6, 0); ;
                ViewData["EstatusEmpleado"] = GetDictionary(7, 0); ;
                ViewData["Departamentos"] = new SelectList(darkManager.Departamento.Get().OrderBy(a => a.Nombre).ToList(), "IdDepartamento", "Nombre", 0);
                ViewData["Puestos"] = new SelectList(darkManager.Puesto.Get().OrderBy(a => a.Nombre).ToList(), "IdPuesto", "Nombre", 0);
                ViewData["Sociedades"] = new SelectList(darkManager.Sociedad.Get().OrderBy(a => a.Descripcion).ToList(), "IdSociedad", "Descripcion", 0);
            }
            else
            {
                ViewData["TipoNomina"] = GetDictionary(6, InforEmpleado.TipoNomina); ;
                ViewData["EstatusEmpleado"] = GetDictionary(7, InforEmpleado.IdEstatus); ;
                ViewData["Departamentos"] = new SelectList(darkManager.Departamento.Get().OrderBy(a => a.Nombre).ToList(), "IdDepartamento", "Nombre", InforEmpleado.IdDepartamento);
                ViewData["Puestos"] = new SelectList(darkManager.Puesto.Get().OrderBy(a => a.Nombre).ToList(), "IdPuesto", "Nombre", InforEmpleado.IdPuesto);
                ViewData["Sociedades"] = new SelectList(darkManager.Sociedad.Get().OrderBy(a => a.Descripcion).ToList(), "IdSociedad", "Descripcion", InforEmpleado.IdSociedad);
            }
            


            var PersonaContacto = darkManager.PersonaContacto.Get("" + result.IdPersona, "IdPersona");
            ViewData["PersonaContacto"] = PersonaContacto;
            ViewData["Parentezcos"] = GetDictionary(9, 0);

            return View(result);
        }

        private void GetSelects()
        {
            //    Generos = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 2, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            //    EstadosCiviles = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 3, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            //    Alergias = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 5, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            //    TiposSangre = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 4, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            //    TipoNomina = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 6, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            //    EstatusEmpleado = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 7, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            //Departamentos = new SelectList(darkManager.Departamento.Get().OrderBy(a => a.Nombre).ToList(), "IdDepartamento", "Nombre");
            //Puestos = new SelectList(darkManager.Puesto.Get().OrderBy(a => a.Nombre).ToList(), "IdPuesto", "Nombre");
            //Sociedades = new SelectList(darkManager.Sociedad.Get().OrderBy(a => a.Descripcion).ToList(), "IdSociedad", "Descripcion");
            //Parentezcos = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 9, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
        }

        private SelectList GetDictionary(int id, int IdSelected)
        {
            return new SelectList(darkManager.CatalogoOpcionesValores.Get("" + id, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion", IdSelected);
        }


        #endregion
    }
}
