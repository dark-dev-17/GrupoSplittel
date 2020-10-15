using GPSInformation.Models;
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

            Nomenclatura = Funciones.GetRegistrosInc();
        }
        #endregion
        #region Metodos
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
    }
}
