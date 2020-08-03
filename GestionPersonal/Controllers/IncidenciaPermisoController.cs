using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPSInformation;
using GPSInformation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using static Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary;

namespace GestionIncidenciaPermisol.Controllers
{
    public class IncidenciaPermisoController : Controller
    {
        private DarkManager darkManager;
        private SelectList TiposPermisos;
        private SelectList PagoPermisoPersonal;

        public IncidenciaPermisoController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.IncidenciaPermiso);
            darkManager.LoadObject(GpsManagerObjects.IncidenciaPermisoProcess);
            darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);
            darkManager.LoadObject(GpsManagerObjects.Persona);
            darkManager.LoadObject(GpsManagerObjects.Puesto);
            darkManager.LoadObject(GpsManagerObjects.Empleado);
        }

        ~IncidenciaPermisoController()
        {

        }

        public ActionResult Create(int id)
        {
            TiposPermisos = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 1009, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            PagoPermisoPersonal = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 1010, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["TiposPermisos"] = TiposPermisos;
            ViewData["PagoPermisoPersonal"] = PagoPermisoPersonal;
            return View(new IncidenciaPermiso() { IdPersona = id, Fecha = DateTime.Now });
        }

        public ActionResult Edit(int id)
        {
            TiposPermisos = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 1009, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            PagoPermisoPersonal = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 1010, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["TiposPermisos"] = TiposPermisos;
            ViewData["PagoPermisoPersonal"] = PagoPermisoPersonal;

            var response =  darkManager.IncidenciaPermiso.Get(id);
            return View(response);
        }

        public ActionResult Details(int id)
        {
            TiposPermisos = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 1009, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            PagoPermisoPersonal = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 1010, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["TiposPermisos"] = TiposPermisos;
            ViewData["PagoPermisoPersonal"] = PagoPermisoPersonal;

            var response = darkManager.IncidenciaPermiso.Get(id);
            return View(response);
        }

        // POST: IncidenciaPermiso/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IncidenciaPermiso IncidenciaPermiso)
        {
            TiposPermisos = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 1009, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            PagoPermisoPersonal = new SelectList(darkManager.CatalogoOpcionesValores.Get("" + 1010, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
            ViewData["TiposPermisos"] = TiposPermisos;
            ViewData["PagoPermisoPersonal"] = PagoPermisoPersonal;
            
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(IncidenciaPermiso);
                }

                darkManager.IncidenciaPermiso.Element = IncidenciaPermiso;
                darkManager.IncidenciaPermiso.Element.CreadoPor = "Empleado";
                if(darkManager.IncidenciaPermiso.Element.IdAsunto == 36)
                {
                    darkManager.IncidenciaPermiso.Element.IdPagoPermiso = 0;
                }

                bool result = darkManager.IncidenciaPermiso.Add();
                if (result)
                {
                    darkManager.IncidenciaPermisoProcess.Element = new IncidenciaPermisoProcess() {
                        IdIncidenciaPermiso = darkManager.IncidenciaPermiso.GetLastId(),
                        Fecha = DateTime.Now,
                        Estatus = 1,
                        Comentarios = "El empleado ha creado una incidencia",
                        IdPersona = IncidenciaPermiso.IdPersona,
                    };
                    darkManager.IncidenciaPermisoProcess.Add();
                    return RedirectToAction("Index", "Incidencia", new { id = IncidenciaPermiso.IdPersona });
                }
                else
                {
                    return View(IncidenciaPermiso);
                }
            }
            catch(GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(IncidenciaPermiso);
            }
        }
    }
}