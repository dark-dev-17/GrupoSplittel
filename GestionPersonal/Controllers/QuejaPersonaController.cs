using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionPersonal.Service;
using GPSInformation;
using GPSInformation.Controllers;
using GPSInformation.Exceptions;
using GPSInformation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class QuejaPersonaController : Controller
    {
        private QuejasCtrl QuejasCtrl;
        private readonly IViewRenderService _viewRenderService;

        public QuejaPersonaController(IConfiguration configuration, IViewRenderService viewRenderService)
        {
            _viewRenderService = viewRenderService;
            QuejasCtrl = new QuejasCtrl(new DarkManager(configuration));
        }

        // GET: QuejaPersonaController
        public ActionResult Index()
        {
            try
            {
                return View(QuejasCtrl.GetQuejaPersonas());
            }
            catch (GpExceptions ex)
            {
                return NotFound(ex.Message);
            }
            finally
            {
                QuejasCtrl.Terminar();
                QuejasCtrl = null;
            }
        }

        // GET: QuejaPersonaController/Create
        public ActionResult Create()
        {
            ViewData["Empleados"] = new SelectList(QuejasCtrl.GetEmpleados(), "IdPersona", "NombreCompleto");
            QuejasCtrl.Terminar();
            QuejasCtrl = null;
            return View();
        }

        // POST: QuejaPersonaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QuejaPersona QuejaPersona)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewData["Empleados"] = new SelectList(QuejasCtrl.GetEmpleados(), "IdPersona", "NombreCompleto", QuejaPersona.IdPersona);
                    return View(QuejaPersona);
                }
                QuejasCtrl.AddQueja(QuejaPersona);
                return RedirectToAction(nameof(Complete));
            }
            catch (GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(QuejaPersona);
            }
            finally
            {
                QuejasCtrl.Terminar();
                QuejasCtrl = null;
            }
        }

        // GET: QuejaPersonaController/Edit/5
        public ActionResult Complete()
        {
            return View();
        }

        
    }
}
