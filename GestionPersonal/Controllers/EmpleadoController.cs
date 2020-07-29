using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPSInformation;
using GPSInformation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class EmpleadoController : Controller
    {
        private DarkManager darkManager;
        private SelectList Generos;
        private SelectList EstadosCiviles;
        public EmpleadoController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.Persona);
            darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);

            Generos = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 2, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            EstadosCiviles = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 3, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
        }

        ~EmpleadoController()
        {

        }

        // GET: Empleado
        public ActionResult Index()
        {
            var result = darkManager.Persona.Get().OrderBy(a => a.Nombre).ToList();
            return View(result);
        }

        // GET: Empleado/Create
        public ActionResult Create()
        {
            ViewData["Generos"] = Generos;
            ViewData["EstadosCiviles"] = EstadosCiviles;
            return View();
        }

        // GET: Empleado/Edit
        public ActionResult Edit(int id)
        {
            var result = darkManager.Persona.Get(id);
            if (result == null)
                return NotFound();
            return View();
        }
    }
}