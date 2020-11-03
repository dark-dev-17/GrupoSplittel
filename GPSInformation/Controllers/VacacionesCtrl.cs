using GPSInformation.Arquitectura;
using GPSInformation.Exceptions;
using GPSInformation.Models;
using GPSInformation.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPSInformation.Controllers
{
    public class VacacionesCtrl: Controlador
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
            this.darkManager.LoadObject(GpsManagerObjects.View_empleado);
            this.darkManager.LoadObject(GpsManagerObjects.IncidenciaProcess);
        }
        #endregion

        #region Metodos
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdIncidenciaVacacion"></param>
        /// <param name="IdPersonaOwner"></param>
        public void Cancelar(int IdIncidenciaVacacion, int IdPersonaOwner = 0)
        {
            darkManager.StartTransaction();
            try
            {
                var result = darkManager.IncidenciaVacacion.Get(IdIncidenciaVacacion);

                if(result is null)
                    throw new GpExceptions("No se encontro la solicitud seleccionada");
                
                if(IdPersonaOwner != 0)
                {
                    if(result.IdPersona != IdPersonaOwner)
                    {
                        throw new GpExceptions("Advertencia, no puedes cancelar solicitudes que no sean tuyas");
                    }
                }

                result.Estatus = 2; // cancel
                darkManager.IncidenciaVacacion.Element = result;

                if (!darkManager.IncidenciaVacacion.Update())
                {
                    throw new GpExceptions("Error, cancelar la solicitud de vacaciones");
                }

                darkManager.Commit();
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                throw ex;
            }
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="IdIncidenciaVacacion"></param>
        /// <param name="Aprobador"></param>
        /// <param name="Comentarios"></param>
        /// <param name="acepted"></param>
        /// <param name="Mode"></param>
        public void Autorizar(int IdIncidenciaVacacion, int Aprobador, string Comentarios, bool acepted, IncidenciasPerVac Mode)
        {
            darkManager.StartTransaction();
            try
            {
                //traer procesos
                var result = darkManager.IncidenciaProcess.Get("" + IdIncidenciaVacacion, nameof(darkManager.IncidenciaProcess.Element.IdIncidenciaVacacion));
                IncidenciaProcess nivel = null;
                if (Mode == IncidenciasPerVac.JefeImediato)
                    //nivel jefe
                    nivel = result.Find(a => a.Nivel == 2);
                else if(Mode == IncidenciasPerVac.GestionPersonal)
                    //nivel GPS
                    nivel = result.Find(a => a.Nivel == 3);
                else
                    throw new GpExceptions("El parametro Nivel no es valido");

                var Persona = darkManager.Persona.Get(Aprobador);
                if(Persona is null)
                    throw new GpExceptions("El aprobador no fue encontrado");

                nivel.Autorizada = acepted;
                nivel.Fecha = DateTime.Now;
                nivel.IdPersona = Aprobador;
                nivel.NombreEmpleado = Persona.NombreCompelto;
                nivel.Revisada = true;
                nivel.IdIncidenciaVacacion = IdIncidenciaVacacion;
                nivel.Comentarios = Comentarios;

                if (!darkManager.IncidenciaProcess.Update())
                    throw new GpExceptions(string.Format("Error al {0} la solicitud ", acepted ? "Aprobar": "Rechazar"));

                darkManager.Commit();
            }
            catch (GPSInformation.Exceptions.GpExceptions ex)
            {
                darkManager.RolBack();
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdIncidenciaVacacion"></param>
        /// <returns></returns>
        public IncidenciaVacacion Get(int IdIncidenciaVacacion)
        {
            var vac = darkManager.IncidenciaVacacion.Get(IdIncidenciaVacacion);
            if(vac != null)
            {
                vac.Proceso = darkManager.IncidenciaProcess.Get("" + vac.IdIncidenciaVacacion, "IdIncidenciaVacacion");
            }
            return vac;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<VacionesPeriodo> Get()
        {
            return darkManager.VacionesPeriodo.Get(IdUsuario + "","IdPersona" );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdPersona"></param>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="incidenciaVacacions"></param>
        /// <param name="GetProcess"></param>
        /// <param name="EmpInfo"></param>
        /// <returns></returns>
        private List<IncidenciaVacacion> AddMore(List<IncidenciaVacacion> incidenciaVacacions, bool GetProcess = false, bool EmpInfo = false)
        {

            View_empleado Emp = null;

            if(incidenciaVacacions.Count > 0)
            {
                incidenciaVacacions.ForEach(vac => {
                    if (Emp == null)
                    {
                        Emp = darkManager.View_empleado.Get(vac.IdPersona);
                    }
                    if(Emp != null && Emp.IdPersona != vac.IdPersona)
                    {
                        Emp = darkManager.View_empleado.Get(vac.IdPersona);
                    }
                    vac.EmpleadoNombre = Emp.NombreCompleto;
                    if(GetProcess)
                        vac.Proceso = darkManager.IncidenciaProcess.Get("" + vac.IdIncidenciaVacacion, "IdIncidenciaVacacion");
                });
            }
            
            return incidenciaVacacions;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdPersona"></param>
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
        /// <summary>
        /// Obtener diferencia en años 
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <returns></returns>
        private int GetDiff(DateTime fechaInicio)
        {
            TimeSpan antiguedad = DateTime.Now - fechaInicio;
            int  years = antiguedad.Days / 365;
            //return (decimal)Math.Round(years);
            return years;
        }
        /// <summary>
        /// Obtener diferencia de tiempo desde una fecha dada hasta hoy
        /// </summary>
        /// <param name="fechaInicio">Fecha de incio</param>
        /// <returns></returns>
        private TimeSpan GetDifferencia(DateTime fechaInicio)
        {
            return DateTime.Now - fechaInicio;
        }
        /// <summary>
        /// Finalizar objeto y recolectar memoria de la instancia
        /// </summary>
        public void Terminar()
        {
            darkManager.CloseConnection();
            this.darkManager.DiaFeriado = null;
            this.darkManager.VacacionesDiasRegla = null;
            darkManager = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        /// <summary>
        /// Obtener días habiles entre dos fechas
        /// </summary>
        /// <param name="desde"></param>
        /// <param name="hasta"></param>
        /// <returns></returns>
        private int GetDays(DateTime desde, DateTime hasta)
        {
            DateTime inicio = desde;
            int dias = 0;
            while (inicio <= hasta)
            {
                if (inicio.DayOfWeek != DayOfWeek.Saturday && inicio.DayOfWeek != DayOfWeek.Sunday)
                {
                    var result = darkManager.DiaFeriado.GetByColumn("", nameof(darkManager.DiaFeriado.Element.Fecha));
                    if (result == null)
                    {
                        dias++;
                    }
                }
                inicio = inicio.AddDays(1);
            }
            return dias;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IncidenciaVacacion"></param>
        private void AddSteps(IncidenciaVacacion IncidenciaVacacion)
        {
            var procesoStep = new IncidenciaProcess();
            procesoStep.IdIncidenciaVacacion = IncidenciaVacacion.IdIncidenciaVacacion;
            procesoStep.IdPersona = IncidenciaVacacion.IdPersona;
            procesoStep.Fecha = DateTime.Now;
            procesoStep.Titulo = "Incidencia creada por solicitante";
            procesoStep.Comentarios = "";
            procesoStep.Nivel = 1;
            procesoStep.Revisada = true;
            procesoStep.Autorizada = true;
            procesoStep.NombreEmpleado = darkManager.View_empleado.Get(IncidenciaVacacion.IdPersona).NombreCompleto;
            darkManager.IncidenciaProcess.Element = procesoStep;
            darkManager.IncidenciaProcess.Add();

            procesoStep.IdPersona = 0;
            procesoStep.Fecha = null;
            procesoStep.Titulo = "Aprobación por jefe inmediato";
            procesoStep.Comentarios = "";
            procesoStep.Nivel = 2;
            procesoStep.Revisada = false;
            procesoStep.Autorizada = false;
            procesoStep.NombreEmpleado = "";
            darkManager.IncidenciaProcess.Element = procesoStep;
            darkManager.IncidenciaProcess.Add();

            procesoStep.IdPersona = 0;
            procesoStep.Fecha = null;
            procesoStep.Titulo = "Aprobación por gestión de personal";
            procesoStep.Comentarios = "";
            procesoStep.Nivel = 3;
            procesoStep.Revisada = false;
            procesoStep.Autorizada = false;
            procesoStep.NombreEmpleado = "";
            darkManager.IncidenciaProcess.Element = procesoStep;
            darkManager.IncidenciaProcess.Add();

            procesoStep.IdPersona = 0;
            procesoStep.Fecha = null;
            procesoStep.Titulo = "Vacaciones concluidas/tomadas";
            procesoStep.Comentarios = "";
            procesoStep.Nivel = 4;
            procesoStep.Revisada = false;
            procesoStep.Autorizada = false;
            procesoStep.NombreEmpleado = "";
            darkManager.IncidenciaProcess.Element = procesoStep;
            darkManager.IncidenciaProcess.Add();
        }
        #endregion
    }

    public enum IncidenciasPerVac
    {
        JefeImediato = 1,
        GestionPersonal = 2
    }
}
