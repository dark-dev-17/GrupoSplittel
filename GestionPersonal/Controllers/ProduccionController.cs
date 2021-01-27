using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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
        private ProduccionModV2Ctrl ProduccionModV2Ctrl;

        public ProduccionController(IConfiguration configuration)
        {
            ProduccionModV2Ctrl = new ProduccionModV2Ctrl(new DarkManager(configuration));
        }
        [AccessView]
        public ActionResult DetailsWeek(int IdPersona, int NoSemana, int Year)
        {
            try
            {
                var turnos = ProduccionModV2Ctrl.Getreporte(new View_empleadoEnsamble(), NoSemana, Year, IdPersona);
                return PartialView(turnos);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ProduccionModV2Ctrl.Terminar();
                return BadRequest(ex.Message);
            }
        }

        [AccessView]
        public ActionResult VerEmpleadoProds(int Id, int Year)
        {
            try
            {

               
                if(Id == 0 && Year == 0)
                {
                    DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                    Calendar cal = dfi.Calendar;

                    return RedirectToAction("VerEmpleadoProds", new { Id = cal.GetWeekOfYear(DateTime.Now, dfi.CalendarWeekRule, dfi.FirstDayOfWeek), Year = DateTime.Now.Year });
                }
                else
                {
                    var turnos = ProduccionModV2Ctrl.EmpleadoProds(Id, Year);
                    return View(turnos);
                }
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ProduccionModV2Ctrl.Terminar();
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [AccessView]
        public ActionResult Change([FromBody] EmpleadoGrupo empleadoGrupo)
        {
            try
            {
                ProduccionModV2Ctrl.CambiarGrupo(empleadoGrupo.Personas, empleadoGrupo.NoSemana, empleadoGrupo.year_);
                return Ok("Cambios Guardados");
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ProduccionModV2Ctrl.Terminar();
                return BadRequest(ex.Message);
            }
        }
    }
}
