using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using GestionPersonal.Models;
using GestionPersonal.Service;
using GPSInformation;
using GPSInformation.Controllers;
using GPSInformation.Exceptions;
using GPSInformation.Models;
using GPSInformation.Reportes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class IncidenciaVacacionController : Controller
    {
        private DarkManager darkManager;
        private readonly IViewRenderService _viewRenderService;
        public IncidenciaVacacionController(IConfiguration configuration, IViewRenderService viewRenderService)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.Persona);
            darkManager.LoadObject(GpsManagerObjects.IncidenciaVacacion);
            darkManager.LoadObject(GpsManagerObjects.IncidenciaProcess);
            darkManager.LoadObject(GpsManagerObjects.DiaFeriado);
            darkManager.LoadObject(GpsManagerObjects.VacionesPeriodo);
            darkManager.LoadObject(GpsManagerObjects.VacacionesDiasRegla);
            darkManager.LoadObject(GpsManagerObjects.Empleado);
            darkManager.LoadObject(GpsManagerObjects.View_empleado);
            _viewRenderService = viewRenderService;
        }

        ~IncidenciaVacacionController()
        {

        }

        [AccessMultipleView(IdAction = new int[] { 30 })]
        public ActionResult DetailsEmail(int id)
        {
            try
            {
                IncidenciaVacaRe incidenciaVacaRe = new IncidenciaVacaRe();
                incidenciaVacaRe.IncidenciaVacacion = darkManager.IncidenciaVacacion.Get(id);
                incidenciaVacaRe.view_Empleado = darkManager.View_empleado.Get(incidenciaVacaRe.IncidenciaVacacion.IdPersona);
                incidenciaVacaRe.IncidenciaVacacion.Proceso = darkManager.IncidenciaProcess.Get("" + id, nameof(darkManager.IncidenciaProcess.Element.IdIncidenciaVacacion));
                return View(incidenciaVacaRe);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                return NotFound(ex.Message);
            }
            finally
            {
                darkManager.CloseConnection();
                darkManager = null;
            }

            
        }

        // GET: IncidenciaVacacion
        public ActionResult Index()
        {
            try
            {
                var result = darkManager.IncidenciaVacacion.Get();
                return View(result);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                return NotFound(ex.Message);
            }
            finally
            {
                darkManager.CloseConnection();
                darkManager = null;
            }
        }

        [AccessMultipleView(IdAction = new int[] { 30 })]
        // GET: IncidenciaVacacion/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                //var result = darkManager.IncidenciaVacacion.Get(""+ id,nameof(darkManager.IncidenciaVacacion.Element.IdPersona));

                var result = darkManager.IncidenciaVacacion.Get(id);

                ViewData["Actividades"] = darkManager.IncidenciaProcess.Get("" + id, nameof(darkManager.IncidenciaProcess.Element.IdIncidenciaVacacion));
                return View(result);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                return NotFound(ex.Message);
            }
            finally
            {
                darkManager.CloseConnection();
                darkManager = null;
            }
           
        }

        [AccessMultipleView(IdAction = new int[] { 30 })]
        // GET: IncidenciaVacacion/Create
        public ActionResult Create(int id)
        {
            return View(new IncidenciaVacacion { IdPersona = (int)HttpContext.Session.GetInt32("user_id"), Inicio= DateTime.Now, Fin = DateTime.Now });
        }

        // POST: IncidenciaVacacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 30 })]
        public async Task<ActionResult> Create(IncidenciaVacacion IncidenciaVacacion)
        {
            try
            {
                // TODO: Add insert logic here
                if (!ModelState.IsValid)
                {
                    return View(IncidenciaVacacion);
                }

                if(IncidenciaVacacion.Inicio >= IncidenciaVacacion.Fin)
                {
                    ModelState.AddModelError("Inicio", "La fecha de inicio es mayor o igual a la fecha de termino");
                    return View(IncidenciaVacacion);
                }

                var vacaciones = darkManager.IncidenciaVacacion.Get(IncidenciaVacacion.IdPersona + "", "IdPersona");

                if (vacaciones.Where(a=>  IncidenciaVacacion.Inicio >= a.Inicio  && IncidenciaVacacion.Inicio <= a.Fin).ToList().Count > 0)
                {
                    ModelState.AddModelError("Inicio", "La fecha de inicio coinside en otras solicitudes de vacaciones");
                    return View(IncidenciaVacacion);
                }
                if (vacaciones.Where(a => IncidenciaVacacion.Fin >= a.Inicio && IncidenciaVacacion.Fin <= a.Fin).ToList().Count > 0)
                {
                    ModelState.AddModelError("Fin", "La fecha de termino coinside en otras solicitudes de vacaciones");
                    return View(IncidenciaVacacion);
                }

                if (vacaciones.Where(a => a.Inicio >= IncidenciaVacacion.Inicio && a.Fin <= IncidenciaVacacion.Fin).ToList().Count > 0)
                {
                    ModelState.AddModelError("", "La fechas de tu solicitud abarcan solicitudes de vacaciones");
                    return View(IncidenciaVacacion);
                }

                darkManager.IncidenciaVacacion.Element = IncidenciaVacacion;
                darkManager.IncidenciaVacacion.Element.CreadoPor = "E";
                darkManager.IncidenciaVacacion.Element.Estatus = 1; // activas 2 canceladas 
                darkManager.IncidenciaVacacion.Element.NoDias = GetDays(IncidenciaVacacion.Inicio, IncidenciaVacacion.Fin);
                darkManager.IncidenciaVacacion.Element.Tipo = "A";
                darkManager.IncidenciaVacacion.Element.NumAutorizaciones = 4;
                darkManager.IncidenciaVacacion.Element.Creado = DateTime.Now;
                if (darkManager.IncidenciaVacacion.Add())
                {
                    int last = darkManager.IncidenciaVacacion.GetLastId(nameof(darkManager.IncidenciaVacacion.Element.IdPersona), IncidenciaVacacion.IdPersona + "");
                    AddSteps(darkManager.IncidenciaVacacion.Get(last));
                    await SendEmailAsync(1, last);
                    await SendEmailAsync(2, last);
                    return RedirectToAction(nameof(Index), "Incidencia", new { Id = IncidenciaVacacion.IdPersona });
                }
                else
                {
                    ModelState.AddModelError("", darkManager.GetLastMessage());
                    return View(IncidenciaVacacion);
                }

            }
            catch(GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(IncidenciaVacacion);
            }
            finally
            {
                darkManager.CloseConnection();
                darkManager = null;
            }
        }

        // GET: IncidenciaVacacion/Delete/5
        [AccessMultipleView(IdAction = new int[] { 30 })]
        public ActionResult Cancel(int id)
        {
            try
            {
                var result = darkManager.IncidenciaVacacion.Get(id);
                ViewData["Actividades"] = darkManager.IncidenciaProcess.Get("" + id, nameof(darkManager.IncidenciaProcess.Element.IdIncidenciaVacacion));
                return View(result);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                return NotFound(ex.Message);
            }
            finally
            {
                darkManager.CloseConnection();
                darkManager = null;
            }
            
        }

        // POST: IncidenciaVacacion/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 30 })]
        public ActionResult Cancel(int id, IFormCollection collection)
        {
            
            try
            {
                // TODO: Add delete logic here
                var result = darkManager.IncidenciaVacacion.Get(id);
                result.Estatus = 2; // cancel
                darkManager.IncidenciaVacacion.Element = result;

                if (darkManager.IncidenciaVacacion.Update())
                {
                    return RedirectToAction(nameof(Index), "Incidencia", new { Id = result.IdPersona });
                }
                else
                {
                    return NotFound(darkManager.GetLastMessage());
                }
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return NotFound(ex.Message);
            }
            finally
            {
                darkManager.CloseConnection();
                darkManager = null;
            }
        }

        [AccessMultipleView(IdAction = new int[] { 32, 36 })]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Aprobar(int id, int Mode)
        {
            darkManager.StartTransaction();
            try
            {
                // TODO: Add delete logic here
                var result = darkManager.IncidenciaProcess.Get("" + id, nameof(darkManager.IncidenciaProcess.Element.IdIncidenciaVacacion));
                if (Mode == 1)
                {
                    var Persona = darkManager.Persona.Get((int)HttpContext.Session.GetInt32("user_id"));
                    var nivel = result.Find(a => a.Nivel == 2);
                    nivel.Autorizada = true;
                    nivel.Fecha = DateTime.Now;
                    nivel.IdPersona = (int)HttpContext.Session.GetInt32("user_id");
                    nivel.NombreEmpleado = Persona.NombreCompelto;
                    nivel.Revisada = true;
                    nivel.IdIncidenciaVacacion = id;

                    darkManager.IncidenciaProcess.Element = nivel;

                    if (darkManager.IncidenciaProcess.Update())
                    {
                        darkManager.Commit();
                        return RedirectToAction("AprobarJefe", "Incidencia", new { tab = "Vacaciones" });
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
                    nivel.IdIncidenciaVacacion = id;
                    darkManager.IncidenciaProcess.Element = nivel;

                    if (darkManager.IncidenciaProcess.Update())
                    {
                        darkManager.Commit();
                        return RedirectToAction("AprobarGPS", "Incidencia", new { tab = "Vacaciones" });
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
            finally
            {
                darkManager.CloseConnection();
                darkManager = null;
            }
        }

        [AccessMultipleView(IdAction = new int[] { 32, 36 })]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rechazar(int id, int Mode, string Comentario)
        {
            darkManager.StartTransaction();
            try
            {
                // TODO: Add delete logic here
                var result = darkManager.IncidenciaProcess.Get("" + id, nameof(darkManager.IncidenciaProcess.Element.IdIncidenciaVacacion));
                if (Mode == 1)
                {
                    var Persona = darkManager.Persona.Get((int)HttpContext.Session.GetInt32("user_id"));
                    var nivel = result.Find(a => a.Nivel == 2);
                    nivel.Autorizada = false;
                    nivel.Fecha = DateTime.Now;
                    nivel.IdPersona = (int)HttpContext.Session.GetInt32("user_id");
                    nivel.NombreEmpleado = Persona.NombreCompelto;
                    nivel.Revisada = true;
                    nivel.IdIncidenciaVacacion = id;
                    nivel.Comentarios = Comentario;
                    darkManager.IncidenciaProcess.Element = nivel;

                    if (darkManager.IncidenciaProcess.Update())
                    {
                        darkManager.Commit();
                        return RedirectToAction("AprobarJefe", "Incidencia", new { tab = "Vacaciones" });
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
                    nivel.Comentarios = Comentario;
                    nivel.IdIncidenciaVacacion = id;
                    darkManager.IncidenciaProcess.Element = nivel;

                    if (darkManager.IncidenciaProcess.Update())
                    {
                        darkManager.Commit();
                        return RedirectToAction("AprobarGPS", "Incidencia", new { tab = "Vacaciones" });
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
                return NotFound(ex.Message);
            }
            finally
            {
                darkManager.CloseConnection();
                darkManager = null;
            }
        }

        [AccessMultipleView(IdAction = new int[] { 32, 36 })]
        public ActionResult Aprobar(int id, string Mode)
        {
            var result = darkManager.IncidenciaVacacion.Get(id);

            ViewData["Actividades"] = darkManager.IncidenciaProcess.Get("" + id, nameof(darkManager.IncidenciaProcess.Element.IdIncidenciaVacacion));
            ViewData["ModeAprobar"] = Mode;
            return View(result);
        }

        [AccessMultipleView(IdAction = new int[] { 32, 36 })]
        public ActionResult Rechazar(int id, string Mode)
        {
            try
            {
                var result = darkManager.IncidenciaVacacion.Get(id);

                ViewData["Actividades"] = darkManager.IncidenciaProcess.Get("" + id, nameof(darkManager.IncidenciaProcess.Element.IdIncidenciaVacacion));
                ViewData["ModeAprobar"] = Mode;
                return View(result);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return NotFound(ex.Message);
            }
            finally
            {
                darkManager.CloseConnection();
                darkManager = null;
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
                var result = darkManager.IncidenciaProcess.Get(""+id, nameof(darkManager.IncidenciaProcess.Element.IdIncidenciaVacacion));
                ViewData["Incidencia"] = darkManager.IncidenciaVacacion.Get(id);
                return PartialView(result);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return NotFound(ex.Message);
            }
            finally
            {
                darkManager.CloseConnection();
                darkManager = null;
            }
        }

        private void AddSteps(IncidenciaVacacion IncidenciaVacacion)
        {
            var procesoStep = new IncidenciaProcess();
            procesoStep.IdIncidenciaVacacion = IncidenciaVacacion.IdIncidenciaVacacion;
            procesoStep.IdPersona = IncidenciaVacacion.IdPersona;
            procesoStep.Fecha = DateTime.Now;
            procesoStep.Titulo = "Incidencia creada por solicitante";
            procesoStep.Comentarios = "";
            procesoStep.Nivel = 1;
            procesoStep.Revisada = true;
            procesoStep.Autorizada = true;
            procesoStep.NombreEmpleado = HttpContext.Session.GetString("user_fullname");
            darkManager.IncidenciaProcess.Element = procesoStep;
            darkManager.IncidenciaProcess.Add();

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

            procesoStep.IdPersona = 0;
            procesoStep.Fecha = null;
            procesoStep.Titulo = "Vacaciones concluidas/tomadas";
            procesoStep.Comentarios = "";
            procesoStep.Nivel = 4;
            procesoStep.Revisada = false;
            procesoStep.Autorizada = false;
            procesoStep.NombreEmpleado = "";
            darkManager.IncidenciaProcess.Element = procesoStep;
            darkManager.IncidenciaProcess.Add();
        }

        private int GetDays(DateTime desde, DateTime hasta)
        {
            DateTime inicio = desde;
            int dias = 0;
            while (inicio <= hasta)
            {
                if (inicio.DayOfWeek != DayOfWeek.Saturday && inicio.DayOfWeek != DayOfWeek.Sunday)
                {
                    var result = darkManager.DiaFeriado.GetByColumn("", nameof(darkManager.DiaFeriado.Element.Fecha));
                    if (result == null)
                    {
                        dias++;
                    }
                }
                inicio = inicio.AddDays(1);
            }
            return dias;
        }

        [HttpGet]
        [AccessMultipleView(IdAction = new int[] { 30, 32, 36 })]
        public ActionResult GenerarPeridos()
        {
            VacacionesCtrl vacacionesCtrl = new VacacionesCtrl((int)HttpContext.Session.GetInt32("user_id"),darkManager);
            try
            {
                vacacionesCtrl.ProcPeridosVac((int)HttpContext.Session.GetInt32("user_id"));
                return Ok("asasdasdasdasda");
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                vacacionesCtrl.Terminar();
                vacacionesCtrl = null;
            }
            
        }
        
        [HttpGet]
        [AccessMultipleView(IdAction = new int[] { 30, 32, 36 })]
        public ActionResult GenerarPeridosAll()
        {
            VacacionesCtrl vacacionesCtrl = new VacacionesCtrl((int)HttpContext.Session.GetInt32("user_id"), darkManager);
            try
            {
                
                vacacionesCtrl.ProcPeridosVacAll();
                return Ok("Periodos completos");
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                vacacionesCtrl.Terminar();
                vacacionesCtrl = null;
            }
            
        }
        private async Task SendEmailAsync(int mode, int id)
        {
            try
            {
                IncidenciaVacaRe incidenciaVacaRe = new IncidenciaVacaRe();
                incidenciaVacaRe.IncidenciaVacacion = darkManager.IncidenciaVacacion.Get(id);
                incidenciaVacaRe.view_Empleado = darkManager.View_empleado.Get(incidenciaVacaRe.IncidenciaVacacion.IdPersona);

                if (mode == 1)
                {
                    darkManager.LoadObject(GpsManagerObjects.OrganigramaVersion);
                    darkManager.LoadObject(GpsManagerObjects.OrganigramaStructura);
                    //nivel 1
                    var resultOrgActive = darkManager.OrganigramaVersion.GetByColumn("" + 2, "Autirizada");
                    if (resultOrgActive != null)
                    {
                        var ResultStructura = darkManager.OrganigramaStructura.Get(
                            "" + resultOrgActive.IdOrganigramaVersion,
                            "IdOrganigramaVersion").Find(a => a.IdPuesto == incidenciaVacaRe.view_Empleado.IdPuesto);

                        if (ResultStructura != null)
                        {
                            //extrar jefe del empleado
                            darkManager.View_empleado.Get("" + ResultStructura.IdPuestoParent, "IdPuesto").ForEach(a => {
                                darkManager.EmailServ_.AddListTO(a.Correo);
                            });
                        }
                        var result = await _viewRenderService.RenderToStringAsync("IncidenciaVacacion/DetailsEmail", incidenciaVacaRe);
                        darkManager.EmailServ_.Send(result, "Nuevas vacaciones - Aprobación N1");
                        darkManager.RestartEmail();
                    }
                }
                else
                { 
                    //nivel 2
                    darkManager.LoadObject(GpsManagerObjects.AccesosSistema);
                    darkManager.LoadObject(GpsManagerObjects.Usuario);
                    darkManager.AccesosSistema.Get("36", "IdSubModulo").Where(a => a.TieneAcceso).ToList().ForEach(a => {
                        darkManager.View_empleado.Get("" + darkManager.Usuario.Get(a.IdUsuario).IdPersona, "IdPersona").ForEach(b => {
                            darkManager.EmailServ_.AddListTO(b.Correo);
                        });
                    });
                    incidenciaVacaRe.ModeAmin = true;
                    var result = await _viewRenderService.RenderToStringAsync("IncidenciaVacacion/DetailsEmail", incidenciaVacaRe);
                    darkManager.EmailServ_.Send(result, "Nuevas vacaciones - Aprobación N2");
                    darkManager.RestartEmail();
                }
            }
            catch (SmtpException ex)
            {
                //throw;
            }
            catch (Exception ex)
            {
                //throw;
            }

        }
    }
}