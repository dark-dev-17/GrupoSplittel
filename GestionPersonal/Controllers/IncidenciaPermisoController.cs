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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using static Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary;

namespace GestionIncidenciaPermisol.Controllers
{
    public class IncidenciaPermisoController : Controller
    {
        private DarkManager darkManager;
        private SelectList TiposPermisos;
        private SelectList PagoPermisoPersonal;

        public IncidenciaPermisoController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.IncidenciaPermiso);
            darkManager.LoadObject(GpsManagerObjects.IncidenciaProcess);
            darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);
            darkManager.LoadObject(GpsManagerObjects.Persona);
            darkManager.LoadObject(GpsManagerObjects.Puesto);
            darkManager.LoadObject(GpsManagerObjects.Empleado);
        }

        ~IncidenciaPermisoController()
        {

        }

        [AccessMultipleView(IdAction = new int[] { 30 })]
        public ActionResult Create(int id)
        {
            TiposPermisos = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 1009, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            PagoPermisoPersonal = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 1010, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["TiposPermisos"] = TiposPermisos;
            ViewData["PagoPermisoPersonal"] = PagoPermisoPersonal;
            return View(new IncidenciaPermiso() { IdPersona = id, Fecha = DateTime.Now });
        }

        [AccessMultipleView(IdAction = new int[] { 30,32,36 })]
        public ActionResult Details(int id)
        {
            TiposPermisos = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 1009, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            PagoPermisoPersonal = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 1010, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["TiposPermisos"] = TiposPermisos;
            ViewData["PagoPermisoPersonal"] = PagoPermisoPersonal;
            ViewData["Procesos"] = darkManager.IncidenciaProcess.Get("" + id, nameof(darkManager.IncidenciaProcess.Element.IdIncidenciaPermiso)); ;

            var response = darkManager.IncidenciaPermiso.Get(id);
            return View(response);
        }

        // POST: IncidenciaPermiso/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 30 })]
        public ActionResult Create(IncidenciaPermiso IncidenciaPermiso)
        {
            TiposPermisos = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 1009, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            PagoPermisoPersonal = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 1010, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["TiposPermisos"] = TiposPermisos;
            ViewData["PagoPermisoPersonal"] = PagoPermisoPersonal;
            darkManager.StartTransaction();
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(IncidenciaPermiso);
                }

                darkManager.IncidenciaPermiso.Element = IncidenciaPermiso;
                darkManager.IncidenciaPermiso.Element.CreadoPor = "Empleado";
                darkManager.IncidenciaPermiso.Element.Estatus = 1;
                darkManager.IncidenciaPermiso.Element.Creado = DateTime.Now;
                if(darkManager.IncidenciaPermiso.Element.IdAsunto == 36)
                {
                    darkManager.IncidenciaPermiso.Element.IdPagoPermiso = 0;
                }

                bool result = darkManager.IncidenciaPermiso.Add();
                if (result)
                {

                    AddSteps(darkManager.IncidenciaPermiso.Get(darkManager.IncidenciaPermiso.GetLastId(nameof(darkManager.IncidenciaPermiso.Element.IdPersona), IncidenciaPermiso.IdPersona + "")));
                    darkManager.Commit();
                    return RedirectToAction("Index", "Incidencia", new { id = IncidenciaPermiso.IdPersona });
                }
                else
                {
                    return View(IncidenciaPermiso);
                }
                
            }
            catch(GPSInformation.Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                ModelState.AddModelError("", ex.Message);
                return View(IncidenciaPermiso);
            }
        }
        
        [AccessMultipleView(IdAction = new int[] { 30 })]
        public ActionResult Cancel(int id)
        {
            TiposPermisos = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 1009, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            PagoPermisoPersonal = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 1010, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["TiposPermisos"] = TiposPermisos;
            ViewData["PagoPermisoPersonal"] = PagoPermisoPersonal;

            var response = darkManager.IncidenciaPermiso.Get(id);
            return View(response);
        }
        
        [AccessMultipleView(IdAction = new int[] { 32,36 })]
        public ActionResult Aprobar(int id, string Mode)
        {
            TiposPermisos = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 1009, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            PagoPermisoPersonal = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 1010, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["TiposPermisos"] = TiposPermisos;
            ViewData["PagoPermisoPersonal"] = PagoPermisoPersonal;

            var response = darkManager.IncidenciaPermiso.Get(id);
            ViewData["ModeAprobar"] = Mode;
            return View(response);
        }
        
        [AccessMultipleView(IdAction = new int[] { 32, 36 })]
        public ActionResult Rechazar(int id, string Mode)
        {
            TiposPermisos = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 1009, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            PagoPermisoPersonal = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 1010, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["TiposPermisos"] = TiposPermisos;
            ViewData["PagoPermisoPersonal"] = PagoPermisoPersonal;

            var response = darkManager.IncidenciaPermiso.Get(id);
            ViewData["ModeAprobar"] = Mode;
            return View(response);
        }
        
        // POST: IncidenciaVacacion/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 30 })]
        public ActionResult Cancel(int id, IFormCollection collection)
        {
            darkManager.StartTransaction();
            try
            {
                // TODO: Add delete logic here
                var result = darkManager.IncidenciaPermiso.Get(id);
                result.Estatus = 2; // cancel
                darkManager.IncidenciaPermiso.Element = result;

                if (darkManager.IncidenciaPermiso.Update())
                {
                    darkManager.Commit();
                    return RedirectToAction("Index", "Incidencia", new { Id = result.IdPersona });
                }
                else
                {
                    darkManager.RolBack();
                    return NotFound(darkManager.GetLastMessage());
                }
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                return NotFound(ex.Message);
            }
        }
        
        // POST: IncidenciaVacacion/Delete/5
        [HttpPost]
        [AccessMultipleView(IdAction = new int[] { 30, 32, 36 })]
        public ActionResult Actividad(int id)
        {
            try
            {
                // TODO: Add delete logic here
                var result = darkManager.IncidenciaProcess.Get("" + id, nameof(darkManager.IncidenciaProcess.Element.IdIncidenciaPermiso));
                var response = darkManager.IncidenciaPermiso.Get(id);
                ViewData["Incidencia"] = response;
                return PartialView(result);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return PartialView(ex.Message);
            }
        }
        
        [AccessMultipleView(IdAction = new int[] { 32, 36 })]
        [HttpGet]
        public ActionResult AprobarInc(int id, int Mode)
        {
            darkManager.StartTransaction();
            try
            {
                // TODO: Add delete logic here
                var result = darkManager.IncidenciaProcess.Get("" + id, nameof(darkManager.IncidenciaProcess.Element.IdIncidenciaPermiso));
                if (Mode == 1)
                {
                    var Persona = darkManager.Persona.Get((int)HttpContext.Session.GetInt32("user_id"));
                    var nivel = result.Find(a => a.Nivel == 2);
                    nivel.Autorizada = true;
                    nivel.Fecha = DateTime.Now;
                    nivel.IdPersona = (int)HttpContext.Session.GetInt32("user_id");
                    nivel.NombreEmpleado = Persona.NombreCompelto;
                    nivel.Revisada = true;
                    nivel.IdIncidenciaPermiso = id;

                    darkManager.IncidenciaProcess.Element = nivel;

                    if (darkManager.IncidenciaProcess.Update())
                    {
                        darkManager.Commit();
                        return RedirectToAction("AprobarJefe","Incidencia", new { tab = "Permisos" });
                    }
                    else
                    {
                        darkManager.RolBack();
                        ModelState.AddModelError("", "Error al aprobar");
                        return RedirectToAction("Aprobar", new { id = id, Mode = Mode });
                    }
                }
                else if (Mode == 2)
                {
                    var Persona = darkManager.Persona.Get((int)HttpContext.Session.GetInt32("user_id"));
                    var nivel = result.Find(a => a.Nivel == 3);
                    nivel.Autorizada = true;
                    nivel.Fecha = DateTime.Now;
                    nivel.IdPersona = (int)HttpContext.Session.GetInt32("user_id");
                    nivel.NombreEmpleado = Persona.NombreCompelto;
                    nivel.Revisada = true;
                    nivel.IdIncidenciaPermiso = id;
                    darkManager.IncidenciaProcess.Element = nivel;

                    if (darkManager.IncidenciaProcess.Update())
                    {
                        darkManager.Commit();
                        return RedirectToAction("AprobarGPS", "Incidencia", new { tab = "Permisos" });
                    }
                    else
                    {
                        darkManager.RolBack();
                        ModelState.AddModelError("", "Error al aprobar");
                        return RedirectToAction("Aprobar", new { id = id, Mode = Mode });
                    }
                }
                else
                {
                    throw new GpExceptions("El parametro mode no es valido");
                }
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                return PartialView(ex.Message);
            }
        }
        
        [AccessMultipleView(IdAction = new int[] { 32, 36 })]
        [HttpGet]
        public ActionResult RechazarInc(int id, int Mode)
        {
            darkManager.StartTransaction();
            try
            {
                // TODO: Add delete logic here
                var result = darkManager.IncidenciaProcess.Get("" + id, nameof(darkManager.IncidenciaProcess.Element.IdIncidenciaPermiso));
                if (Mode == 1)
                {
                    var Persona = darkManager.Persona.Get((int)HttpContext.Session.GetInt32("user_id"));
                    var nivel = result.Find(a => a.Nivel == 2);
                    nivel.Autorizada = false;
                    nivel.Fecha = DateTime.Now;
                    nivel.IdPersona = (int)HttpContext.Session.GetInt32("user_id");
                    nivel.NombreEmpleado = Persona.NombreCompelto;
                    nivel.Revisada = true;
                    nivel.IdIncidenciaPermiso = id;
                    darkManager.IncidenciaProcess.Element = nivel;

                    if (darkManager.IncidenciaProcess.Update())
                    {
                        darkManager.Commit();
                        return RedirectToAction("AprobarJefe", "Incidencia", new { tab = "Permisos" });
                    }
                    else
                    {
                        darkManager.RolBack();
                        ModelState.AddModelError("", "Error al Rechazar");
                        return RedirectToAction("Rechazar", new { id = id, Mode = Mode });
                    }
                }
                else if (Mode == 2)
                {
                    var Persona = darkManager.Persona.Get((int)HttpContext.Session.GetInt32("user_id"));
                    var nivel = result.Find(a => a.Nivel == 3);
                    nivel.Autorizada = false;
                    nivel.Fecha = DateTime.Now;
                    nivel.IdPersona = (int)HttpContext.Session.GetInt32("user_id");
                    nivel.NombreEmpleado = Persona.NombreCompelto;
                    nivel.Revisada = true;
                    nivel.IdIncidenciaPermiso = id;
                    darkManager.IncidenciaProcess.Element = nivel;

                    if (darkManager.IncidenciaProcess.Update())
                    {
                        darkManager.Commit();
                        return RedirectToAction("AprobarGPS", "Incidencia", new { tab = "Permisos" });
                    }
                    else
                    {
                        darkManager.RolBack();
                        ModelState.AddModelError("", "Error al Rechazar");
                        return RedirectToAction("Aprobar", new { id = id, Mode = Mode });
                    }
                }
                else
                {
                    throw new GpExceptions("El parametro mode no es valido");
                }
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                return PartialView(ex.Message);
            }
        }

        private void AddSteps(IncidenciaPermiso IncidenciaPermiso)
        {
            try
            {
                var procesoStep = new IncidenciaProcess();
                procesoStep.IdIncidenciaPermiso = IncidenciaPermiso.IdIncidenciaPermiso;
                procesoStep.IdIncidenciaVacacion = 0;
                procesoStep.IdPersona = IncidenciaPermiso.IdPersona;
                procesoStep.Fecha = DateTime.Now;
                procesoStep.Titulo = "Incidencia creada por solicitante";
                procesoStep.Comentarios = "";
                procesoStep.Nivel = 1;
                procesoStep.Revisada = true;
                procesoStep.Autorizada = true;
                procesoStep.NombreEmpleado = HttpContext.Session.GetString("user_fullname");
                darkManager.IncidenciaProcess.Element = procesoStep;
                darkManager.IncidenciaProcess.Add();

                procesoStep.IdIncidenciaVacacion = 0;
                procesoStep.IdPersona = 0;
                procesoStep.Fecha = null;
                procesoStep.Titulo = "Aprobación por jefe inmediato";
                procesoStep.Comentarios = "";
                procesoStep.Nivel = 2;
                procesoStep.Revisada = false;
                procesoStep.Autorizada = false;
                procesoStep.NombreEmpleado = "";
                darkManager.IncidenciaProcess.Element = procesoStep;
                darkManager.IncidenciaProcess.Add();

                procesoStep.IdIncidenciaVacacion = 0;
                procesoStep.IdPersona = 0;
                procesoStep.Fecha = null;
                procesoStep.Titulo = "Aprobación por gestión de personal";
                procesoStep.Comentarios = "";
                procesoStep.Nivel = 3;
                procesoStep.Revisada = false;
                procesoStep.Autorizada = false;
                procesoStep.NombreEmpleado = "";
                darkManager.IncidenciaProcess.Element = procesoStep;
                darkManager.IncidenciaProcess.Add();

                procesoStep.IdIncidenciaVacacion = 0;
                procesoStep.IdPersona = 0;
                procesoStep.Fecha = null;
                procesoStep.Titulo = "permiso concluido/tomado";
                procesoStep.Comentarios = "";
                procesoStep.Nivel = 4;
                procesoStep.Revisada = false;
                procesoStep.Autorizada = false;
                procesoStep.NombreEmpleado = "";
                darkManager.IncidenciaProcess.Element = procesoStep;
                darkManager.IncidenciaProcess.Add();
            }
            catch (Exception ex)
            {

                throw new GPSInformation.Exceptions.GpExceptions(ex.Message);
            }
            
            
        }
    }
}