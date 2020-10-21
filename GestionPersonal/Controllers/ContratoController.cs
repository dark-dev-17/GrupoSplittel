using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionPersonal.Models;
using GPSInformation;
using GPSInformation.Controllers;
using GPSInformation.Exceptions;
using GPSInformation.Models;
using GPSInformation.Reportes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Rotativa.AspNetCore;

namespace GestionPersonal.Controllers
{
    public class ContratoController : Controller
    {
        private EmpleadoCtrl EmpleadoCtrl;

        public ContratoController(IConfiguration configuration)
        {
            //int IdUserLog = (int)HttpContext.Session.GetInt32("user_id");
            EmpleadoCtrl = new EmpleadoCtrl(new DarkManager(configuration));
        }

        // GET: EvaluacionController
        [AccessMultipleView(IdAction = new int[] { 20 })]
        public ActionResult Index(int id)
        {
            try
            {
                ViewData["Idpersona"] = id;
                return View(EmpleadoCtrl.GetContratos(id));
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return View(ex.Message);
            }
           
        }


        [AccessMultipleView(IdAction = new int[] { 20 })]
        public ActionResult Create(int id)
        {
            try
            {
                return View(new EmpleadoContrato { 
                    Created = DateTime.Now,
                    Inicio = DateTime.Now,
                    Fin = DateTime.Now,
                    Tipo = 1,
                    IdPersona = id,
                });
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return View(ex.Message);
            }

        }

        // POST: EvaluacionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 20 })]
        public ActionResult Create(EmpleadoContrato EmpleadoContrato)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(EmpleadoContrato);
                }
                EmpleadoCtrl.Add(EmpleadoContrato);
                return RedirectToAction(nameof(Index), new { id = EmpleadoContrato.IdPersona});
            }
            catch (GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(EmpleadoContrato);
            }
        }

        [AccessMultipleView(IdAction = new int[] { 19, 20 })]
        public ActionResult Indeterminado(int id)
        {
            ContratoEmp contrato = new ContratoEmp();
            try
            {
                EmpleadoCtrl.GetDarkManager.LoadObject(GpsManagerObjects.Puesto);
                EmpleadoCtrl.GetDarkManager.LoadObject(GpsManagerObjects.Empleado);
                EmpleadoCtrl.GetDarkManager.LoadObject(GpsManagerObjects.Persona);
                EmpleadoCtrl.GetDarkManager.LoadObject(GpsManagerObjects.EmpleadoContrato);
                EmpleadoCtrl.GetDarkManager.LoadObject(GpsManagerObjects.InformacionCompania);
                contrato.EmpleadoContrato = EmpleadoCtrl.GetDarkManager.EmpleadoContrato.Get(id);
                contrato.Persona = EmpleadoCtrl.GetDarkManager.Persona.Get(contrato.EmpleadoContrato.IdPersona);
                contrato.Empleado = EmpleadoCtrl.GetDarkManager.Empleado.GetByColumn(contrato.EmpleadoContrato.IdPersona + "", "IdPersona");
                contrato.Puesto = EmpleadoCtrl.GetDarkManager.Puesto.GetByColumn(contrato.Empleado.IdPuesto + "", "IdPuesto");
                contrato.InformacionCompania = EmpleadoCtrl.GetDarkManager.InformacionCompania.GetByColumn("1", "Activa");
               
                var report = new ViewAsPdf(contrato)
                {
                    PageMargins = { Left = 20, Bottom = 20, Right = 20, Top = 20 },
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                    //PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    //CustomSwitches = "--disable-smart-shrinking --page-offset 0 --footer-center [page] --footer-font-size 12",
                    //FileName = String.Format("Contrato_{0}.pdf", contrato.Empleado.NumeroNomina),
                };

                return report;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                EmpleadoCtrl.GetDarkManager.CloseConnection();
            }
        }

        [AccessMultipleView(IdAction = new int[] { 19, 20 })]
        public IActionResult Determinado(int id)
        {
            ContratoEmp contrato = new ContratoEmp();
            try
            {
                EmpleadoCtrl.GetDarkManager.LoadObject(GpsManagerObjects.Puesto);
                EmpleadoCtrl.GetDarkManager.LoadObject(GpsManagerObjects.Empleado);
                EmpleadoCtrl.GetDarkManager.LoadObject(GpsManagerObjects.Persona);
                EmpleadoCtrl.GetDarkManager.LoadObject(GpsManagerObjects.EmpleadoContrato);
                EmpleadoCtrl.GetDarkManager.LoadObject(GpsManagerObjects.InformacionCompania);
                contrato.EmpleadoContrato = EmpleadoCtrl.GetDarkManager.EmpleadoContrato.Get(id);
                contrato.Persona = EmpleadoCtrl.GetDarkManager.Persona.Get(contrato.EmpleadoContrato.IdPersona);
                contrato.Empleado = EmpleadoCtrl.GetDarkManager.Empleado.GetByColumn(contrato.EmpleadoContrato.IdPersona + "", "IdPersona");
                contrato.Puesto = EmpleadoCtrl.GetDarkManager.Puesto.GetByColumn(contrato.Empleado.IdPuesto + "", "IdPuesto");
                contrato.InformacionCompania = EmpleadoCtrl.GetDarkManager.InformacionCompania.GetByColumn("1", "Activa");
                var report = new ViewAsPdf(contrato)
                {
                    PageMargins = { Left = 20, Bottom = 20, Right = 20, Top = 20 },
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                    //PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    //CustomSwitches = "--disable-smart-shrinking --page-offset 0 --footer-center [page] --footer-font-size 12",
                    //FileName = String.Format("Contrato_{0}.pdf", contrato.Empleado.NumeroNomina),
                };

                return report;
            }
            finally
            {
                EmpleadoCtrl.GetDarkManager.CloseConnection();
            }
        }
    }
}
