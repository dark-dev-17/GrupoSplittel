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
            ViewData["Valores"] = darkManager.CatalogoOpcionesValores.Get(id+"", "IdCatalogoOpciones");
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
        public IActionResult CreateValue(int id)
        {
            var OpcionesCatalogos = darkManager.CatalogoOpciones.Get().OrderBy(a => a.Descripcion);
            ViewData["OpcionesCatalogos"] = new SelectList(OpcionesCatalogos, "IdCatalogoOpciones", "Descripcion", id);
            return View(new CatalogoOpcionesValores { IdCatalogoOpciones = id  });
        }
        public IActionResult EditValue(int id)
        {
            var OpcionesCatalogos = darkManager.CatalogoOpciones.Get().OrderBy(a => a.Descripcion);
            var valorCatalogo = darkManager.CatalogoOpcionesValores.Get(id);
            ViewData["OpcionesCatalogos"] = new SelectList(OpcionesCatalogos, "IdCatalogoOpciones", "Descripcion", valorCatalogo.IdCatalogoOpciones);
            return View(valorCatalogo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateValue(CatalogoOpcionesValores CatalogoOpcionesValores)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewData["OpcionesCatalogos"] = new SelectList(darkManager.CatalogoOpciones.Get().OrderBy(a => a.Descripcion), "IdCatalogoOpciones", "Descripcion", CatalogoOpcionesValores.IdCatalogoOpciones);
                    return View(CatalogoOpcionesValores);
                }

                darkManager.CatalogoOpcionesValores.Element = CatalogoOpcionesValores;
                var result = darkManager.CatalogoOpcionesValores.Add();
                if (result)
                {
                    return RedirectToAction(nameof(Edit), new { id = CatalogoOpcionesValores.IdCatalogoOpciones});
                }
                else
                {
                    ModelState.AddModelError("", "error al insertar");
                    ViewData["OpcionesCatalogos"] = new SelectList(darkManager.CatalogoOpciones.Get().OrderBy(a => a.Descripcion), "IdCatalogoOpciones", "Descripcion", CatalogoOpcionesValores.IdCatalogoOpciones);
                    return View(CatalogoOpcionesValores);
                }
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewData["OpcionesCatalogos"] = new SelectList(darkManager.CatalogoOpciones.Get().OrderBy(a => a.Descripcion), "IdCatalogoOpciones", "Descripcion", CatalogoOpcionesValores.IdCatalogoOpciones);
                return View(CatalogoOpcionesValores);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditValue(CatalogoOpcionesValores CatalogoOpcionesValores)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewData["OpcionesCatalogos"] = new SelectList(darkManager.CatalogoOpciones.Get().OrderBy(a => a.Descripcion), "IdCatalogoOpciones", "Descripcion", CatalogoOpcionesValores.IdCatalogoOpciones);
                    return View(CatalogoOpcionesValores);
                }

                darkManager.CatalogoOpcionesValores.Element = CatalogoOpcionesValores;
                var result = darkManager.CatalogoOpcionesValores.Update();
                if (result)
                {
                    return RedirectToAction(nameof(Edit), new { id = CatalogoOpcionesValores.IdCatalogoOpciones });
                }
                else
                {
                    ModelState.AddModelError("", "error al insertar");
                    ViewData["OpcionesCatalogos"] = new SelectList(darkManager.CatalogoOpciones.Get().OrderBy(a => a.Descripcion), "IdCatalogoOpciones", "Descripcion", CatalogoOpcionesValores.IdCatalogoOpciones);
                    return View(CatalogoOpcionesValores);
                }
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewData["OpcionesCatalogos"] = new SelectList(darkManager.CatalogoOpciones.Get().OrderBy(a => a.Descripcion), "IdCatalogoOpciones", "Descripcion", CatalogoOpcionesValores.IdCatalogoOpciones);
                return View(CatalogoOpcionesValores);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteValue(int id)
        {
            try
            {
                var objeto = darkManager.CatalogoOpcionesValores.Get(id);
                if (objeto == null)
                {
                    return NotFound();
                }
                darkManager.CatalogoOpcionesValores.Element = new CatalogoOpcionesValores { IdCatalogoOpcionesValores = id };
                if (darkManager.CatalogoOpcionesValores.Delete())
                {
                    return RedirectToAction(nameof(Edit), new { id = objeto.IdCatalogoOpciones });
                }
                else
                {
                    return NotFound();
                }
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        #endregion
    }
}
