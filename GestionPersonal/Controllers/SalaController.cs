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
        }

        ~SalaController()
        {

        }

        #region Sala
        [AccessMultipleView(IdAction = new int[] { 33, 35 })]
        public ActionResult Index()
        {
            return View();
        }

        [AccessDataSession(IdAction = new int[] { 33 })]
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
        //[ValidateAntiForgeryToken]
        [AccessDataSession(IdAction = new int[] { 35 })]
        public ActionResult Create(Sala Sala)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return BadRequest("Algunos campos son invalidos");
                }
                darkManager.Sala.Element = Sala;
                if (darkManager.Sala.Add())
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

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AccessDataSession(IdAction = new int[] { 35 })]
        public ActionResult Edit(Sala Sala)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return BadRequest("Algunos campos son invalidos");
                }
                darkManager.Sala.Element = Sala;
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
                return BadRequest( ex.Message);
            }
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
        #endregion


    }
}