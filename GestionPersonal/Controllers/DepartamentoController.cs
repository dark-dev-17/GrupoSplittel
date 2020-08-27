using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionPersonal.Models;
using GPSInformation;
using GPSInformation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class DepartamentoController : Controller
    {
        private DarkManager darkManager;
        private SelectList Direcciones;


        public DepartamentoController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.Departamento);
            darkManager.LoadObject(GpsManagerObjects.Direccion);

            Direcciones = new SelectList(darkManager.Direccion.Get().OrderBy(a => a.Nombre).ToList(), "IdDireccion", "Nombre");
        }

        ~DepartamentoController()
        {

        }

        // GET: Departamento
        [AccessMultipleView( IdAction = new int[] { 14,15})]
        public ActionResult Index()
        {
            var result = darkManager.Departamento.Get().OrderBy(a => a.Nombre).ToList();
            result.ForEach(a => {
                a.Direccion = darkManager.Direccion.Get(a.IdDireccion);
            });
            return View(result);
        }

        // GET: Departamento/Details/5
        [AccessMultipleView(IdAction = new int[] { 14, 15 })]
        public ActionResult Details(int id)
        {
            var result = darkManager.Departamento.Get(id);

            if(result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // GET: Departamento/Create
        [AccessMultipleView(IdAction = new int[] { 15 })]
        public ActionResult Create()
        {
            ViewData["Direcciones"] = Direcciones;
            return View();
        }

        // POST: Departamento/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 15 })]
        public ActionResult Create(Departamento Departamento)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    ViewData["Direcciones"] = Direcciones;
                    return View(Departamento);
                }

                darkManager.Departamento.Element = Departamento;
                bool result = darkManager.Departamento.Add();
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["Direcciones"] = Direcciones;
                    ModelState.AddModelError("", darkManager.GetLastMessage());
                    return View(Departamento);
                }
                
            }
            catch(GPSInformation.Exceptions.GpExceptions ex)
            {
                ViewData["Direcciones"] = Direcciones;
                ModelState.AddModelError("", ex.Message);
                return View(Departamento);
            }
        }

        // GET: Departamento/Edit/5
        [AccessMultipleView(IdAction = new int[] { 15 })]
        public ActionResult Edit(int id)
        {
            ViewData["Direcciones"] = Direcciones;
            var result = darkManager.Departamento.Get(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // POST: Departamento/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 15 })]
        public ActionResult Edit(Departamento Departamento)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    ViewData["Direcciones"] = Direcciones;
                    return View(Departamento);
                }

                darkManager.Departamento.Element = Departamento;
                bool result = darkManager.Departamento.Update();
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["Direcciones"] = Direcciones;
                    ModelState.AddModelError("", darkManager.GetLastMessage());
                    return View(Departamento);
                }

            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ViewData["Direcciones"] = Direcciones;
                ModelState.AddModelError("", ex.Message);
                return View(Departamento);
            }
        }
    }
}