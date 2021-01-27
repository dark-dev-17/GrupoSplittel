using GPSInformation.Models.Produccion;
using GPSInformation.Reportes.Produccion;
using GPSInformation.Tools;
using GPSInformation.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPSInformation.Controllers
{
    public class ProduccionModV3Crtl
    {
        #region Atributos
        private DarkManager darkManager;
        #endregion

        #region Constructores
        public ProduccionModV3Crtl(DarkManager darkManager)
        {
            this.darkManager = darkManager;
            this.darkManager.OpenConnection();
            this.darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);
            this.darkManager.LoadObject(GpsManagerObjects.View_empleadoEnsamble);
            this.darkManager.LoadObject(GpsManagerObjects.GrupoHorario);
            this.darkManager.LoadObject(GpsManagerObjects.GrupoCambios);
            this.darkManager.LoadObject(GpsManagerObjects.GrupoExcepcion);
        }
        #endregion
        #region Metodos
        public ReporteProdEmp GetEmpleados(DateTime Fecha)
        {
            var Empleados_re = darkManager.View_empleadoEnsamble.Get();

            ReporteProdEmp reporteProdEmp = new ReporteProdEmp
            {
                Empleados = new List<EmpleadoProduccion>(),
                Inicio = Funciones.GetFirtsDatWeek(Fecha),
                Fin = Funciones.GetLastDatWeek(Fecha)
            };

            this.darkManager.OpenConnectionAcces();
            this.darkManager.LoadObject(GpsControlAcceso.View_gps_ensambleSinFiltro);

            //llenar lista de empleados
            Empleados_re.ForEach(Empleado =>
            {
                reporteProdEmp.Empleados.Add(Processempleado(reporteProdEmp.Inicio, reporteProdEmp.Fin, Empleado));
            });

            return reporteProdEmp;
        }
        public EmpleadoProduccion Processempleado(DateTime Inicio, DateTime Fin, View_empleadoEnsamble Empleado, int IdPersona_ = 0)
        {
            if (IdPersona_ != 0)
            {
                Empleado = darkManager.View_empleadoEnsamble.GetOpenquerys($"where IdPersona = {IdPersona_}");
            }

            EmpleadoProduccion empleadoProduccion = new EmpleadoProduccion
            {
                IdPersona = Empleado.IdPersona,
                Nombre = Empleado.NombreCompleto,
                NumeroNomina = Empleado.NumeroNomina,
                Antiguedad = Empleado.Antiguedad,
                Puesto = Empleado.PuestoNombre,
                Dias = new List<DiaEmpleadoProd>()
            };

            #region busqueda de horari asignado
            DateTime Corte = Inicio;
            while (Corte <= Fin)
            {
                var UltimoCambio = GetUltimoCambio(Corte, Empleado.IdPersona);

                if (UltimoCambio != null)
                {
                    // obtener dia de acuerdo al grupo
                    var DiaHorario_re = darkManager.GrupoHorario.GetOpenquerys($"where IdGrupo = '{UltimoCambio.IdGrupo}' and dia = '{(int)Corte.DayOfWeek}'");
                    var GrupoExcepcion_re = darkManager.GrupoExcepcion.GetOpenquerys($"where IdPersona = '{Empleado.IdPersona}' and Fecha = '{Corte.ToString("yyyy-MM-dd")}'");
                    DiaEmpleadoProd DiaEmpleadoProd = new DiaEmpleadoProd
                    {
                        Fecha = Corte,
                        GrupoHorario = DiaHorario_re,
                        GrupoExcepcion = GrupoExcepcion_re,
                        HorasReal = 0,
                        HorasAprobadas= 0,
                        HorasMeta= 0,
                        Logs = new List<View_gps_ensambleSinFiltro>()
                    };
                    empleadoProduccion.Dias.Add(DiaEmpleadoProd);
                }
                else
                {
                    empleadoProduccion.Dias.Add(null);
                }

                Corte = Corte.AddDays(1);
            }
            #endregion
            #region Procesar entradas
            empleadoProduccion.Dias.ForEach(dia =>
            {
                if (dia != null)
                {
                    if (dia.GrupoExcepcion != null)
                    {
                        dia.R_Entrada = dia.Fecha + dia.GrupoExcepcion.Entrada;
                        dia.HorasMeta = dia.GrupoExcepcion.HorasMeta;
                    }
                    else
                    {
                        if (dia.GrupoHorario.EsCruce == false && dia.GrupoHorario.Descanso == false)
                        {
                            dia.R_Entrada = dia.Fecha + dia.GrupoHorario.Entrada;
                        }
                    }
                }
            });
            #endregion

            #region Procesar salidas
            empleadoProduccion.Dias.ForEach(dia =>
            {
                if (dia != null)
                {
                    if(DateTime.Parse("0001-01-01 00:00:00") != dia.R_Entrada)
                    {
                        var DiaNext = empleadoProduccion.Dias.Find(a => a.Fecha == dia.Fecha.AddDays(1));
                        if (DateTime.Parse("0001-01-01 00:00:00") != DiaNext.R_Entrada)
                        {
                            dia.R_Salida = DiaNext.R_Entrada.AddHours(-1);
                        }
                        else
                        {
                            dia.R_Salida = dia.R_Entrada.AddHours(23);
                        }
                    }
                    
                }
            });
            #endregion

            #region Log checks
            empleadoProduccion.Dias.ForEach(dia =>
            {
                if (dia != null)
                {
                    if (DateTime.Parse("0001-01-01 00:00:00") != dia.R_Salida && DateTime.Parse("0001-01-01 00:00:00") != dia.R_Entrada)
                    {
                        dia.Logs = darkManager.View_gps_ensambleSinFiltro.GetOpenquery(
                                    $"where tIdentification = '{Empleado.NumeroNomina}' " +
                                    $"AND dtEventReal >= '{dia.R_Entrada.ToString("yyyy-MM-dd HH:mm:ss")}' " +
                                    $"AND dtEventReal <= '{dia.R_Salida.ToString("yyyy-MM-dd HH:mm:ss")}' " +
                                    $"AND IdReader = 17",
                                    $"ORDER BY iEmployeeNum , dtEventReal");
                    }
                }
            });
            #endregion

            #region Procesar logs
            empleadoProduccion.Dias.ForEach(dia =>
            {
                if (dia != null)
                {
                    if (DateTime.Parse("0001-01-01 00:00:00") != dia.R_Salida && DateTime.Parse("0001-01-01 00:00:00") != dia.R_Entrada)
                    {
                        if (dia.GrupoExcepcion != null)
                        {
                            dia.HorasMeta = dia.GrupoExcepcion.HorasMeta;
                        }
                        else
                        {
                            dia.HorasMeta = dia.GrupoHorario.Horas;
                        }

                        if (dia.Logs.Count > 0)
                        {
                            dia.Entrada = dia.Logs.Where(a => a.IdReader == 17).ToList().Min(a => a.dtEventReal);
                            dia.Salida = dia.Logs.Where(a => a.IdReader == 17).ToList().Max(a => a.dtEventReal);

                            dia.HorasReal = dia.Salida != null && dia.Entrada != null ? Funciones.DifFechashoras((DateTime)dia.Salida, (DateTime)dia.Entrada) : 0;

                            double HorasMin = ((dia.HorasMeta / 100) * 95);

                            if (dia.HorasReal >= HorasMin && dia.HorasReal <= dia.HorasMeta)
                            {
                                empleadoProduccion.HorasTrabajadas += dia.HorasMeta;
                                dia.HorasAprobadas = dia.HorasMeta;
                            }
                            else
                            {
                                empleadoProduccion.HorasTrabajadas += dia.HorasReal;
                                dia.HorasAprobadas = dia.HorasReal;
                            }


                            empleadoProduccion.HorasMeta += dia.HorasMeta;


                        }
                        else
                        {
                            dia.HorasAprobadas = 0;
                            dia.HorasReal = 0;
                        }
                    }

                }
            });
            #endregion

            return empleadoProduccion;
        }

        private GrupoCambios GetUltimoCambio(DateTime Corte, int IdPersona_)
        {
            var UltimoCambio = darkManager.GrupoCambios.GetUnicSatatment($"select top 1 * from GrupoCambios where IdPersona = {IdPersona_} and Fecha <= '{Corte.ToString("yyyy-MM-dd")}' order by Fecha desc ");

            if (UltimoCambio != null)
            {
                UltimoCambio.GrupoName = UltimoCambio.IdGrupo == 86 ? "Gris" : UltimoCambio.IdGrupo == 87 ? "Rojo" : UltimoCambio.IdGrupo == 88 ? "Verde" : "Sin asginar";
            }
            return UltimoCambio;
        }
        /// <summary>
        /// Terminar controlador 
        /// </summary>
        public void Terminar()
        {
            darkManager.CloseConnection();
            darkManager.CatalogoOpcionesValores = null;
            darkManager.View_empleadoEnsamble = null;
            darkManager.GrupoHorario = null;
            darkManager.GrupoCambios = null;
        }
        #endregion
    }
}
