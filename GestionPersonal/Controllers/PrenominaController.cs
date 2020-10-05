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

namespace GestionPersonal.Controllers
{
    public class PrenominaController : Controller
    {
        private PrenominaCtrl PrenominaCtrl;

        public PrenominaController(IConfiguration configuration)
        {
            PrenominaCtrl = new PrenominaCtrl(new DarkManager(configuration));
        }

        // GET: EvaluacionController
        [AccessMultipleView(IdAction = new int[] { 37 })]
        public ActionResult Index()
        {
            try
            {
                return View(PrenominaCtrl.GetExpediente());
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return View(ex.Message);
            }
           
        }

        [AccessMultipleView(IdAction = new int[] { 37 })]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Prenomina_Rep prenomina_Rep)
        {
            try
            {
                ViewData["Empleados"] = PrenominaCtrl.GetExpediente(prenomina_Rep);
                return View(prenomina_Rep);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return View(ex.Message);
            }

        }
    }
}
