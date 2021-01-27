using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPSInformation;
using GPSInformation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class ProduccionV1Controller : Controller
    {
        private ProduccionModV3Crtl ProduccionModV3Ctrl;

        public ProduccionV1Controller(IConfiguration configuration)
        {
            ProduccionModV3Ctrl = new ProduccionModV3Crtl(new DarkManager(configuration));
        }
        public IActionResult Index()
        {
            try
            {
                var result = ProduccionModV3Ctrl.GetEmpleados(DateTime.Parse("2021-01-04"));
                return View(result);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ProduccionModV3Ctrl.Terminar();
                return NotFound(ex.Message);
            }
            
        }
    }
}
