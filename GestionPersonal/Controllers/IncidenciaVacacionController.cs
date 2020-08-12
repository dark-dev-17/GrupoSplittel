using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionPersonal.Models;
using GPSInformation;
using GPSInformation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class IncidenciaVacacionController : Controller
    {
        private DarkManager darkManager;

        public IncidenciaVacacionController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.Persona);
            darkManager.LoadObject(GpsManagerObjects.IncidenciaVacacion);
            darkManager.LoadObject(GpsManagerObjects.IncidenciaVacacionProcess);
            darkManager.LoadObject(GpsManagerObjects.DiaFeriado);
        }

        ~IncidenciaVacacionController()
        {

        }
        // GET: IncidenciaVacacion
        public ActionResult Index()
        {
            var result = darkManager.IncidenciaVacacion.Get();
            return View(result);
        }

        // GET: IncidenciaVacacion/Details/5
        public ActionResult Details(int id)
        {
            //var result = darkManager.IncidenciaVacacion.Get(""+ id,nameof(darkManager.IncidenciaVacacion.Element.IdPersona));
            var result = darkManager.IncidenciaVacacion.Get(id);
            return View(result);
        }

        // GET: IncidenciaVacacion/Create
        public ActionResult Create(int id)
        {
            return View(new IncidenciaVacacion { IdPersona = id, Inicio= DateTime.Now, Fin = DateTime.Now });
        }

        // POST: IncidenciaVacacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IncidenciaVacacion IncidenciaVacacion)
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
                if (darkManager.IncidenciaVacacion.Add())
                {
                    AddSteps(IncidenciaVacacion);

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
        }

        // GET: IncidenciaVacacion/Edit/5
        public ActionResult Edit(int id)
        {
            var result = darkManager.IncidenciaVacacion.Get(id);
            return View(result);
        }

        // POST: IncidenciaVacacion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IncidenciaVacacion IncidenciaVacacion)
        {
            try
            {
                // TODO: Add insert logic here
                if (!ModelState.IsValid)
                {
                    return View(IncidenciaVacacion);
                }
                darkManager.IncidenciaVacacion.Element = IncidenciaVacacion;
                darkManager.IncidenciaVacacion.Element.CreadoPor = "E";
                darkManager.IncidenciaVacacion.Element.Estatus = 1;
                darkManager.IncidenciaVacacion.Element.NoDias = 2;
                darkManager.IncidenciaVacacion.Element.Tipo = "A";
                darkManager.IncidenciaVacacion.Element.NumAutorizaciones = 3;
                if (darkManager.IncidenciaVacacion.Update())
                {
                    return RedirectToAction(nameof(Index), "Incidencia", new { Id = IncidenciaVacacion.IdPersona });
                }
                else
                {
                    ModelState.AddModelError("", darkManager.GetLastMessage());
                    return View(IncidenciaVacacion);
                }

            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(IncidenciaVacacion);
            }
        }

        // GET: IncidenciaVacacion/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: IncidenciaVacacion/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: IncidenciaVacacion/Delete/5
        [HttpPost]
        public ActionResult Actividad(int id)
        {
            try
            {
                // TODO: Add delete logic here
                var result = darkManager.IncidenciaVacacionProcess.Get(""+id, nameof(darkManager.IncidenciaVacacionProcess.Element.IdIncidenciaVacacion));
                return PartialView(result);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return PartialView(ex.Message);
            }
        }

        private void AddSteps(IncidenciaVacacion IncidenciaVacacion)
        {
            var procesoStep = new IncidenciaVacacionProcess();
            procesoStep.IdIncidenciaVacacion = darkManager.IncidenciaVacacion.GetLastId();
            procesoStep.IdPersona = IncidenciaVacacion.IdPersona;
            procesoStep.Fecha = DateTime.Now;
            procesoStep.Titulo = "Incidencia creada por solicitante";
            procesoStep.Comentarios = "";
            procesoStep.Nivel = 1;
            procesoStep.Revisada = true;
            procesoStep.Autorizada = true;
            procesoStep.NombreEmpleado = HttpContext.Session.GetString("user_fullname");
            darkManager.IncidenciaVacacionProcess.Element = procesoStep;
            darkManager.IncidenciaVacacionProcess.Add();

            procesoStep.IdPersona = 0;
            procesoStep.Fecha = null;
            procesoStep.Titulo = "Aprobación por jefe inmediato";
            procesoStep.Comentarios = "";
            procesoStep.Nivel = 2;
            procesoStep.Revisada = false;
            procesoStep.Autorizada = false;
            procesoStep.NombreEmpleado = "";
            darkManager.IncidenciaVacacionProcess.Element = procesoStep;
            darkManager.IncidenciaVacacionProcess.Add();

            procesoStep.IdPersona = 0;
            procesoStep.Fecha = null;
            procesoStep.Titulo = "Aprobación por gestión de personal";
            procesoStep.Comentarios = "";
            procesoStep.Nivel = 3;
            procesoStep.Revisada = false;
            procesoStep.Autorizada = false;
            procesoStep.NombreEmpleado = "";
            darkManager.IncidenciaVacacionProcess.Element = procesoStep;
            darkManager.IncidenciaVacacionProcess.Add();

            procesoStep.IdPersona = 0;
            procesoStep.Fecha = null;
            procesoStep.Titulo = "Vacaciones concluidas/tomadas";
            procesoStep.Comentarios = "";
            procesoStep.Nivel = 4;
            procesoStep.Revisada = false;
            procesoStep.Autorizada = false;
            procesoStep.NombreEmpleado = "";
            darkManager.IncidenciaVacacionProcess.Element = procesoStep;
            darkManager.IncidenciaVacacionProcess.Add();
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
    }
}