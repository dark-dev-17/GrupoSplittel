using GPSInformation.Models;
using GPSInformation.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPSInformation.Controllers
{
    public class IncidenciaCtrl
    {
        #region Atributos
        private int IdUsuario;
        private DarkManager darkManager;
        #endregion

        #region Constructores
        public IncidenciaCtrl(int IdUsuario, DarkManager darkManager)
        {
            this.IdUsuario = IdUsuario;
            this.darkManager = darkManager;
            this.darkManager.OpenConnection();
            this.darkManager.LoadObject(GpsManagerObjects.IncidenciaPermiso);
            this.darkManager.LoadObject(GpsManagerObjects.IncidenciaVacacion);
            this.darkManager.LoadObject(GpsManagerObjects.IncidenciaProcess);
        }
        public IncidenciaCtrl(DarkManager darkManager)
        {
            this.darkManager = darkManager;
            this.darkManager.OpenConnection();
            this.darkManager.LoadObject(GpsManagerObjects.IncidenciaPermiso);
            this.darkManager.LoadObject(GpsManagerObjects.IncidenciaVacacion);
            this.darkManager.LoadObject(GpsManagerObjects.IncidenciaProcess);
        }
        #endregion

        #region Metodos
        public void ProcessVacacionesComplete(DateTime Dia)
        {
            try
            {
                darkManager.StartTransaction();
                var Permisos_re = darkManager.IncidenciaVacacion.Get("1", "Estatus");
                Permisos_re.ForEach(mpermiso => {
                    var proceso = darkManager.IncidenciaProcess.Get("" + mpermiso.IdIncidenciaVacacion, "IdIncidenciaVacacion");
                    int Aprobaciones = proceso.Where(a => a.Revisada = true && a.Autorizada).ToList().Count;

                    if (Aprobaciones == 3)
                    {
                        if (mpermiso.Inicio <= Dia)
                        {

                            var LastProcess = proceso.Find(a => a.Nivel == 4);
                            if (LastProcess is null)
                            {
                                throw new Exceptions.GpExceptions("No se encontro el ultimo proceso de la incidencia");
                            }

                            LastProcess.Autorizada = true;
                            LastProcess.Revisada = true;
                            LastProcess.Fecha = DateTime.Now;
                            LastProcess.IdPersona = 221;
                            LastProcess.NombreEmpleado = "GPS Automatico";
                            LastProcess.IdIncidenciaVacacion = mpermiso.IdIncidenciaVacacion;

                            darkManager.IncidenciaProcess.Element = LastProcess;
                            if (!darkManager.IncidenciaProcess.Update())
                            {
                                throw new Exceptions.GpExceptions("Error al actualizar workflow");
                            }

                            //IncidenciaPermiso como tomada
                            mpermiso.IdIncidenciaVacacion = mpermiso.IdIncidenciaVacacion;
                            mpermiso.Estatus = 3;
                            darkManager.IncidenciaVacacion.Element = mpermiso;
                            if (!darkManager.IncidenciaVacacion.Update())
                            {
                                throw new Exceptions.GpExceptions("Error al actualizar datos maestros de la incidencia");
                            }


                        }
                    }
                });

                darkManager.Commit();
            }
            catch (Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                throw ex;
            }
        }
        public void ProcessPermisosComplete(DateTime Dia)
        {
            try
            {
                darkManager.StartTransaction();
                var Permisos_re = darkManager.IncidenciaPermiso.Get("1", "Estatus");
                Permisos_re.ForEach(mpermiso => {
                    var proceso = darkManager.IncidenciaProcess.Get("" + mpermiso.IdIncidenciaPermiso, "IdIncidenciaPermiso");
                    int Aprobaciones = proceso.Where(a => a.Revisada = true && a.Autorizada).ToList().Count;

                    if (Aprobaciones == 3)
                    {
                        if (mpermiso.Fecha <= Dia)
                        {

                            var LastProcess = proceso.Find(a => a.Nivel == 4);
                            if (LastProcess is null)
                            {
                                throw new Exceptions.GpExceptions("No se encontro el ultimo proceso de la incidencia");
                            }

                            LastProcess.Autorizada = true;
                            LastProcess.Revisada = true;
                            LastProcess.Fecha = DateTime.Now;
                            LastProcess.IdPersona = 221;
                            LastProcess.NombreEmpleado = "GPS Automatico";
                            LastProcess.IdIncidenciaPermiso = mpermiso.IdIncidenciaPermiso;

                            darkManager.IncidenciaProcess.Element = LastProcess;
                            if (!darkManager.IncidenciaProcess.Update())
                            {
                                throw new Exceptions.GpExceptions("Error al actualizar workflow");
                            }

                            //IncidenciaPermiso como tomada
                            mpermiso.IdIncidenciaPermiso = mpermiso.IdIncidenciaPermiso;
                            mpermiso.Estatus = 3;
                            darkManager.IncidenciaPermiso.Element = mpermiso;
                            if (!darkManager.IncidenciaPermiso.Update())
                            {
                                throw new Exceptions.GpExceptions("Error al actualizar datos maestros de la incidencia");
                            }


                        }
                    }
                });

                darkManager.Commit();
            }
            catch (Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                throw ex;
            }
        }
        #endregion
    }
}
