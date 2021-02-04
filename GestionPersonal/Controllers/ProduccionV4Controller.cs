using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GestionPersonal.Models;
using GPSInformation;
using GPSInformation.Controllers;
using GPSInformation.Models.Produccion;
using GPSInformation.Reportes.ProduccionV3;
using GPSInformation.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace GestionPersonal.Controllers
{
    public class ProduccionV4Controller : Controller
    {
        private ProduccionV4Ctrl ProduccionV4Ctrl;

        public ProduccionV4Controller(IConfiguration configuration)
        {
            ProduccionV4Ctrl = new ProduccionV4Ctrl(new DarkManager(configuration));
        }

        #region Incidencias
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
        public IActionResult RegisterIncidencia([FromBody] GrupoProdIncidencia GrupoProdIncidencia)
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
        #endregion

        #region Arreglos eventos
        //[AccessDataSession]


        //[AccessDataSession]
        [HttpPost]
        public IActionResult AddArregloEvento(int IdEvent, int IdPersona, DateTime Fecha, string Comentarios)
        {
            try
            {
                if (string.IsNullOrEmpty(Comentarios))
                {
                    return BadRequest("Por favor llena todos los campos");
                }
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
        #endregion

        #region Reporte
        [AccessMultipleView(IdAction = new int[] { 51 })]
        public IActionResult Index(DateTime Inicio)
        {
            try
            {
                var Permisos = ProduccionV4Ctrl.VerPermisos((int)HttpContext.Session.GetInt32("user_id"));

                

                if(Permisos.Count(a => a.Autorization == true) <= 0)
                {
                    return RedirectToAction("MiReporte");
                }
                ViewData["Permisos"] = Permisos;
                if (Inicio == DateTime.Parse("0001-01-01 00:00:00"))
                {
                    Inicio = Funciones.GetFirtsDatWeek(DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 05:45:00")));
                }
                else
                {
                    Inicio = Funciones.GetFirtsDatWeek(Inicio);
                }
                if (Inicio < Funciones.GetFirtsDatWeek(DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 05:45:00"))))
                {
                    if (Permisos.Find(a => a.IdSubModulo == 58).Autorization == false && Permisos.Find(a => a.IdSubModulo == 53).Autorization == false)
                    {
                        Inicio = Funciones.GetFirtsDatWeek(DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 05:45:00")));
                    }
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
        [AccessMultipleView(IdAction = new int[] { 51 })]
        public IActionResult Details(int IdPersona, DateTime Inicio)
        {
            try
            {
                var Permisos = ProduccionV4Ctrl.VerPermisos((int)HttpContext.Session.GetInt32("user_id"));
                if (Inicio == DateTime.Parse("0001-01-01 00:00:00"))
                {
                    Inicio = Funciones.GetFirtsDatWeek(DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 05:45:00")));
                }
                else
                {
                    Inicio = Funciones.GetFirtsDatWeek(Inicio);
                }
                if (DateTime.Now < Inicio)
                {
                    if (Permisos.Find(a => a.IdSubModulo == 58).Autorization == false && Permisos.Find(a => a.IdSubModulo == 53).Autorization == false)
                    {
                        Inicio = Funciones.GetFirtsDatWeek(DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 05:45:00")));
                    }
                }

                ViewData["Permisos"] = Permisos;
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
        [AccessView]
        public IActionResult MiReporte(DateTime Inicio)
        {
            try
            {
                var Permisos = ProduccionV4Ctrl.VerPermisos((int)HttpContext.Session.GetInt32("user_id"));
                int IdPersona = (int)HttpContext.Session.GetInt32("user_id");
                if (Inicio == DateTime.Parse("0001-01-01 00:00:00"))
                {
                    Inicio = Funciones.GetFirtsDatWeek(DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 05:45:00")));
                }
                else
                {
                    Inicio = Funciones.GetFirtsDatWeek(Inicio);
                }
                var result = ProduccionV4Ctrl.ProcesarEmpleado(new GPSInformation.Views.View_empleadoEnsamble(), IdPersona, Inicio);
                ViewData["Permisos"] = Permisos;
                ViewData["PagoPermisoPersonal"] = new SelectList(ProduccionV4Ctrl.darkManager.CatalogoOpcionesValores.Get("" + 1010, "IdCatalogoOpciones").OrderBy(a => a.Descripcion).ToList(), "IdCatalogoOpcionesValores", "Descripcion");
                return View(result);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ProduccionV4Ctrl.Terminar();
                return NotFound(ex.Message);
            }
        }

        #endregion

        #region Cambios de turno
        [HttpPost]
        public IActionResult CreateCorte(int IdPersona, DateTime Corte)
        {
            try
            {
                ProduccionV4Ctrl.CreateCorte(IdPersona, Corte);
                return Ok("Datos cambiados");
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ProduccionV4Ctrl.Terminar();
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult DeleteCambio(int IdPersona, int IdGrupoCambios)
        {
            try
            {
                ProduccionV4Ctrl.DeleteCambio(IdGrupoCambios,IdPersona);
                return Ok("Datos cambiados");
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ProduccionV4Ctrl.Terminar();
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult CreateCambio([FromBody]GrupoCambios createCambio)
        {
            try
            {
                ProduccionV4Ctrl.AddCambio(createCambio);
                return Ok("Datos cambiados");
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ProduccionV4Ctrl.Terminar();
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Permisos
        [HttpPost]
        public IActionResult ChangePermisos([FromBody]List<PermisosBloq> Permisos)
        {
            try
            {
                ProduccionV4Ctrl.ChangePermisos(Permisos);
                return Ok("Datos cambiados");
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ProduccionV4Ctrl.Terminar();
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult VerPermisos(int IdPersona_)
        {
            try
            {
                var data = ProduccionV4Ctrl.VerPermisos(IdPersona_);
                return Ok(data);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                ProduccionV4Ctrl.Terminar();
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region reporte Excel
        [HttpGet]
        public ActionResult DescargarReporte(DateTime Inicio)
        {
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                package.Workbook.Worksheets.Add("Worksheet1");
                var excelWorksheet = package.Workbook.Worksheets["Worksheet1"];

                var respuesta = ProduccionV4Ctrl.ProcesarEmpleados(Inicio);



                #region Agregar columnas
                excelWorksheet.Cells[1, 1].Value = "Inicio";
                excelWorksheet.Cells[2, 1].Value = "Fin";
                excelWorksheet.Cells[1, 2].Value = respuesta.Inicio.ToString("F");
                excelWorksheet.Cells[2, 2].Value = respuesta.Fin.ToString("F");

                excelWorksheet.Cells[5, 1].Value = "Nomina";
                excelWorksheet.Cells[5, 2].Value = "Empleado";
                excelWorksheet.Cells[5, 3].Value = "Puesto";
                excelWorksheet.Cells[5, 4].Value = "Hrs.Semana";
                excelWorksheet.Cells[5, 5].Value = "Hrs.Trabajadas";
                excelWorksheet.Cells[5, 6].Value = "Hrs.Justificadas";
                excelWorksheet.Cells[5, 7].Value = "Horas Score";
                excelWorksheet.Cells[5, 8].Value = "Estatus";

                for (int i = 1; i < 9; i++)
                {
                    Color myColor = System.Drawing.ColorTranslator.FromHtml("#ffab40");
                    excelWorksheet.Cells[5, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    excelWorksheet.Cells[5, i].Style.Fill.BackgroundColor.SetColor(myColor);
                }
                
                #endregion

                int Fila = 6;
                respuesta.EmpleadoProds.OrderBy(a => a.NombreCompleto).ToList().ForEach(emp => 
                {
                    excelWorksheet.Cells[Fila, 1].Value = emp.NumeroNomina;
                    excelWorksheet.Cells[Fila, 2].Value = emp.NombreCompleto;
                    excelWorksheet.Cells[Fila, 3].Value = emp.PuestoNombre;
                    excelWorksheet.Cells[Fila, 4].Value = string.Format("{0:#.##}", emp.HorasMeta);
                    excelWorksheet.Cells[Fila, 5].Value = string.Format("{0:#.##}", emp.HorasReal);
                    excelWorksheet.Cells[Fila, 6].Value = string.Format("{0:#.##}", emp.HorasAprobadas);
                    excelWorksheet.Cells[Fila, 7].Value = string.Format("{0:#.##}", emp.HorasScore);


                    



                    if (emp.HorasScore > 0)
                    {
                        excelWorksheet.Cells[Fila, 8].Value = "Debe horas";
                        Color myColor = System.Drawing.ColorTranslator.FromHtml("#ff616f");
                        excelWorksheet.Cells[Fila, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        excelWorksheet.Cells[Fila, 8].Style.Fill.BackgroundColor.SetColor(myColor);
                    }
                    else
                    {
                        excelWorksheet.Cells[Fila, 8].Value = "Horas a empleado";
                        Color myColor = System.Drawing.ColorTranslator.FromHtml("#66ffa6");
                        excelWorksheet.Cells[Fila, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        excelWorksheet.Cells[Fila, 8].Style.Fill.BackgroundColor.SetColor(myColor);
                    }
                    Fila++;


                });

                excelWorksheet.Cells.AutoFitColumns();
                package.Save();
            }

            stream.Position = 0;
            string excelName = $"Rep_ProduccionHrs_{Inicio.ToString("yyyyMMddHHmmssfff")}.xlsx";
            // above I define the name of the file using the current datetime.
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName); // this will be the actual export.
        }
        #endregion
    }
}
