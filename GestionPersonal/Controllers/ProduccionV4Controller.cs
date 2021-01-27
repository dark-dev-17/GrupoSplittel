using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionPersonal.Models;
using GPSInformation;
using GPSInformation.Controllers;
using GPSInformation.Models.Produccion;
using GPSInformation.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace GestionPersonal.Controllers
{
    public class ProduccionV4Controller : Controller
    {
        private ProduccionV4Ctrl ProduccionV4Ctrl;

        public ProduccionV4Controller(IConfiguration configuration)
        {
            ProduccionV4Ctrl = new ProduccionV4Ctrl(new DarkManager(configuration));
        }

        //[AccessDataSession]
        [HttpPost]
        public IActionResult DeleteInci(int IdGrupoProdIncidencia, int IdPersona)
        {
            try
            {
                ProduccionV4Ctrl.DeleteInci(IdGrupoProdIncidencia, IdPersona);
                return Ok("Cambios guardados");
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ProduccionV4Ctrl.Terminar();
                return BadRequest(ex.Message);
            }
        }
        //[AccessDataSession]
        [HttpPost]
        public IActionResult DetailsInci(int IdGrupoProdIncidencia, int IdPersona)
        {
            try
            {
                var data = ProduccionV4Ctrl.DetailsInci(IdGrupoProdIncidencia, IdPersona);
                return Ok(data);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ProduccionV4Ctrl.Terminar();
                return BadRequest(ex.Message);
            }
        }
        //[AccessDataSession]
        [HttpPost]
        public IActionResult RegisterIncidencia([FromBody]GrupoProdIncidencia GrupoProdIncidencia)
        {
            try
            {
                ProduccionV4Ctrl.RegisterIncidencia(GrupoProdIncidencia);
                return Ok("Cambios guardados");
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ProduccionV4Ctrl.Terminar();
                return BadRequest(ex.Message);
            }
        }
        [AccessView]
        public IActionResult Index(DateTime Inicio)
        {
            try
            {
                if(Inicio == DateTime.Parse("0001-01-01 00:00:00"))
                {
                    Inicio = Funciones.GetFirtsDatWeek(DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 05:45:00")));
                }
                var result = ProduccionV4Ctrl.ProcesarEmpleados(Inicio);
                return View(result);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ProduccionV4Ctrl.Terminar();
                return NotFound(ex.Message);
            }
        }
        [AccessView]
        public IActionResult Details(int IdPersona,DateTime Inicio)
        {
            try
            {
                if (Inicio == DateTime.Parse("0001-01-01 00:00:00"))
                {
                    Inicio = Funciones.GetFirtsDatWeek(DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 05:45:00")));
                }
                var result = ProduccionV4Ctrl.ProcesarEmpleado(new GPSInformation.Views.View_empleadoEnsamble(), IdPersona, Inicio);
                ViewData["PagoPermisoPersonal"] = new SelectList(ProduccionV4Ctrl.darkManager.CatalogoOpcionesValores.Get("" + 1010, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
                return View(result);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ProduccionV4Ctrl.Terminar();
                return NotFound(ex.Message);
            }
        }
        //[AccessDataSession]
        [HttpPost]
        public IActionResult AddArregloEvento(int IdEvent, int IdPersona, DateTime Fecha, string Comentarios)
        {
            try
            {
                ProduccionV4Ctrl.AddArreglo(IdEvent, IdPersona, Fecha, Comentarios);
                return Ok("Datos cambiados");
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ProduccionV4Ctrl.Terminar();
                return BadRequest(ex.Message);
            }
        }
        //[AccessDataSession]
        [HttpPost]
        public IActionResult DeleteArreglo(int IdEvent, int IdPersona)
        {
            try
            {
                ProduccionV4Ctrl.DeleteArreglo(IdEvent, IdPersona);
                return Ok("Datos cambiados");
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ProduccionV4Ctrl.Terminar();
                return BadRequest(ex.Message);
            }
        }
    }
}
