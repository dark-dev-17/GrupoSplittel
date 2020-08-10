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
    public class UsuarioController : Controller
    {
        private DarkManager darkManager;
        private SelectList Personas;
        public UsuarioController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.Usuario);
            darkManager.LoadObject(GpsManagerObjects.Persona);
        }

        // GET: Direccion
        public IActionResult Index()
        {
            var List = darkManager.Usuario.Get();
            darkManager.CloseConnection();
            return View(List);
        }


        #region Usuario
        public IActionResult Create()
        {
            Personas = new SelectList(darkManager.Persona.Get().OrderBy(a => a.NombreCompelto).ToList(), "IdPersona", "NombreCompelto");
            ViewData["Personas"] = Personas;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Usuario Usuario)
        {
            
            Personas = new SelectList(darkManager.Persona.Get().OrderBy(a => a.NombreCompelto).ToList(), "IdPersona", "NombreCompelto");
            ViewData["Personas"] = Personas;

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(Usuario);
                }

                darkManager.Usuario.Element = Usuario;
                darkManager.Usuario.Element.UltimoIngreso = DateTime.Now;
                var result = darkManager.Usuario.Add();
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "error al insertar");
                    return View(Usuario);
                }
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(Usuario);
            }
        }

        public IActionResult Edit(int id)
        {
            var List = darkManager.Usuario.Get(id);
            Personas = new SelectList(darkManager.Persona.Get().OrderBy(a => a.NombreCompelto).ToList(), "IdPersona", "NombreCompelto", List.IdPersona);
            ViewData["Personas"] = Personas;
            return View(List);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Usuario Usuario)
        {

            if (!ModelState.IsValid)
            {
                return View(Usuario);
            }

            darkManager.Usuario.Element = Usuario;
            var result = darkManager.Usuario.Update();
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "error al actualizar");
                return View(Usuario);
            }

        }
        #endregion
    }
}
