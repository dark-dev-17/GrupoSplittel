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
    public class CatalogoOpcionesController : Controller
    {
        private DarkManager darkManager;
        public CatalogoOpcionesController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.CatalogoOpciones);
            darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);
        }

        // GET: Direccion
        public async Task<IActionResult> Index()
        {
            var List = darkManager.CatalogoOpciones.Get();
            darkManager.CloseConnection();
            return View(List);
        }


        #region CatalogoOpciones
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CatalogoOpciones catalogoOpciones)
        {

            if (!ModelState.IsValid)
            {
                return View(catalogoOpciones);
            }

            darkManager.CatalogoOpciones.Element = catalogoOpciones;
            var result = darkManager.CatalogoOpciones.Add();
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "error al insertar");
                return View(catalogoOpciones);
            }

        }

        public IActionResult Edit(int id)
        {
            var List = darkManager.CatalogoOpciones.Get(id);
            
            return View(List);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CatalogoOpciones catalogoOpciones)
        {

            if (!ModelState.IsValid)
            {
                return View(catalogoOpciones);
            }

            darkManager.CatalogoOpciones.Element = catalogoOpciones;
            var result = darkManager.CatalogoOpciones.Update();
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "error al actualizar");
                return View(catalogoOpciones);
            }

        }
        #endregion

        #region CatalogoOpcionesValores
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateValue(CatalogoOpcionesValores CatalogoOpcionesValores)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(CatalogoOpcionesValores);
                }

                darkManager.CatalogoOpcionesValores.Element = CatalogoOpcionesValores;
                var result = darkManager.CatalogoOpciones.Add();
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "error al insertar");
                    return View(CatalogoOpcionesValores);
                }
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("", "error al insertar");
                return View(CatalogoOpcionesValores);
            }
        }
        public IActionResult EditValue(CatalogoOpcionesValores CatalogoOpcionesValores)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(CatalogoOpcionesValores);
                }

                darkManager.CatalogoOpcionesValores.Element = CatalogoOpcionesValores;
                var result = darkManager.CatalogoOpcionesValores.Add();
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "error al insertar");
                    return View(CatalogoOpcionesValores);
                }
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("", "error al insertar");
                return View(CatalogoOpcionesValores);
            }
        }
        #endregion
    }
}
