using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionPersonal.Models;
using GestionPersonal.Service;
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
    public class BuzonQuejaController : Controller
    {
        private BuzonQuejaCtrl QuejasCtrl;
        private readonly IViewRenderService _viewRenderService;

        public BuzonQuejaController(IConfiguration configuration, IViewRenderService viewRenderService)
        {
            _viewRenderService = viewRenderService;
            QuejasCtrl = new BuzonQuejaCtrl(new DarkManager(configuration));
        }

        // GET: QuejaPersonaController
        [AccessMultipleView(IdAction = new int[] { 44 })]
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
            return View();
        }

        // POST: QuejaPersonaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BuzonQueja BuzonQueja)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(BuzonQueja);
                }
                var LastBuzon = QuejasCtrl.AddQueja(BuzonQueja);
                if (LastBuzon != null)
                {
                    var result = await _viewRenderService.RenderToStringAsync("BuzonQueja/DetailsEmail", LastBuzon);
                    QuejasCtrl.EnviarCorreo(result);
                }
                
                return RedirectToAction(nameof(Complete));
            }
            catch (GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(BuzonQueja);
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
