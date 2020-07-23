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

namespace GestionPersonal.Controllers
{
    public class DireccionController : Controller
    {
        private DarkManager darkManager;
        public DireccionController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.CatalogoOpciones);
        }

        // GET: Direccion
        public async Task<IActionResult> Index()
        {
            var List = darkManager.CatalogoOpciones.Get();
            darkManager.CloseConnection();
            return Ok(List);
        }

    }
}
