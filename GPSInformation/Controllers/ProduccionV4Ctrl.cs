using GPSInformation.Views;
using GPSInformation.Reportes.ProduccionV3;
using System;
using System.Collections.Generic;
using System.Text;
using GPSInformation.Models.Produccion;
using System.Linq;
using GPSInformation.Tools;

namespace GPSInformation.Controllers
{
    public class ProduccionV4Ctrl
    {
        #region Atributos
        public DarkManager darkManager;
        #endregion

        #region Constructores
        public ProduccionV4Ctrl(DarkManager darkManager)
        {
            this.darkManager = darkManager;
            this.darkManager.OpenConnection();
            this.darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);
            this.darkManager.LoadObject(GpsManagerObjects.View_empleadoEnsamble);
            this.darkManager.LoadObject(GpsManagerObjects.GrupoHorario);

            this.darkManager.LoadObject(GpsManagerObjects.GrupoCambios);
            this.darkManager.LoadObject(GpsManagerObjects.GrupoArreglo);
            this.darkManager.LoadObject(GpsManagerObjects.GrupoProdIncidencia);

            darkManager.OpenConnectionAcces();
            darkManager.LoadObject(GpsControlAcceso.View_gps_ensambleSinFiltro);

        }
        #endregion
        #region Metodos
        public void DeleteInci(int IdGrupoProdIncidencia, int IdPersona)
        {
            var res = darkManager.GrupoProdIncidencia.Get(IdGrupoProdIncidencia);
            if(res is null)
            {
                throw new Exceptions.GpExceptions("Error, no se encontro la incidencia");
            }
            darkManager.GrupoProdIncidencia.Element = res;
            if (!darkManager.GrupoProdIncidencia.Delete())
            {
                throw new Exceptions.GpExceptions("Error al guardar los cambios");
            }
        }
        public GrupoProdIncidencia DetailsInci(int IdGrupoProdIncidencia, int IdPersona)
        {
            var res = darkManager.GrupoProdIncidencia.Get(IdGrupoProdIncidencia);
            return res;
        }
        public void RegisterIncidencia(GrupoProdIncidencia GrupoProdIncidencia)
        {
            if(GrupoProdIncidencia is null)
                throw new Exceptions.GpExceptions("Datos incorrectos");
            darkManager.GrupoProdIncidencia.Element = GrupoProdIncidencia;
            if (GrupoProdIncidencia.IdGrupoProdIncidencia == 0)
            {
                darkManager.GrupoProdIncidencia.Element.Creado = DateTime.Now;
                darkManager.GrupoProdIncidencia.Element.Modificado = DateTime.Now;
                if (!darkManager.GrupoProdIncidencia.Add())
                {
                    throw new Exceptions.GpExceptions("Error al guardar los cambios");
                }
            }
            else
            {
                darkManager.GrupoProdIncidencia.Element.Modificado = DateTime.Now;
                if (!darkManager.GrupoProdIncidencia.Update())
                {
                    throw new Exceptions.GpExceptions("Error al guardar los cambios");
                }
            }
        }
        public void DeleteArreglo(int IdEvent, int IdPersona)
        {
            darkManager.GrupoArreglo.Element = darkManager.GrupoArreglo.GetOpenquerys($"where IdPersona = {IdPersona} and IdGrupoArreglo = {IdEvent}");
            if(darkManager.GrupoArreglo.Element is null)
            {
                throw new Exceptions.GpExceptions("No se encontro el evento de tipo arreglo");
            }

            if (!darkManager.GrupoArreglo.Delete())
            {
                throw new Exceptions.GpExceptions("Error al guardar los cambios");
            }
        }
        public void AddArreglo(int IdEvent, int IdPersona, DateTime Fecha, string Comentarios)
        {
            darkManager.GrupoArreglo.Element = new GrupoArreglo
            {
                IdEvent = IdEvent,
                IdPersona = IdPersona,
                FechaHora = Fecha,
                Comentarios = Comentarios,
                EsIgnorado = true,
                Creado = DateTime.Now,
                Modificado = DateTime.Now,
            };

            if (!darkManager.GrupoArreglo.Add())
            {
                throw new Exceptions.GpExceptions("Error al guardar los cambios");
            }
        }
        public ReporteProd ProcesarEmpleados(DateTime Inicio_)
        {
            ReporteProd reporteProd = new ReporteProd { 
                Inicio = Inicio_,
                Fin = Inicio_.AddDays(6),
                EmpleadoProds = new List<EmpleadoProd>()
            };
            darkManager.View_empleadoEnsamble.Get().ForEach(a => {
                reporteProd.EmpleadoProds.Add(ProcesarEmpleado(a, 0, Inicio_));
            });
            return reporteProd;
        }
        public EmpleadoProd ProcesarEmpleado(View_empleadoEnsamble Empleado, int IdPersona, DateTime Inicio_)
        {
            if(IdPersona != 0)
            {
                Empleado = darkManager.View_empleadoEnsamble.Get(IdPersona);
            }
            EmpleadoProd EmpleadoProd = new EmpleadoProd { 
                IdPersona = Empleado.IdPersona,
                NombreCompleto = Empleado.NombreCompleto,
                NumeroNomina = Empleado.NumeroNomina,
                PuestoNombre = Empleado.PuestoNombre,
                Antiguedad = Empleado.Antiguedad,
                Incio = Inicio_,
                Fin = Inicio_.AddDays(6),
                Accessos = new List<AccessLog>(),
                JornadaGrupos = new List<JornadaGrupo>()
            };

            #region Procesar logs
            var Logs = darkManager.View_gps_ensambleSinFiltro.GetOpenquery(
                                    $"where tIdentification = '{Empleado.NumeroNomina}' " +
                                    $"AND dtEventReal >= '{EmpleadoProd.Incio.ToString("yyyy-MM-dd 05:50:00")}' " +
                                    $"AND dtEventReal <= '{EmpleadoProd.Fin.AddDays(1).ToString("yyyy-MM-dd 05:45:00")}' " +
                                    $"AND IdReader = 17",
                                    $"ORDER BY iEmployeeNum , dtEventReal");

            Logs.ForEach(log => {
                //validar ignorar log
                var logIgnore = darkManager.GrupoArreglo.GetOpenquerys($"where IdEvent = '{log.IdAutoEvents}' and IdPersona = { Empleado.IdPersona}");

                EmpleadoProd.Accessos.Add(new AccessLog
                {
                    IdEventChec = log.IdAutoEvents,
                    IdGrupoArreglo = logIgnore != null ? logIgnore.IdGrupoArreglo : 0,
                    Descripcion = logIgnore != null ? logIgnore.Comentarios : log.tDesc,
                    Fecha = log.dtEventReal,
                    Activo = logIgnore is null ? true : false,
                    Forzado = false,
                    IdAccessLog = 0
                });
            });
            #endregion

            #region procesar arreglos a logs
            var arreglos = darkManager.GrupoArreglo.GetOpenquery($"where " +
               $"IdEvent is null and IdPersona = { Empleado.IdPersona} " +
               $"AND FechaHora >= '{EmpleadoProd.Incio.ToString("yyyy-MM-dd 05:50:00")}' " +
               $"AND FechaHora <= '{EmpleadoProd.Fin.AddDays(1).ToString("yyyy-MM-dd 05:45:00")}' ",
               $"Order by FechaHora");

            arreglos.ForEach(arr => {
                EmpleadoProd.Accessos.Add(new AccessLog
                {
                    IdEventChec = 0,
                    Descripcion = arr.Comentarios,
                    Fecha = arr.FechaHora,
                    Activo = true,
                    Forzado = true,
                    IdAccessLog = arr.IdGrupoArreglo
                });
            });
            #endregion

            #region Procesar dias por grupo asginado
            DateTime Corte = EmpleadoProd.Incio;
            while (Corte <= EmpleadoProd.Fin)
            {
                var UltimoCambio = GetUltimoCambio(Corte, Empleado.IdPersona);

                if (UltimoCambio != null)
                {
                    // obtener dia de acuerdo al grupo
                    var DiaHorario_re = darkManager.GrupoHorario.GetOpenquerys($"where IdGrupo = '{UltimoCambio.IdGrupo}' and dia = '{(int)Corte.DayOfWeek}'");
                    var diaLog = new JornadaGrupo
                    {
                        IdGrupo = DiaHorario_re.IdGrupo,
                        HorasMeta = DiaHorario_re.Horas,
                        TipoJornada = DiaHorario_re.TipoDia,
                        Fecha = Corte + DiaHorario_re.Entrada,
                    };

                    diaLog.Salida = diaLog.Fecha.AddHours(diaLog.HorasMeta);
                    EmpleadoProd.JornadaGrupos.Add(diaLog);
                }

                Corte = Corte.AddDays(1);
            }
            #endregion

            #region Validando entradas
            EmpleadoProd.Accessos = EmpleadoProd.Accessos.OrderBy(a => a.Fecha).ToList();

            int Pos = 1;

            EmpleadoProd.Accessos.ForEach(a => {
                if (a.Activo)
                {
                    a.Position = Pos;
                    Pos++;
                }
            });

            #endregion

            #region procesar horas meta
            DateTime Entrada = DateTime.Parse("0001-01-01 00:00:00");

            if ((EmpleadoProd.Accessos.Count(a => a.Activo) % 2 == 0))
            {
                EmpleadoProd.Accessos.ForEach(a => {
                    if (a.Activo)
                    {
                        if (a.TipoAcceso == TipoAcceso.Entrada)
                        {
                            if (Entrada < a.Fecha)
                            {
                                Entrada = a.Fecha;
                            }
                        }
                        else
                        {
                            EmpleadoProd.HorasReal += Funciones.DifFechashoras((DateTime)a.Fecha, (DateTime)Entrada);
                        }
                    }
                });
            }

            EmpleadoProd.JornadaGrupos.ForEach(a => {
                EmpleadoProd.HorasMeta += a.HorasMeta;
            });
            #endregion

            #region obtener incidencias
            var Incidencias = darkManager.GrupoProdIncidencia.GetOpenquery($"where " +
               $"IdPersona = { Empleado.IdPersona}  and TipoIncidecia = 'Per'" +
               $"AND FechaPermiso >= '{EmpleadoProd.Incio.ToString("yyyy-MM-dd 05:50:00")}' " +
               $"AND FechaPermiso <= '{EmpleadoProd.Fin.AddDays(1).ToString("yyyy-MM-dd 05:45:00")}' ",
               $"Order by FechaPermiso");

            EmpleadoProd.GrupoProdIncidencias = Incidencias;

            var vacaciones = darkManager.GrupoProdIncidencia.GetOpenquery($"where " +
               $"IdPersona = { Empleado.IdPersona}  and TipoIncidecia = 'Vac'" +
               $"AND FechaSalVac >= '{EmpleadoProd.Incio.ToString("yyyy-MM-dd 05:50:00")}' " +
               $"AND FechaSalVac <= '{EmpleadoProd.Fin.AddDays(1).ToString("yyyy-MM-dd 05:45:00")}' " +
               $"AND FechaRegVac >= '{EmpleadoProd.Incio.ToString("yyyy-MM-dd 05:50:00")}' " +
               $"AND FechaRegVac <= '{EmpleadoProd.Fin.AddDays(1).ToString("yyyy-MM-dd 05:45:00")}' ",
               $"Order by FechaRegVac");

            vacaciones.ForEach(eve => {
                EmpleadoProd.GrupoProdIncidencias.Add(eve);
            });


            EmpleadoProd.GrupoProdIncidencias.ForEach(a=> { 
                if(a.TipoIncidecia == "Per")
                {
                    var opc = darkManager.CatalogoOpcionesValores.Get(a.TipoPermiso);
                    if(opc != null)
                    {
                        a.NameTipoPermiso = opc.Descripcion;
                    }
                }

                EmpleadoProd.HorasAprobadas += a.HorasJustific;
            });

            #endregion

            return EmpleadoProd;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Corte"></param>
        /// <param name="IdPersona_"></param>
        /// <returns></returns>
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
            darkManager.CloseConnectionAccess();
            darkManager.CatalogoOpcionesValores = null;
            darkManager.View_empleadoEnsamble = null;
            darkManager.GrupoHorario = null;
            darkManager.View_gps_ensambleSinFiltro = null;
            darkManager.GrupoCambios = null;
            darkManager.GrupoArreglo = null;
        }
        #endregion
    }
}
