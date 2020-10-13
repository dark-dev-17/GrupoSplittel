using GPSInformation.Models;
using GPSInformation.Reportes;
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
        //public List<Registro> Nomenclatura = new List<Registro>();
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
        }
        #endregion
        #region Metodos
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
                "WHERE EMP.tIdentification = '{1}'", Inicio.ToString("yyyy/MM/dd"), NumeroNomina);

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
                            Inicio = Turnos_re.Entrada.Add(TimeSpan.FromHours(-1)),
                            Fin = Turnos_re.Salida.Add(TimeSpan.FromHours(1)),
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
                            Inicio = Turnos_re.Entrada.Add(TimeSpan.FromHours(-1)),
                            Fin = Turnos_re.Salida.Add(TimeSpan.FromHours(1)),
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
