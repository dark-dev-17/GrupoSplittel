using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GestionPersonal.Models;
using GPSInformation;
using GPSInformation.Controllers;
using GPSInformation.Exceptions;
using GPSInformation.Models;
using GPSInformation.Reportes;
using GPSInformation.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using MySqlX.XDevAPI.Relational;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace GestionPersonal.Controllers
{
    public class ProduccionController : Controller
    {
        private ProduccionModCtrl ProduccionModCtrl;

        public ProduccionController(IConfiguration configuration)
        {
            ProduccionModCtrl = new ProduccionModCtrl(new DarkManager(configuration));
        }

        #region Producion startup v1
        [AccessMultipleView(IdAction = new int[] { 40 })]
        public ActionResult Index()
        {
            try
            {
                return View(ProduccionModCtrl.GetEmpleadosProd());
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return View(ex.Message);
            }
        }
        [AccessMultipleView(IdAction = new int[] { 40 })]
        public ActionResult AsignarTurno(int id)
        {
            try
            {
                ViewData["Turnos"] = new SelectList(ProduccionModCtrl.GetTurnos(), "IdTurnosProduccion", "Descripcion", 0);
                return PartialView(ProduccionModCtrl.GetTurnoEmpleado(id));
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return PartialView(ex.Message);
            }
        }
        [AccessMultipleView(IdAction = new int[] { 40 })]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AsignarTurno(TurnoEmpleadoForm turnoEmpleado)
        {
            try
            {

                ViewData["Turnos"] = new SelectList(ProduccionModCtrl.GetTurnos(), "IdTurnosProduccion", "Descripcion", turnoEmpleado.IdTurnosProduccion);
                if (!ModelState.IsValid)
                {
                    return PartialView(turnoEmpleado);
                }
                ProduccionModCtrl.CambioTurno(turnoEmpleado.IdPersona, turnoEmpleado.IdTurnosProduccion, turnoEmpleado.FechaInicio, turnoEmpleado.FechaFin);
                return PartialView(ProduccionModCtrl.GetTurnoEmpleado(turnoEmpleado.IdPersona));
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return PartialView(ex.Message);
            }
        }

        // GET: EvaluacionController
        [AccessMultipleView(IdAction = new int[] { 40 })]
        public ActionResult Prenomina()
        {
            try
            {

                return View(ProduccionModCtrl.GetPrenomina());
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return View(ex.Message);
            }
        }

        [AccessMultipleView(IdAction = new int[] { 40 })]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Prenomina(Prenomina_RepProd prenomina_Rep)
        {
            try
            {
                var empleadoEnsambles = ProduccionModCtrl.GetEmpleadosProd();
                ViewData["Empleados"] = empleadoEnsambles;
                ViewData["Dias"] = ProduccionModCtrl.GetPreniminaLists(prenomina_Rep, empleadoEnsambles);

                return View(prenomina_Rep);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return View(ex.Message);
            }
        }

        [AccessDataSession(IdAction = new int[] { 40 })]
        public ActionResult GetTunosHistorico(int id)
        {
            try
            {
                var turnos = ProduccionModCtrl.GetEmpleadoTurnos(id);
                return Ok(new { Turno_1 = turnos.Where(a => a.IdTurnosProduccion == 1), Turno_b = turnos.Where(a => a.IdTurnosProduccion == 2) });
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Horas trabajadas v2
        public ActionResult GetReportEnsamble()
        {
            try
            {
                var turnos = ProduccionModCtrl.GetReportEnsamble();
                return Ok(turnos);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public ActionResult ProduccionHoras()
        {
            try
            {
                var turnos = ProduccionModCtrl.GetReportEnsamble();
                return View(turnos);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Horas trabajadas v3
        [HttpPost]
        public ActionResult Asignargrupo(TurnoProdForm TurnoProdForm)
        {
            ViewData["Turnos"] = new SelectList(ProduccionModCtrl.GetTurnosSelect(), "IdCatalogoOpcionesValores", "Descripcion", TurnoProdForm.IdGrupo);
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(TurnoProdForm);
                }
                ProduccionModCtrl.Chengegrupo(TurnoProdForm.IdPersona, TurnoProdForm.IdGrupo, TurnoProdForm.FechaInicio, TurnoProdForm.FechaFin);
                ViewData["Success"] = "Cambio de grupo hecho exitosamente";
                return PartialView(TurnoProdForm);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView(TurnoProdForm);
            }
        }
        public ActionResult Asignargrupo(int id)
        {
            ViewData["Turnos"] = new SelectList(ProduccionModCtrl.GetTurnosSelect(), "IdCatalogoOpcionesValores", "Descripcion");
            return PartialView(new TurnoProdForm { IdPersona = id, FechaInicio = DateTime.Now, FechaFin = DateTime.Now });
        }
        public ActionResult EmpleadoGrupoDetais(int id)
        {
            try
            {
                var turnos = ProduccionModCtrl.GetGrupos(id);
                return Ok(turnos);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AccessView]
        public ActionResult VerEmpleadoProds()
        {
            try
            {
                var turnos = ProduccionModCtrl.EmpleadoProds(DateTime.Parse("2021-01-04 00:00:00"));
                return View(turnos);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AccessView]
        public ActionResult OpeTurno()
        {
            try
            {
                var turnos = ProduccionModCtrl.GetEmpleadogrupoProds();
                return View(turnos);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AccessView]
        public ActionResult DetailsWeek(int IdPersona)
        {
            try
            {
                var turnos = ProduccionModCtrl.ProcessEmpProd(new View_empleadoEnsamble(),DateTime.Parse("2021-01-04 00:00:00"), IdPersona);
                return PartialView(turnos);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
    }
}
