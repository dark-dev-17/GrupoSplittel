using GPSInformation.Models;
using GPSInformation.Models.Produccion;
using GPSInformation.Reportes;
using GPSInformation.Tools;
using GPSInformation.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPSInformation.Controllers
{
    public class ProduccionModCtrl
    {
        #region Atributos
        private DarkManager darkManager;
        public List<Registro> Nomenclatura = new List<Registro>();
        #endregion

        #region Constructores
        public ProduccionModCtrl(DarkManager darkManager)
        {
            this.darkManager = darkManager;
            this.darkManager.OpenConnection();
            this.darkManager.LoadObject(GpsManagerObjects.View_empleadoEnsamble);
            this.darkManager.LoadObject(GpsManagerObjects.EnsablesTurnos);
            this.darkManager.LoadObject(GpsManagerObjects.TurnoEmpleado);
            this.darkManager.LoadObject(GpsManagerObjects.TurnosProduccion);
            this.darkManager.LoadObject(GpsManagerObjects.IncidenciaPermiso);
            this.darkManager.LoadObject(GpsManagerObjects.IncidenciaVacacion);
            this.darkManager.LoadObject(GpsManagerObjects.FaltaJustificacion);
            this.darkManager.LoadObject(GpsManagerObjects.DiaFeriado);

            this.darkManager.LoadObject(GpsManagerObjects.GrupoHorario);
            this.darkManager.LoadObject(GpsManagerObjects.GrupoProduccion);
            this.darkManager.LoadObject(GpsManagerObjects.CatalogoOpcionesValores);

            Nomenclatura = Funciones.GetRegistrosInc();
        }
        #endregion
        #region Metodos produccion admin
        public List<PrenominaDias> GetPreniminaLists(Prenomina_RepProd Prenomina_RepProd, List<View_empleadoEnsamble> view_Empleados)
        {
            List<PrenominaDias> prenominaDias = new List<PrenominaDias>();
            darkManager.OpenConnectionAcces();
            view_Empleados.ForEach( emp => {
                PrenominaDias preDia = new PrenominaDias
                {
                    IdPersona = emp.IdPersona,
                    NumeroNomina = emp.NumeroNomina,
                    Dias = new List<PreniminaList>(),
                };
                //agregar dias
                DateTime InitialDate = Prenomina_RepProd.Inicio;

                while (InitialDate <= Prenomina_RepProd.Fin)
                {
                    var preniminaList = new PreniminaList
                    {
                        Fecha = InitialDate,
                        Incidencias = new List<Registro>()
                    };

                    var emplTur_re = darkManager.TurnoEmpleado.Get("Fecha", InitialDate.ToString("yyyy-MM-dd"), "IdPersona", "" + emp.IdPersona);

                    if(emplTur_re == null)
                    {
                        preniminaList.Incidencias.Add(Nomenclatura.Find(a => a.Clave =="DES"));
                    }
                    else
                    {
                        TimeSpan diferencia;
                        if (emplTur_re.IdTurnosProduccion == 1)
                        {
                            //primer turno
                            //obtener horas trabajadas de acuerdo al turno
                            diferencia = GetDiference(InitialDate.Add(emplTur_re.Inicio), InitialDate.Add(emplTur_re.Fin), emp.NumeroNomina);
                        }
                        if (emplTur_re.IdTurnosProduccion == 2)
                        {
                            //segundo turno
                            //obtener horas trabajadas de acuerdo al turno
                            diferencia = GetDiference(InitialDate.Add(emplTur_re.Inicio), InitialDate.AddDays(1).Add(emplTur_re.Fin), emp.NumeroNomina);
                        }
                        else
                        {

                        }

                        if (diferencia.TotalHours > 0 && diferencia.TotalHours < 8)
                        {
                            //buscar permisos
                            //var Tipo = Nomenclatura.Find(a => a.Clave == "PCG");
                            //Tipo.Title = string.Format("{0:00}", diferencia.TotalHours);
                            //preniminaList.Incidencias.Add(Tipo);
                            var Permiso_re = GetPermiso(emp.IdPersona, InitialDate);
                            if (emplTur_re.IdTurnosProduccion == 2 && Permiso_re.Clave == "SNJ")
                            {
                                Permiso_re = GetPermiso(emp.IdPersona, InitialDate.AddDays(1));
                            }
                            preniminaList.Incidencias.Add(Permiso_re);

                        }
                        else if (diferencia.TotalHours <= 0)
                        {
                            //buscar jutificaciones, vacaciones o dias festivos durante la fecha seleccionada
                            //var Tipo = Nomenclatura.Find(a => a.Clave == "VAC");
                            //Tipo.Title = string.Format("{0:00}", diferencia.TotalHours);
                            //preniminaList.Incidencias.Add(Tipo);
                            if (TieneVacaciones(emp.IdPersona, InitialDate))
                            {
                                preniminaList.Incidencias.Add(Nomenclatura.Find(a => a.Clave.Trim() == "VAC"));
                            }
                            else
                            {
                                var Feriado = darkManager.DiaFeriado.GetByColumn("" + InitialDate.ToString("yyyy/MM/dd"), "Fecha");
                                if (Feriado is null)
                                {
                                    var Justi = darkManager.FaltaJustificacion.Get("IdPersona", "" + emp.IdPersona, "Fecha", InitialDate.ToString("yyyy/MM/dd"));
                                    if (Justi is null)
                                    {
                                        preniminaList.Incidencias.Add(Nomenclatura.Find(a => a.Clave.Trim() == "FAL"));
                                    }
                                    else
                                    {
                                        var Res = Nomenclatura.Find(a => a.Clave.Trim() == "INJ");
                                        Res.Title = string.Format("Incidencia justificada, comentarios: {0}", Justi.Comentarios);
                                        preniminaList.Incidencias.Add(Res);
                                    }
                                }
                                else
                                {
                                    preniminaList.Incidencias.Add(Nomenclatura.Find(a => a.Clave.Trim() == "FES"));
                                }

                            }
                        }
                        else
                        {
                            //validacion para turnos aceptados
                            ////buscar jutificaciones, vacaciones o dias festivos durante la fecha seleccionada
                            //var Tipo = Nomenclatura.Find(a => a.Clave == "FAL");
                            //Tipo.Title = string.Format("{0:00}", diferencia.TotalHours);
                            //preniminaList.Incidencias.Add(Tipo);
                        }
                    }

                    preDia.Dias.Add(preniminaList);
                    InitialDate = InitialDate.AddDays(1);
                }
                prenominaDias.Add(preDia);
            });
            darkManager.CloseConnectionAccess();
            return prenominaDias;
        }
        private Registro GetPermiso(int IdPersona, DateTime Fecha)
        {
            var Permiso_re = darkManager.IncidenciaPermiso.Get("" + IdPersona, "IdPersona").Find(per => per.Fecha == Fecha);
            if (Permiso_re is null)
            {
                var Justi = darkManager.FaltaJustificacion.Get("IdPersona", "" + IdPersona, "Fecha", Fecha.ToString("yyyy/MM/dd"));
                if (Justi is null)
                {
                    return Nomenclatura.Find(a => a.Clave.Trim() == "SNJ");
                }
                else
                {
                    var Res = Nomenclatura.Find(a => a.Clave.Trim() == "INJ");
                    Res.Title = string.Format("Incidencia justificada, comentarios: {0}", Justi.Comentarios);
                    return Res;
                }
            }
            if (Permiso_re.IdAsunto != 36)
            {
                // Laboral: capacitación
                if (Permiso_re.IdAsunto == 37)
                {
                    return Nomenclatura.Find(a => a.Clave.Trim() == "SAL");
                }
                // Laboral: reunión de trabajo
                else if (Permiso_re.IdAsunto == 38)
                {
                    return Nomenclatura.Find(a => a.Clave.Trim() == "SAL");
                }
                // laboral: visita al cliente
                else if (Permiso_re.IdAsunto == 39)
                {
                    return Nomenclatura.Find(a => a.Clave.Trim() == "VIS");
                }
                else
                {
                    throw new GPSInformation.Exceptions.GpExceptions("El asunto del permiso no es valido");
                }
            }
            else
            {
                // Permiso con goce de sueldo
                if (Permiso_re.IdPagoPermiso == 40)
                {
                    return Nomenclatura.Find(a => a.Clave.Trim() == "PCG");
                }
                // Permiso sin goce de sueldo
                else if (Permiso_re.IdPagoPermiso == 41)
                {
                    return Nomenclatura.Find(a => a.Clave.Trim() == "PSG");
                }
                // permiso tiempo por tiempo
                else if (Permiso_re.IdPagoPermiso == 42)
                {
                    return Nomenclatura.Find(a => a.Clave.Trim() == "TXT");
                }
                // salario emocional: cumpleaños
                else if (Permiso_re.IdPagoPermiso == 43)
                {
                    return Nomenclatura.Find(a => a.Clave.Trim() == "DXC");
                }
                // salario emocional: medio dia libre
                else if (Permiso_re.IdPagoPermiso == 44)
                {
                    return Nomenclatura.Find(a => a.Clave.Trim() == "S.E.S");
                }
                else
                {
                    throw new GPSInformation.Exceptions.GpExceptions("El pago del permiso no es valido");
                }
            }
        }
        private bool TieneVacaciones(int IdPersona, DateTime Fecha)
        {
            bool Tiene = false;
            var vacacion_re = darkManager.IncidenciaVacacion.Get("" + IdPersona, "IdPersona").Find(vac => vac.Inicio <= Fecha && Fecha <= vac.Fin);
            if (vacacion_re != null)
            {
                if (vacacion_re.Estatus == 3)
                {
                    Tiene = true;
                }
            }
            else
            {
                Tiene = false;
            }
            return Tiene;
        }
        private TimeSpan GetDiference(DateTime Inicio, DateTime Fin, string NumeroNomina)
        {
            string statement = string.Format("" +
                "SELECT " +
                    "( " +
                        "SELECT MAX(EVN.dtEventReal) FROM tblEvents AS EVN WHERE EMP.iEmployeeNum = EVN.IdEmpNum AND EVN.dtEventReal >= '{0}' AND EVN.dtEventReal <= '{1} '" +
                    ") AS MAX_DATE, " +
                    " (" +
                        "SELECT MIN(EVN.dtEventReal) FROM tblEvents AS EVN WHERE EMP.iEmployeeNum = EVN.IdEmpNum AND EVN.dtEventReal >= '{0}' AND EVN.dtEventReal <= '{1} '" +
                    ") AS MIN_DATE " +
                "FROM[dbo].[tblEmployees] AS EMP " +
                "WHERE EMP.tIdentification = '{2}'", Inicio.ToString("yyyy/MM/dd HH:mm:ss"), Fin.ToString("yyyy/MM/dd HH:mm:ss"), NumeroNomina);

            System.Data.SqlClient.SqlDataReader Data = darkManager.ExecuteStatementAccess(statement);

            DateTime MAX_DATE = DateTime.Now;
            DateTime MIN_DATE = DateTime.Now;
            if (Data.HasRows)
            {
                while (Data.Read())
                {
                    MAX_DATE = Data.GetValue(Data.GetOrdinal("MAX_DATE")) is System.DBNull ? DateTime.Now : Data.GetDateTime(Data.GetOrdinal("MAX_DATE"));
                    MIN_DATE = Data.GetValue(Data.GetOrdinal("MIN_DATE")) is System.DBNull ? DateTime.Now : Data.GetDateTime(Data.GetOrdinal("MIN_DATE"));
                }
                TimeSpan antiguedad = MAX_DATE - MIN_DATE;
                Data.Close();
                return antiguedad;
            }
            else
            {
                TimeSpan antiguedad = MAX_DATE - MIN_DATE;
                Data.Close();
                return antiguedad;
            }

        }
        public void CambioTurno(int IdPersona, int IdTurno, DateTime Inicio, DateTime Fin)
        {
            darkManager.StartTransaction();
            try
            {
                if(IdPersona == 0)
                    throw new Exceptions.GpExceptions(string.Format("Por favor selecciona un empleado"));
                if (IdTurno == 0)
                    throw new Exceptions.GpExceptions(string.Format("Por favor selecciona un turno"));


                var Turnos_re = darkManager.TurnosProduccion.Get(IdTurno);


                DateTime InitialDate = Inicio;

                while (InitialDate <= Fin)
                {
                    var emplTur_re = darkManager.TurnoEmpleado.Get("Fecha", InitialDate.ToString("yyyy-MM-dd"), "IdPersona", "" + IdPersona);
                    if(emplTur_re is null)
                    {
                        TurnoEmpleado turnoEmpleado = new TurnoEmpleado
                        {
                            IdPersona = IdPersona,
                            IdTurnosProduccion = IdTurno,
                            Fecha = InitialDate,
                            Inicio = Turnos_re.Entrada,
                            Fin = Turnos_re.Salida,
                            Activo = Inicio >= DateTime.Now ? true : false
                        };
                        darkManager.TurnoEmpleado.Element = turnoEmpleado;
                        if (!darkManager.TurnoEmpleado.Add())
                        {
                            throw new Exceptions.GpExceptions(string.Format("Error al registrar el cambio de turno"));
                        }
                    }
                    else
                    {
                        TurnoEmpleado turnoEmpleado = new TurnoEmpleado
                        {
                            IdPersona = IdPersona,
                            IdTurnosProduccion = IdTurno,
                            Fecha = InitialDate,
                            IdTurnoEmpleado = emplTur_re.IdTurnoEmpleado,
                            Inicio = Turnos_re.Entrada,
                            Fin = Turnos_re.Salida,
                            Activo = Inicio >= DateTime.Now ? true : false
                        };
                        darkManager.TurnoEmpleado.Element = turnoEmpleado;
                        if (!darkManager.TurnoEmpleado.Update())
                        {
                            throw new Exceptions.GpExceptions(string.Format("Error al actualizar el cambio de turno"));
                        }
                    }
                    InitialDate = InitialDate.AddDays(1);
                }

                darkManager.Commit();
            }
            catch (Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                throw ex;
            }
        }
        public List<TurnosProduccion> GetTurnos()
        {
            return darkManager.TurnosProduccion.Get();
        }
        public Prenomina_RepProd GetPrenomina()
        {
            var respuesta = new Prenomina_RepProd()
            {
                Inicio = DateTime.Now,
                Fin = DateTime.Now,
                Turnos = new List<Turno>()
            };
            darkManager.TurnosProduccion.Get().ForEach(turno => {
                respuesta.Turnos.Add(new Turno { IdTurnosProduccion = turno.IdTurnosProduccion, Nombre = turno.Descripcion, Selected = false });
            });
            return respuesta;
        }
        public TurnoEmpleadoForm GetTurnoEmpleado(int IdPersona)
        {
            //var cambios_re = darkManager.TurnoEmpleado.Get("" + IdPersona, "IdPersona").OrderByDescending(a => a.Fecha).ToList();
            TurnoEmpleadoForm turnoEmpleado = new TurnoEmpleadoForm();
            turnoEmpleado.IdPersona = IdPersona;
            turnoEmpleado.FechaInicio = DateTime.Now;
            turnoEmpleado.FechaFin = DateTime.Now;
            turnoEmpleado.IdTurnosProduccion = 0;
            return turnoEmpleado;
        }
        public List<View_empleadoEnsamble> GetEmpleadosProd()
        {
            return darkManager.View_empleadoEnsamble.Get().OrderBy(a => a.NombreCompleto).ToList();
        }
        public List<EnsablesTurnos> GetEmpleadoTurnos(int IdPersona)
        {
            return darkManager.EnsablesTurnos.Get("" + IdPersona, "IdPersona");
        }
        #endregion

        #region Produccion horas trabajadas v1
        public List<ColaboradorEnsamble> GetReportEnsamble()
        {
            darkManager.OpenConnectionAcces();
            darkManager.LoadObject(GpsControlAcceso.View_gps_ensambleSinFiltro);
            List<ColaboradorEnsamble> reporte = new List<ColaboradorEnsamble>();
            var re_empleados = darkManager.View_empleadoEnsamble.Get();
            re_empleados.ForEach(emp => {
                var op = EvaluarGrupos(DateTime.Parse("2020-12-02 00:00:00"), emp.NumeroNomina);
                ColaboradorEnsamble colaboradorEnsamble = new ColaboradorEnsamble
                {
                    //Dias = ProcessDate(DateTime.Parse("2020-12-02 00:00:00"), emp.NumeroNomina),
                    NombreCompleto = emp.NombreCompleto,
                    Nomina = emp.NumeroNomina,
                    DefTurno = op,
                    SelectGroup = Total(op)[0].Indice
                };
                reporte.Add(colaboradorEnsamble);
            });
            return reporte;
        }
        private List<GrupoOrder> Total(List<GrupoDef> grupoDefs)
        {
            var lista = new List<GrupoOrder> {
                new GrupoOrder { Indice = 0, total = 0},
                new GrupoOrder { Indice = 1, total = 0},
                new GrupoOrder { Indice = 2, total = 0},
            };
            grupoDefs[0].DiasHorarios.ForEach(a => lista[0].total += a.Accesos != null ? a.Accesos.Count : 0 );
            grupoDefs[1].DiasHorarios.ForEach(a => lista[1].total += a.Accesos != null ? a.Accesos.Count : 0);
            grupoDefs[2].DiasHorarios.ForEach(a => lista[2].total += a.Accesos != null ? a.Accesos.Count : 0);
            return lista.OrderByDescending( a => a.total).ToList();
        }
        private List<GrupoDef> EvaluarGrupos(DateTime Fecha, string NumeroNomina)
        {
            #region definicion de horarios por dia y grupo
            var TurnoGris = new GrupoDef
            {
                Tipo = GrupoTurno.Gris,
                DiasHorarios = new List<TurnoDia> {
                    new TurnoDia { Dia = DayOfWeek.Monday, EsCambio = false,  EsDescanso = true, IsCrossDay = false, Entrada = new TimeSpan(18,0,0),Salida = new TimeSpan(6,0,0) },
                    new TurnoDia { Dia = DayOfWeek.Tuesday, EsCambio = false,  EsDescanso = false, IsCrossDay = true, Entrada = new TimeSpan(18,0,0),Salida = new TimeSpan(6,0,0) },
                    new TurnoDia { Dia = DayOfWeek.Wednesday, EsCambio = false,  EsDescanso = false, IsCrossDay = true, Entrada = new TimeSpan(18,0,0),Salida = new TimeSpan(6,0,0) },
                    new TurnoDia { Dia = DayOfWeek.Thursday, EsCambio = false,  EsDescanso = false, IsCrossDay = true, Entrada = new TimeSpan(18,0,0),Salida = new TimeSpan(6,0,0) },
                    new TurnoDia { Dia = DayOfWeek.Friday, EsCambio = true,  EsDescanso = false, IsCrossDay = false, Entrada = new TimeSpan(18,0,0),Salida = new TimeSpan(6,0,0) },
                    new TurnoDia { Dia = DayOfWeek.Saturday, EsCambio = false,  EsDescanso = false, IsCrossDay = false, Entrada = new TimeSpan(15,0,0),Salida = new TimeSpan(21,0,0) },
                    new TurnoDia { Dia = DayOfWeek.Sunday, EsCambio = false,  EsDescanso = true, IsCrossDay = false, Entrada = new TimeSpan(18,0,0),Salida = new TimeSpan(6,0,0) },
                }
            };
            var TurnoRojo = new GrupoDef
            {
                Tipo = GrupoTurno.Rojo,
                DiasHorarios = new List<TurnoDia> {
                    new TurnoDia { Dia = DayOfWeek.Monday, EsCambio = false,  EsDescanso = false, IsCrossDay = true, Entrada = new TimeSpan(18,0,0),Salida = new TimeSpan(6,0,0) },
                    new TurnoDia { Dia = DayOfWeek.Tuesday, EsCambio = true,  EsDescanso = false, IsCrossDay = false, Entrada = new TimeSpan(),Salida = new TimeSpan() },
                    new TurnoDia { Dia = DayOfWeek.Wednesday, EsCambio = false,  EsDescanso = false, IsCrossDay = false, Entrada = new TimeSpan(6,0,0),Salida = new TimeSpan(18,0,0) },
                    new TurnoDia { Dia = DayOfWeek.Thursday, EsCambio = false,  EsDescanso = false, IsCrossDay = true, Entrada = new TimeSpan(18,0,0),Salida = new TimeSpan(6,0,0) },
                    new TurnoDia { Dia = DayOfWeek.Friday, EsCambio = true,  EsDescanso = false, IsCrossDay = false, Entrada = new TimeSpan(),Salida = new TimeSpan() },
                    new TurnoDia { Dia = DayOfWeek.Saturday, EsCambio = false,  EsDescanso = false, IsCrossDay = false, Entrada = new TimeSpan(6,0,0),Salida = new TimeSpan(15,0,0) },
                    new TurnoDia { Dia = DayOfWeek.Sunday, EsCambio = false,  EsDescanso = true, IsCrossDay = false, Entrada = new TimeSpan(),Salida = new TimeSpan() },
                }
            };
            var TurnoVerde = new GrupoDef
            {
                Tipo = GrupoTurno.Verde,
                DiasHorarios = new List<TurnoDia> {
                    new TurnoDia { Dia = DayOfWeek.Monday, EsCambio = false,  EsDescanso = false, IsCrossDay = false, Entrada = new TimeSpan(6,0,0),Salida = new TimeSpan(18,0,0) },
                    new TurnoDia { Dia = DayOfWeek.Tuesday, EsCambio = false,  EsDescanso = false, IsCrossDay = false, Entrada = new TimeSpan(6,0,0),Salida = new TimeSpan(18,0,0) },
                    new TurnoDia { Dia = DayOfWeek.Wednesday, EsCambio = false,  EsDescanso = true, IsCrossDay = false, Entrada = new TimeSpan(6,0,0),Salida = new TimeSpan(18,0,0) },
                    new TurnoDia { Dia = DayOfWeek.Thursday, EsCambio = false,  EsDescanso = false, IsCrossDay = false, Entrada = new TimeSpan(6,0,0),Salida = new TimeSpan(18,0,0) },
                    new TurnoDia { Dia = DayOfWeek.Friday, EsCambio = false,  EsDescanso = false, IsCrossDay = false, Entrada = new TimeSpan(6,0,0),Salida = new TimeSpan(18,0,0) },
                    new TurnoDia { Dia = DayOfWeek.Saturday, EsCambio = false,  EsDescanso = true, IsCrossDay = false, Entrada = new TimeSpan(6,0,0),Salida = new TimeSpan(18,0,0) },
                    new TurnoDia { Dia = DayOfWeek.Sunday, EsCambio = false,  EsDescanso = true, IsCrossDay = false, Entrada = new TimeSpan(6,0,0),Salida = new TimeSpan(18,0,0) },
                }
            };
            #endregion

            #region Carga de turnos y horarios
            List<GrupoDef> grupoDefs = new List<GrupoDef>() {
                new GrupoDef{ 
                    Tipo = GrupoTurno.Gris,
                    DiasHorarios = new List<TurnoDia>()
                },
                new GrupoDef{
                    Tipo = GrupoTurno.Rojo,
                    DiasHorarios = new List<TurnoDia>()
                },
                new GrupoDef{
                    Tipo = GrupoTurno.Verde,
                    DiasHorarios = new List<TurnoDia>()
                },
            };

            DateTime Inicio = Fecha;
            DateTime Fin = DateTime.Parse(Fecha.AddDays(6).ToString("yyyy-MM-dd 23:59:59"));
            while (Inicio <= Fin)
            {
                TurnoDia grisday = TurnoGris.DiasHorarios.Find(a => a.Dia == Inicio.DayOfWeek);
                if (grisday != null) 
                    grisday.FechaDia = Inicio;
                grupoDefs[0].DiasHorarios.Add(grisday);

                TurnoDia rojoday = TurnoRojo.DiasHorarios.Find(a => a.Dia == Inicio.DayOfWeek);
                if (rojoday != null) 
                        rojoday.FechaDia = Inicio;
                grupoDefs[1].DiasHorarios.Add(rojoday);

                TurnoDia verdeday = TurnoVerde.DiasHorarios.Find(a => a.Dia == Inicio.DayOfWeek);
                if (verdeday != null)
                    verdeday.FechaDia = Inicio;
                grupoDefs[2].DiasHorarios.Add(verdeday);

                Inicio = Inicio.AddDays(1);
                grisday = null;
                rojoday = null;
                verdeday = null;
            }

            TurnoGris = null;
            TurnoRojo = null;
            TurnoVerde = null;
            #endregion

            #region Extraccion de accesos por turno dia y horarios
            grupoDefs.ForEach(grp => {
                List<View_gps_ensambleSinFiltro> Logs;
                grp.DiasHorarios.ForEach(shedule => {

                    if(shedule.EsCambio == false)
                    {
                        if (shedule.IsCrossDay)
                        {
                            DateTime Entrada = shedule.FechaDia + shedule.Entrada;
                            DateTime Salida = shedule.FechaDia.AddDays(1) + shedule.Salida;

                            Logs = darkManager.View_gps_ensambleSinFiltro.GetOpenquery(
                               $"where tIdentification = '{NumeroNomina}' " +
                               $"AND dtEventReal >= '{Entrada.AddHours(-0.5).ToString("yyyy-MM-dd HH:mm:ss")}' " +
                               $"AND dtEventReal <= '{Salida.AddHours(2).ToString("yyyy-MM-dd HH:mm:ss")}'",
                               $"ORDER BY iEmployeeNum , dtEventReal");
                        }
                        else
                        {
                            DateTime Entrada = shedule.FechaDia + shedule.Entrada;
                            DateTime Salida = shedule.FechaDia + shedule.Salida;

                            Logs = darkManager.View_gps_ensambleSinFiltro.GetOpenquery(
                               $"where tIdentification = '{NumeroNomina}' " +
                               $"AND dtEventReal >= '{Entrada.AddHours(-0.5).ToString("yyyy-MM-dd HH:mm:ss")}' " +
                               $"AND dtEventReal <= '{Salida.AddHours(2).ToString("yyyy-MM-dd HH:mm:ss")}'",
                               $"ORDER BY iEmployeeNum , dtEventReal");
                        }
                        shedule.Accesos = new List<LogDiaTurno>();
                        Logs.ForEach(log => {
                            shedule.Accesos.Add(new LogDiaTurno { TipoLog = log.TipoRegistro, Descripccion = log.tDesc, Re_time = log.dtEventReal });
                        });

                        shedule.Infordia = new TurnoDiareporte();
                        if (shedule.Accesos.Count > 0)
                        {
                            var Entradas = shedule.Accesos.Where(a => a.TipoLog == EnsamblesTipoChec.Entrada).ToList();
                            var Salidas = shedule.Accesos.Where(a => a.TipoLog == EnsamblesTipoChec.Salida).ToList();
                            if (Entradas.Count > 0)
                            {
                                shedule.Infordia.Entrada = Entradas.Min(a => a.Re_time);
                            }
                            else
                            {
                                shedule.Infordia.Logdescripcion += "NO hay registro de hora de entrada,";
                            }

                            if (Salidas.Count > 0)
                            {
                                shedule.Infordia.Salida = Salidas.Max(a => a.Re_time);
                            }
                            else
                            {
                                shedule.Infordia.Logdescripcion += "NO hay registro de hora de entrada,";
                            }
                        }
                        else
                        {
                            shedule.Infordia.Logdescripcion = "Sin registros de accesos";
                        }
                    }


                    
                });
                //Turno dia
            });
            #endregion
            return grupoDefs;
        }
        private List<DiaColaborador> ProcessDate(DateTime Fecha, string NumeroNomina)
        {

            DateTime Inicio = Fecha;
            DateTime Fin = DateTime.Parse(Fecha.AddDays(7).ToString("yyyy-MM-dd 23:59:59"));

            List<DiaColaborador> diasCol = new List<DiaColaborador>();

            while (Inicio <= Fin)
            {
                DiaColaborador diaColaborador = new DiaColaborador
                {
                    Incidencias = new List<string> { 
                        $"Dia: {Inicio.DayOfWeek.ToString()} {Inicio.ToString("yyyy-MM-dd HH:mm:ss")}"
                    },
                };
                diasCol.Add(diaColaborador);
                Inicio = Inicio.AddDays(1);
            }

            return diasCol;
        }
        #endregion

        #region Produccion horas trabajadas v2
        public List<CatalogoOpcionesValores> GetTurnosSelect()
        {
            var turno_re = darkManager.CatalogoOpcionesValores.GetOpenquery("where IdCatalogoOpciones = '1020'");
            return turno_re;
        }
        public List<EmpleadogrupoProd> GetEmpleadogrupoProds()
        {
            List<EmpleadogrupoProd> Lista = new List<EmpleadogrupoProd>();
            var re_empleados = darkManager.View_empleadoEnsamble.Get();
            re_empleados.ForEach(emp =>
            {
                var empleado = new EmpleadogrupoProd
                {
                    Active = true,
                    IdPersona = emp.IdPersona,
                    NombreCompleto = emp.NombreCompleto,
                    NumeroNomina = emp.NumeroNomina,
                    Puesto = emp.PuestoNombre,
                    Ingreso = emp.Ingreso,
                };
                var turno_re = darkManager.GrupoProduccion.GetOpenquerys($"where Fecha = '{DateTime.Now.ToString("yyyy-MM-dd")}' and IdPersona = '{emp.IdPersona}'");
                if (turno_re != null)
                {
                    empleado.Turno = turno_re.IdGrupo == 86 ? "Gris" : turno_re.IdGrupo == 87 ? "Rojo" : "Verde";
                }
                Lista.Add(empleado);
            });

            return Lista;
        }
        public void Chengegrupo(int IdPersona, int IdGrupo, DateTime Inicio, DateTime Fin)
        {
            darkManager.StartTransaction();
            try
            {
                while (Inicio <= Fin)
                {
                    var turno_re = darkManager.GrupoProduccion.GetOpenquerys($"where Fecha = '{Inicio.ToString("yyyy-MM-dd")}' and IdPersona = '{IdPersona}'");
                    if (turno_re != null)
                    {
                        turno_re.IdPersona = IdPersona;
                        turno_re.IdGrupo = IdGrupo;
                        turno_re.Modificado = DateTime.Now;
                        darkManager.GrupoProduccion.Element = turno_re;
                        if (darkManager.GrupoProduccion.Update() == false)
                        {
                            throw new Exceptions.GpExceptions(string.Format("Error al actualizar el cambio de grupo"));
                        }
                    }
                    else
                    {
                        turno_re = new GrupoProduccion();
                        turno_re.IdPersona = IdPersona;
                        turno_re.IdGrupo = IdGrupo;
                        turno_re.Fecha = Inicio;
                        turno_re.Creado = DateTime.Now;
                        turno_re.Modificado = DateTime.Now;
                        turno_re.Comentarios = "n/a";
                        darkManager.GrupoProduccion.Element = turno_re;
                        if (darkManager.GrupoProduccion.Add() == false)
                        {
                            throw new Exceptions.GpExceptions(string.Format("Error al registrar el cambio de grupo"));
                        }
                    }

                    Inicio = Inicio.AddDays(1);
                }
                darkManager.Commit();
            }
            catch (Exception)
            {
                darkManager.RolBack();
                throw;
            }
            
        }
        public List<GrupoProduccion> GetGrupos(int IdPersona)
        {
            var turno_re = darkManager.GrupoProduccion.GetOpenquery($"where IdPersona = '{IdPersona}'");
            return turno_re;
        }


        public WeekEmpleadoProd EmpleadoProds(DateTime Inicio)
        {
            darkManager.OpenConnectionAcces();
            darkManager.LoadObject(GpsControlAcceso.View_gps_ensambleSinFiltro);

            WeekEmpleadoProd weekEmpleadoProd = new WeekEmpleadoProd
            {
                Empleados = new List<EmpleadoProd>(),
                Inicio = Inicio,
                Fin = Inicio.AddDays(7)
            };

            
            var re_empleados = darkManager.View_empleadoEnsamble.Get();
            re_empleados.ForEach(emp => {
                var empleadoProd = ProcessEmpProd(emp, Inicio);
                weekEmpleadoProd.Empleados.Add(empleadoProd);
            });
            darkManager.CloseConnectionAccess();

            weekEmpleadoProd.Empleados = weekEmpleadoProd.Empleados.OrderBy(a => a.Nombre).ToList();
            return weekEmpleadoProd;
        }

        public EmpleadoProd ProcessEmpProd(View_empleadoEnsamble emp, DateTime Inicio, int IdPersona = 0)
        {
            if(IdPersona != 0)
            {
                darkManager.OpenConnectionAcces();
                darkManager.LoadObject(GpsControlAcceso.View_gps_ensambleSinFiltro);

                emp = darkManager.View_empleadoEnsamble.Get(IdPersona);

            }

            DateTime Fin = Inicio.AddDays(7);
            DateTime Corte = Inicio;

            EmpleadoProd empleadoProd = new EmpleadoProd
            {
                NumeroNomina = emp.NumeroNomina,
                Nombre = emp.NombreCompleto,
                Dias = new List<DiaProd>(),
                Inicio = Inicio,
                Fin = Fin,
                Puesto = emp.PuestoNombre,
                Antiguedad = emp.Antiguedad,
                IdPersona = emp.IdPersona,
                GrupoName = ""
            };
            

            while (Corte < Fin)
            {
                DiaProd diaProd = new DiaProd
                {
                    Dia = Corte,
                    Logs = new List<string>()
                };
                var turno_re = darkManager.GrupoProduccion.GetOpenquerys($"where Fecha = '{Corte.ToString("yyyy-MM-dd")}' and IdPersona = '{emp.IdPersona}'");
                if (turno_re != null)
                {
                    diaProd.IsLocated = true;
                    
                    diaProd.Turno = turno_re.IdGrupo == 86 ? "Gris" : turno_re.IdGrupo == 87 ? "Rojo" : turno_re.IdGrupo == 88 ? "Verde" : "Sin asginar";
                    empleadoProd.GrupoName = diaProd.Turno;
                    var DiaHorario_re = darkManager.GrupoHorario.GetOpenquerys($"where IdGrupo = '{turno_re.IdGrupo}' and dia = '{(int)Corte.DayOfWeek}'");
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
                        empleadoProd.HorasTrabajadas += diaProd.Horas;
                        diaProd.HorasMeta = DiaHorario_re.Horas;
                    }
                }
                else
                {
                    diaProd.IsLocated = false;
                    diaProd.Turno = "Not found";
                }
                empleadoProd.Dias.Add(diaProd);
                Corte = Corte.AddDays(1);
            }
            return empleadoProd;
        }
        #endregion
    }
}
