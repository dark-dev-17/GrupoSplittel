using GPSInformation.Models.Produccion;
using GPSInformation.Reportes;
using GPSInformation.Tools;
using GPSInformation.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace GPSInformation.Controllers
{
    public class ProduccionModV2Ctrl
    {
        #region Atributos
        private DarkManager darkManager;
        #endregion

        #region Constructores
        public ProduccionModV2Ctrl(DarkManager darkManager)
        {
            this.darkManager = darkManager;
            this.darkManager.OpenConnection();
            this.darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);
            this.darkManager.LoadObject(GpsManagerObjects.View_empleadoEnsamble);
            this.darkManager.LoadObject(GpsManagerObjects.GrupoHorario);
            this.darkManager.LoadObject(GpsManagerObjects.GrupoProduccionAsi);
        }
        #endregion
        #region Metodos
        public WeekEmpleadoProd EmpleadoProds(int NoSemana_, int year_)
        {
            darkManager.OpenConnectionAcces();
            darkManager.LoadObject(GpsControlAcceso.View_gps_ensambleSinFiltro);
            
            var Fecha = new DateTime(DateTime.Now.Year, 1, 1).AddDays((NoSemana_ * 7) - 1);
            WeekEmpleadoProd weekEmpleadoProd = new WeekEmpleadoProd
            {
                Empleados = new List<EmpleadoProd>(),
                Inicio = Funciones.GetFirtsDatWeek(Fecha),
                Fin = Funciones.GetLastDatWeek(Fecha),
            };

            var re_empleados = darkManager.View_empleadoEnsamble.Get();
            re_empleados.ForEach(emp => {
                var empleadoProd = Getreporte(emp, NoSemana_, year_);
                weekEmpleadoProd.Empleados.Add(empleadoProd);
            });
            darkManager.CloseConnectionAccess();

            weekEmpleadoProd.Empleados = weekEmpleadoProd.Empleados.OrderBy(a => a.Nombre).ToList();
            return weekEmpleadoProd;
        }
        /// <summary>
        /// Obtener reporte de acceos por persona
        /// </summary>
        /// <param name="IdPersona_"></param>
        /// <param name="NoSemana_"></param>
        /// <param name="ForceActiveChecs"></param>
        /// <returns></returns>
        public EmpleadoProd Getreporte(View_empleadoEnsamble emp, int NoSemana_, int year_, int IdPersona_ = 0)
        {
            if (IdPersona_ != 0)
            {
                darkManager.OpenConnectionAcces();
                darkManager.LoadObject(GpsControlAcceso.View_gps_ensambleSinFiltro);
                emp = darkManager.View_empleadoEnsamble.GetOpenquerys($"where IdPersona = {IdPersona_}");
            }

            if(emp != null)
            {
                var TurnoWeek = darkManager.GrupoProduccionAsi.GetOpenquerys($"where IdPersona = {emp.IdPersona} and NoSemana = {NoSemana_} and Year = {year_}");

                if(TurnoWeek != null)
                {
                    EmpleadoProd empleadoProd = new EmpleadoProd
                    {
                        NumeroNomina = emp.NumeroNomina,
                        Nombre = emp.NombreCompleto,
                        Dias = new List<DiaProd>(),
                        Inicio = TurnoWeek.Inicio,
                        Fin = TurnoWeek.Fin,
                        Puesto = emp.PuestoNombre,
                        Antiguedad = emp.Antiguedad,
                        IdPersona = emp.IdPersona,
                        GrupoName = TurnoWeek.IdGrupo == 86 ? "Gris" : TurnoWeek.IdGrupo == 87 ? "Rojo" : TurnoWeek.IdGrupo == 88 ? "Verde" : "Sin asginar"
                    };

                    DateTime Corte = TurnoWeek.Inicio;
                    while (Corte < TurnoWeek.Fin)
                    {
                        DiaProd diaProd = new DiaProd
                        {
                            Dia = Corte,
                            Logs = new List<string>()
                        };
                        var DiaHorario_re = darkManager.GrupoHorario.GetOpenquerys($"where IdGrupo = '{TurnoWeek.IdGrupo}' and dia = '{(int)Corte.DayOfWeek}'");
                        if (DiaHorario_re != null)
                        {
                            DateTime Entradatur = Corte + DiaHorario_re.Entrada;
                            DateTime Salidatur = Corte + DiaHorario_re.Salida;


                            diaProd.EsCruce = DiaHorario_re.EsCruce;
                            diaProd.EsNoche = DiaHorario_re.EsNoche;
                            diaProd.EsDescanso = DiaHorario_re.Descanso;

                            if (DiaHorario_re.EsCruce == false)
                            {
                                if (DiaHorario_re.EsNoche)
                                {
                                    Salidatur = Salidatur.AddDays(1);
                                }

                                var Logs_re = darkManager.View_gps_ensambleSinFiltro.GetOpenquery(
                                   $"where tIdentification = '{emp.NumeroNomina}' " +
                                   $"AND dtEventReal >= '{Entradatur.AddHours(-0.5).ToString("yyyy-MM-dd HH:mm:ss")}' " +
                                   $"AND dtEventReal <= '{Salidatur.AddHours(6).ToString("yyyy-MM-dd HH:mm:ss")}'",
                                   $"ORDER BY iEmployeeNum , dtEventReal");

                                if (Logs_re.Count > 0)
                                {
                                    diaProd.Entrada = Logs_re.Where(a => a.IdReader == 17).ToList().Min(a => a.dtEventReal);
                                    diaProd.Salida = Logs_re.Where(a => a.IdReader == 17).ToList().Max(a => a.dtEventReal);

                                    Logs_re.ForEach(a => diaProd.Logs.Add($"{a.dtEventReal.ToString("MM-dd HH:mm")} - {a.tDesc}"));
                                }
                            }

                            empleadoProd.HorasMeta += DiaHorario_re.Horas;

                            diaProd.Horas = diaProd.Salida != null && diaProd.Entrada != null ? Funciones.DifFechashoras((DateTime)diaProd.Salida, (DateTime)diaProd.Entrada) : 0;

                            diaProd.HorasMin = ((DiaHorario_re.Horas / 100) * 95);

                            if(diaProd.Horas >= diaProd.HorasMin && diaProd.Horas <= DiaHorario_re.Horas)
                            {
                                diaProd.Horas = DiaHorario_re.Horas;
                                empleadoProd.HorasTrabajadas += DiaHorario_re.Horas;
                            }
                            else
                            {
                                empleadoProd.HorasTrabajadas += diaProd.Horas;
                            }


                            diaProd.HorasMeta = DiaHorario_re.Horas;
                        }
                        empleadoProd.Dias.Add(diaProd);
                        Corte = Corte.AddDays(1);
                    }
                        return empleadoProd;
                }
                else
                {
                    var Fecha = new DateTime(DateTime.Now.Year, 1, 1).AddDays((NoSemana_ * 7) - 1);
                    EmpleadoProd empleadoProd = new EmpleadoProd
                    {
                        NumeroNomina = emp.NumeroNomina,
                        Nombre = emp.NombreCompleto,
                        Dias = new List<DiaProd>(),
                        Inicio = Funciones.GetFirtsDatWeek(Fecha),
                        Fin = Funciones.GetLastDatWeek(Fecha),
                        Puesto = emp.PuestoNombre,
                        Antiguedad = emp.Antiguedad,
                        IdPersona = emp.IdPersona,
                        GrupoName = "Sin turno"
                    };

                    return empleadoProd;
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Cambiar turnos a las personas de departamento de produccion
        /// </summary>
        /// <param name="Personas"></param>
        /// <param name="IdGrupo_"></param>
        /// <param name="NoSemana_"></param>
        public void CambiarGrupo(List<EmpleadoGr> Personas, int NoSemana_, int year)
        {
            darkManager.StartTransaction();
            try
            {
                if (Personas is null)
                {
                    throw new Exceptions.GpExceptions(string.Format("Error, no se encontraron personas para cambio de grupo"));
                }

                var Horarios = darkManager.GrupoHorario.Get();

                var Fecha = new DateTime(DateTime.Now.Year, 1, 1).AddDays((NoSemana_ * 7) - 1);

                Personas.ForEach(id =>
                {
                    if(id.IdGrupo != 0)
                    {
                        var empleadoProd = darkManager.GrupoProduccionAsi.GetOpenquerys($"where IdPersona = {id.IdPersona} and NoSemana = {NoSemana_} and Year = {year}");
                        if (empleadoProd is null)
                        {
                            GrupoProduccionAsi grupoProduccionAsi = new GrupoProduccionAsi
                            {
                                IdGrupo = id.IdGrupo,
                                IdPersona = id.IdPersona,
                                Comentarios = "",
                                NoSemana = NoSemana_,
                                HrsMeta = Horarios.Where(a => a.IdGrupo == id.IdGrupo).ToList().Sum(a => a.Horas),
                                HrsTrabaja = 0,
                                Inicio = Funciones.GetFirtsDatWeek(Fecha),
                                Fin = Funciones.GetLastDatWeek(Fecha),
                                Creado = DateTime.Now,
                                Modificado = DateTime.Now,
                                Year = year
                            };
                            darkManager.GrupoProduccionAsi.Element = grupoProduccionAsi;
                            if (!darkManager.GrupoProduccionAsi.Add())
                            {
                                throw new Exceptions.GpExceptions(string.Format("Error, no se logró guardar los cambios de turnos, por favor intenta de nuevo"));
                            }
                        }
                        else
                        {
                            empleadoProd.IdGrupo = id.IdGrupo;
                            empleadoProd.Modificado = DateTime.Now;
                            darkManager.GrupoProduccionAsi.Element = empleadoProd;
                            if (!darkManager.GrupoProduccionAsi.Update())
                            {
                                throw new Exceptions.GpExceptions(string.Format("Error, no se logró guardar los cambios de turnos, por favor intenta de nuevo"));
                            }
                        }
                    }
                    else
                    {

                    }
                });

                darkManager.Commit();
            }
            catch (Exceptions.GpExceptions)
            {
                darkManager.RolBack();
                throw;
            }
            

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
            darkManager.GrupoProduccionAsi = null;
        }
        #endregion
    }
}
