using GPSInformation.Exceptions;
using GPSInformation.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSInformation.Controllers
{
    public class VacacionesCtrl
    {
        #region Atributos
        private int IdUsuario;
        private DarkManager darkManager;
        #endregion

        #region Constructores
        public VacacionesCtrl(int IdUsuario, DarkManager darkManager)
        {
            this.IdUsuario = IdUsuario;
            this.darkManager = darkManager;
            this.darkManager.LoadObject(GpsManagerObjects.DiaFeriado);
            this.darkManager.LoadObject(GpsManagerObjects.VacacionesDiasRegla);
        }
        #endregion

        #region Metodos

        public List<VacionesPeriodo> Get()
        {
            return darkManager.VacionesPeriodo.Get(IdUsuario + "","IdPersona" );
        }

        public void ProcPeridosVac(int IdPersona)
        {
            darkManager.StartTransaction();
            try
            {
                ProcPeridosVacUs(IdPersona);
                darkManager.Commit();
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                throw ex;
            }
        }
        public void ProcPeridosVacAll()
        {
            darkManager.StartTransaction();
            try
            {
                darkManager.View_empleado.Get().ForEach( emp => {
                    ProcPeridosVacUs(emp.IdPersona);
                });
               
                darkManager.Commit();
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                throw ex;
            }
        }

        private void ProcPeridosVacUs(int IdPersona)
        {

                var Empleado_re = darkManager.Empleado.GetByColumn("" + IdPersona, "IdPersona");
                if (Empleado_re == null)
                {
                    throw new GpExceptions("No existe el empleado");
                }
                double years = GetDiff(Empleado_re.Ingreso);
                int Antiguedad = Int32.Parse(years + "");

                if (Antiguedad > 0)
                {
                    for (int i = 1; i <= Antiguedad; i++)
                    {
                        var peridod_re = darkManager.VacionesPeriodo.Get("IdPersona", IdPersona + "", "NumeroPeriodo", i + "");
                        if (peridod_re == null)
                        {
                            var diaPeridodo = darkManager.VacacionesDiasRegla.GetByColumn("" + i, "NoAnio");
                            VacionesPeriodo vacionesPeriodo = new VacionesPeriodo();
                            vacionesPeriodo.IdPersona = IdPersona;
                            vacionesPeriodo.NumeroPeriodo = i;
                            vacionesPeriodo.DiasAprobadors = diaPeridodo != null ? diaPeridodo.NoDias : darkManager.VacacionesDiasRegla.Get(darkManager.VacacionesDiasRegla.GetLastId()).NoDias;
                            vacionesPeriodo.DiasUsados = 0;
                            vacionesPeriodo.Creado = DateTime.Now;
                            vacionesPeriodo.Actualizado = DateTime.Now;
                            vacionesPeriodo.Completo = true;
                            darkManager.VacionesPeriodo.Element = vacionesPeriodo;
                            if (!darkManager.VacionesPeriodo.Add())
                            {
                                throw new GpExceptions("Error al crear el periodo de vacaciones no " + i);
                            }
                        }
                        else
                        {
                            if (!peridod_re.Completo)
                            {
                                var diaPeridodo = darkManager.VacacionesDiasRegla.GetByColumn("" + i, "NoAnio");
                                peridod_re.DiasAprobadors = diaPeridodo != null ? diaPeridodo.NoDias : darkManager.VacacionesDiasRegla.Get(darkManager.VacacionesDiasRegla.GetLastId()).NoDias; ;
                                peridod_re.Actualizado = DateTime.Now;
                                peridod_re.Completo = true;
                                darkManager.VacionesPeriodo.Element = peridod_re;
                                if (!darkManager.VacionesPeriodo.Update())
                                {
                                    throw new GpExceptions("Error al actualizar el periodo de vacaciones no " + i);
                                }
                            }
                        }
                    }
                    DateTime fechaLast = Empleado_re.Ingreso.AddYears(Antiguedad).AddMonths(6);
                    if (fechaLast <= DateTime.Now && darkManager.VacionesPeriodo.Get("IdPersona", IdPersona + "", "NumeroPeriodo", (Antiguedad + 1) + "") == null)
                    {
                        VacionesPeriodo vacionesPeriodo = new VacionesPeriodo();
                        var diaPeridodo = darkManager.VacacionesDiasRegla.GetByColumn("" + (Antiguedad + 1), "NoAnio");
                        vacionesPeriodo.IdPersona = IdPersona;
                        vacionesPeriodo.NumeroPeriodo = Antiguedad + 1;
                        vacionesPeriodo.DiasAprobadors = (diaPeridodo != null ? diaPeridodo.NoDias : darkManager.VacacionesDiasRegla.Get(darkManager.VacacionesDiasRegla.GetLastId()).NoDias) / 2;
                        vacionesPeriodo.DiasUsados = 0;
                        vacionesPeriodo.Completo = false;
                        vacionesPeriodo.Creado = DateTime.Now;
                        vacionesPeriodo.Actualizado = DateTime.Now;
                        darkManager.VacionesPeriodo.Element = vacionesPeriodo;
                        if (!darkManager.VacionesPeriodo.Add())
                        {
                            throw new GpExceptions("Error al crear el periodo de vacaciones no " + Antiguedad + 1);
                        }
                    }
                }
                else
                {
                    DateTime fecha10 = Empleado_re.Ingreso.AddMonths(10);
                    //DateTime fecha6 = Empleado_re.Ingreso.AddMonths(6);
                    if (fecha10 <= DateTime.Now && darkManager.VacionesPeriodo.Get("IdPersona", IdPersona + "", "NumeroPeriodo", 1 + "") == null)
                    {
                        VacionesPeriodo vacionesPeriodo = new VacionesPeriodo();
                        vacionesPeriodo.IdPersona = IdPersona;
                        vacionesPeriodo.NumeroPeriodo = 1;
                        vacionesPeriodo.DiasAprobadors = darkManager.VacacionesDiasRegla.GetByColumn("" + 1, "NoAnio").NoDias / 2;
                        vacionesPeriodo.DiasUsados = 0;
                        vacionesPeriodo.Completo = false;
                        vacionesPeriodo.Creado = DateTime.Now;
                        vacionesPeriodo.Actualizado = DateTime.Now;
                        darkManager.VacionesPeriodo.Element = vacionesPeriodo;
                        if (!darkManager.VacionesPeriodo.Add())
                        {
                            throw new GpExceptions("Error al crear el periodo de vacaciones no " + 1);
                        }
                    }
                }
                //darkManager.VacionesPeriodo.Get();

        }

        private int GetDiff(DateTime fechaInicio)
        {
            TimeSpan antiguedad = DateTime.Now - fechaInicio;
            int  years = antiguedad.Days / 365;
            //return (decimal)Math.Round(years);
            return years;
        }

        private TimeSpan GetDifferencia(DateTime fechaInicio)
        {
            return DateTime.Now - fechaInicio;
        }
        #endregion
    }
}
