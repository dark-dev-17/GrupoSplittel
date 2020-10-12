using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionPersonal.Models;
using GPSInformation;
using GPSInformation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class SalaController : Controller
    {
        private DarkManager darkManager;

        public SalaController(IConfiguration configuration)
        {
            darkManager = new DarkManager(configuration);
            darkManager.OpenConnection();
            darkManager.LoadObject(GpsManagerObjects.Sala);
            darkManager.LoadObject(GpsManagerObjects.SalaReservacion);
            darkManager.LoadObject(GpsManagerObjects.Persona);
            darkManager.LoadObject(GpsManagerObjects.Empleado);
            darkManager.LoadObject(GpsManagerObjects.AccesosSistema);
        }

        ~SalaController()
        {

        }

        #region Sala
        [AccessMultipleView(IdAction = new int[] { 33 })]
        public ActionResult Reservar()
        {
            var AccesoAdmin = darkManager.AccesosSistema.Get(
                "IdUsuario","" + (int)HttpContext.Session.GetInt32("user_id_permiss"),
                "IdSubModulo", "35");
            bool access = AccesoAdmin != null ? AccesoAdmin.TieneAcceso : false;
            ViewData["access"] = access;
            return View();
        }

        [AccessMultipleView(IdAction = new int[] { 35 })]
        public ActionResult Index()
        {
            
            return View(darkManager.Sala.Get());
        }

        [AccessMultipleView(IdAction = new int[] { 33, 35 })]
        public ActionResult Details(int id)
        {
            var result = darkManager.Sala.Get(id);
            if(result == null)
            {
                return BadRequest("No se encontró ninguna sala");
            }
            return Ok(result);
        }

        [HttpPost]
        [AccessDataSession(IdAction = new int[] { 33 })]
        public ActionResult GetList()
        {
            var result = darkManager.Sala.Get().OrderBy(a => a.Nombre);
            return Ok(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 35 })]
        public ActionResult Create(Sala Sala)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(Sala);
                }
                darkManager.Sala.Element = Sala;
                if (darkManager.Sala.Add())
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", darkManager.GetLastMessage());
                    return View(Sala);
                }
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(Sala);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessMultipleView(IdAction = new int[] { 35 })]
        public ActionResult Edit(Sala Sala)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(Sala);
                }
                darkManager.Sala.Element = Sala;
                if (darkManager.Sala.Update())
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", darkManager.GetLastMessage());
                    return View(Sala);
                }
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(Sala);
            }
        }
        [AccessMultipleView(IdAction = new int[] { 35 })]
        public ActionResult Edit(int id)
        {
            return View(darkManager.Sala.Get(id));
        }
        [AccessMultipleView(IdAction = new int[] { 35 })]
        public ActionResult Create()
        {
            return View();
        }

        #endregion

        #region Reservaciones
        [AccessDataSession(IdAction = new int[] { 33 })]
        public ActionResult DetailsReservacion(int id)
        {
            var result = darkManager.SalaReservacion.Get(id);
            if (result == null)
            {
                return BadRequest("No se encontró ninguna reservación");
            }
            return Ok(result);
        }

        [HttpPost]
        [AccessDataSession(IdAction = new int[] { 33 })]
        public ActionResult GetListReservacion()
        {
            var result = darkManager.SalaReservacion.Get().OrderByDescending(a => a.FechaInicio);

            return Ok(result);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AccessDataSession(IdAction = new int[] { 33 })]
        public ActionResult CreateReservacion([FromBody]SalaReservacion SalaReservacion)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Algunos campos son invalidos");
                }
                darkManager.SalaReservacion.Element = SalaReservacion;
                darkManager.SalaReservacion.Element.IdPersona = (int)HttpContext.Session.GetInt32("user_id");
                darkManager.SalaReservacion.Element.Activa = true;
                if (darkManager.SalaReservacion.Add())
                {
                    return Ok(darkManager.SalaReservacion.GetLastId());
                }
                else
                {
                    return BadRequest(darkManager.GetLastMessage());
                }
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AccessDataSession(IdAction = new int[] { 33 })]
        public ActionResult EditReservacion(SalaReservacion SalaReservacion)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return BadRequest("Algunos campos son invalidos");
                }
                darkManager.SalaReservacion.Element = SalaReservacion;
                if (darkManager.Sala.Update())
                {
                    return Ok(darkManager.GetLastMessage());
                }
                else
                {
                    return BadRequest(darkManager.GetLastMessage());
                }
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        [AccessDataSession(IdAction = new int[] { 33 })]
        public ActionResult Ismine(int id)
        {
            try
            {
                var result = darkManager.SalaReservacion.Get(id);
                if (result == null)
                {
                    return BadRequest("No se encontró ninguna sala");
                }
                if(result.IdPersona != (int)HttpContext.Session.GetInt32("user_id"))
                {
                    return BadRequest("No es tu reservación");
                }
                return Ok(result);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[ValidateAntiForgeryToken]
        [AccessDataSession(IdAction = new int[] { 33 })]
        public ActionResult Delete(int id)
        {
            try
            {
                var result = darkManager.SalaReservacion.Get(id);
                if (result == null)
                {
                    return BadRequest("No se encontró ninguna sala");
                }
                if (result.IdPersona != (int)HttpContext.Session.GetInt32("user_id"))
                {
                    return BadRequest("No es tu reservación");
                }
                darkManager.SalaReservacion.Element = result;
                if (!darkManager.SalaReservacion.Delete())
                {
                    return BadRequest("Error al eliminar");
                }
                else
                {
                    return Ok("Se elimino la reservación");
                }
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion


    }
}