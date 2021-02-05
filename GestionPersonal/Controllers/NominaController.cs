using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using GestionPersonal.Models;
using GPSInformation;
using GPSInformation.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class NominaController : Controller
    {
        private NominaCtrl ExpedienteCtrl;

        public NominaController(IConfiguration configuration)
        {
            ExpedienteCtrl = new NominaCtrl(new DarkManager(configuration));
        }

        ~NominaController()
        {

        }
        [AccessMultipleView(IdAction = new int[] { 18 })]
        public ActionResult Index()
        {
            
            return View(ExpedienteCtrl.GetNOminas(HttpContext.Session.GetString("user_RFC")));
        }

        // GET: Nomina
        public ActionResult ProccessFiles()
        {
            try
            {
                ExpedienteCtrl.ProccessFiles();
                return Ok("Proceso completo");
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return BadRequest(ex.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
            
        }
        [HttpGet]
        public FileResult GetByRFC_Id(int id, string Type)
        {
            string RFC = HttpContext.Session.GetString("user_RFC");
            byte[] fileBytes = ExpedienteCtrl.GetCFDI(RFC, id, Type);
            string fileName = ExpedienteCtrl.GetNomina(RFC, id).NombreArchivo + "." + Type;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

    }
}