using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GestionPersonal.Models;
using GPSInformation;
using GPSInformation.Controllers;
using GPSInformation.Exceptions;
using GPSInformation.Models;
using GPSInformation.Reportes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using MySqlX.XDevAPI.Relational;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace GestionPersonal.Controllers
{
    public class PrenominaController : Controller
    {
        private PrenominaCtrl PrenominaCtrl;

        public PrenominaController(IConfiguration configuration)
        {
            PrenominaCtrl = new PrenominaCtrl(new DarkManager(configuration));
        }

        // GET: EvaluacionController
        [AccessMultipleView(IdAction = new int[] { 37 })]
        public ActionResult Index()
        {
            try
            {
                return View(PrenominaCtrl.GetExpediente());
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return View(ex.Message);
            }
           
        }

        [AccessMultipleView(IdAction = new int[] { 37 })]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Prenomina_Rep prenomina_Rep)
        {
            try
            {
                var empleados_re = PrenominaCtrl.GetExpediente(prenomina_Rep);
                ViewData["Empleados"] = empleados_re;
                ViewData["Dias"] = PrenominaCtrl.GetPreniminaLists(prenomina_Rep, empleados_re);
                return View(prenomina_Rep);
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return View(ex.Message);
            }

        }
        [AccessMultipleView(IdAction = new int[] { 37 })]
        [HttpPost]
        public ActionResult Justificar([FromBody]FaltaJustificacion faltaJustificacion)
        {
            try
            {
                PrenominaCtrl.JustificarIncidencia(faltaJustificacion);
                return Ok("Justificación guardada");
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [AccessMultipleView(IdAction = new int[] { 37 })]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReporteExcel(Prenomina_Rep prenomina_Rep)
        {
            // above code loads the data using LINQ with EF (query of table), you can substitute this with any data source.
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                package.Workbook.Worksheets.Add("Worksheet1");
                var excelWorksheet = package.Workbook.Worksheets["Worksheet1"];
                
                #region Agregar cabeceras
                //CreatedAtRoute encabezado
                excelWorksheet.Cells[1, 1].Value = "Nomina";
                excelWorksheet.Cells[1, 2].Value = "Empleado";
                excelWorksheet.Cells[1, 3].Value = "Departamento";
                excelWorksheet.Cells[1, 4].Value = "Puesto";
                //agregar dias
                int contador = 5;
                DateTime InitialDate = prenomina_Rep.Inicio;
                while (InitialDate <= prenomina_Rep.Fin)
                {
                    excelWorksheet.Cells[1, contador].Value = InitialDate.ToString("ddd - dd");
                    if(InitialDate.DayOfWeek == DayOfWeek.Saturday || InitialDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        Color myColor = System.Drawing.ColorTranslator.FromHtml("#5cb85c");
                        excelWorksheet.Cells[1, contador].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        excelWorksheet.Cells[1, contador].Style.Fill.BackgroundColor.SetColor(myColor); 
                    }
                    InitialDate = InitialDate.AddDays(1);
                    contador++;
                }
                var empleados_re = PrenominaCtrl.GetExpediente(prenomina_Rep);
                var dias_re = PrenominaCtrl.GetPreniminaLists(prenomina_Rep, empleados_re);
                contador = 2;
                empleados_re.ForEach(emp => {
                    int columna = 5;
                    excelWorksheet.Cells[contador, 1].Value = emp.NumeroNomina;
                    excelWorksheet.Cells[contador, 2].Value = emp.NombreCompleto;
                    excelWorksheet.Cells[contador, 3].Value = emp.NombreDepartamento;
                    excelWorksheet.Cells[contador, 4].Value = emp.PuestoNombre;
                    //agregar dias
                    InitialDate = prenomina_Rep.Inicio;
                    while (InitialDate <= prenomina_Rep.Fin)
                    {
                        excelWorksheet.Cells[contador, columna].Value = "";
                        if (InitialDate.DayOfWeek == DayOfWeek.Saturday || InitialDate.DayOfWeek == DayOfWeek.Sunday)
                        {
                            Color myColor = System.Drawing.ColorTranslator.FromHtml("#5cb85c");
                            excelWorksheet.Cells[contador, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            excelWorksheet.Cells[contador, columna].Style.Fill.BackgroundColor.SetColor(myColor);
                        }
                        else
                        {
                            List<PreniminaList> Lista = dias_re.Find(a => a.IdPersona == emp.IdPersona).Dias;
                            PreniminaList dia = Lista.Find(a => a.Fecha == InitialDate);
                            if(dia != null)
                            {
                                var incidencia = dia.Incidencias.ElementAt(0);

                                excelWorksheet.Cells[contador, columna].Value = incidencia.Clave;

                                Color myColor = System.Drawing.ColorTranslator.FromHtml(incidencia.Color);
                                excelWorksheet.Cells[contador, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                excelWorksheet.Cells[contador, columna].Style.Fill.BackgroundColor.SetColor(myColor);

                                myColor = System.Drawing.ColorTranslator.FromHtml(incidencia.TextColor);
                                excelWorksheet.Cells[contador, columna].Style.Font.Color.SetColor(myColor);

                            }
                        }
                        InitialDate = InitialDate.AddDays(1);
                        columna++;
                    }
                    contador++;
                });

                contador += 1;
                excelWorksheet.Cells[contador, 1].Value = "Clave";
                excelWorksheet.Cells[contador, 2].Value = "Descripcion";
                contador += 1;
                PrenominaCtrl.Nomenclatura.ForEach(a => {
                    excelWorksheet.Cells[contador, 1].Value = a.Clave;
                    excelWorksheet.Cells[contador, 2].Value = a.Title;
                    contador++;
                });

                excelWorksheet.Cells.AutoFitColumns();
                #endregion
                package.Save();
            }

            stream.Position = 0;
            string excelName = $"Prenomina-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            // above I define the name of the file using the current datetime.
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName); // this will be the actual export.
        }
    }
}
