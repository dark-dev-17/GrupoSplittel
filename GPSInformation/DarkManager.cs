using GPSInformation.DBManagers;
using GPSInformation.Models;
using GPSInformation.Tools;
using GPSInformation.Views;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace GPSInformation
{
    public class DarkManager
    {
        protected DBConnection dBConnection { get; set; }
        protected DBConnection dBConnectionAccess { get; set; }
        public EmailServ EmailServ_ { get; set; }
        protected string DefaultAccess { get; set; }
        protected string StringConnectionDb { get; set; }
        protected string Server { get; set; }
        protected string From { get; set; }
        protected string Port { get; set; }
        protected string User { get; set; }
        protected string Password { get; set; }
        protected bool UserSSL { get; set; }

        #region Variables de acceso
        public virtual DarkAttributes<CatalogoOpciones> CatalogoOpciones { get; set; }
        public virtual DarkAttributes<CatalogoOpcionesValores> CatalogoOpcionesValores { get; set; }
        public virtual DarkAttributes<Sociedad> Sociedad { get; set; }
        public virtual DarkAttributes<Direccion> Direccion { get; set; }
        public virtual DarkAttributes<Departamento> Departamento { get; set; }
        public virtual DarkAttributes<Puesto> Puesto { get; set; }
        public virtual DarkAttributes<Persona> Persona { get; set; }
        public virtual DarkAttributes<InformacionMedica> InformacionMedica { get; set; }
        public virtual DarkAttributes<Empleado> Empleado { get; set; }
        public virtual DarkAttributes<PersonaContacto> PersonaContacto { get; set; }
        public virtual DarkAttributes<IncidenciaPermiso> IncidenciaPermiso { get; set; }
        public virtual DarkAttributes<IncidenciaPermisoProcess> IncidenciaPermisoProcess { get; set; }
        public virtual DarkAttributes<OrganigramaVersion> OrganigramaVersion { get; set; }
        public virtual DarkAttributes<OrganigramaStructura> OrganigramaStructura { get; set; }
        public virtual DarkAttributes<Sala> Sala { get; set; }
        public virtual DarkAttributes<SalaReservacion> SalaReservacion { get; set; }
        public virtual DarkAttributes<Usuario> Usuario { get; set; }
        public virtual DarkAttributes<RequisicionPersonal> RequisicionPersonal { get; set; }
        public virtual DarkAttributes<RequisicionHabilidades> RequisicionHabilidades { get; set; }
        public virtual DarkAttributes<VacacionesDiasRegla> VacacionesDiasRegla { get; set; }
        public virtual DarkAttributes<DiaFeriado> DiaFeriado { get; set; }
        public virtual DarkAttributes<IncidenciaProcess> IncidenciaProcess { get; set; }
        public virtual DarkAttributes<IncidenciaVacacion> IncidenciaVacacion { get; set; }
        public virtual DarkAttributes<Modulo> Modulo { get; set; }
        public virtual DarkAttributes<SubModulo> SubModulo { get; set; }
        public virtual DarkAttributes<AccesosSistema> AccesosSistema { get; set; }
        public virtual DarkAttributes<VacionesPeriodo> VacionesPeriodo { get; set; }
        public virtual DarkAttributes<Evaluacion> Evaluacion { get; set; }
        public virtual DarkAttributes<EvaluacionSeccionPregnts> EvaluacionSeccionPregnts { get; set; }
        public virtual DarkAttributes<EvaluacioSeccion> EvaluacioSeccion { get; set; }
        public virtual DarkAttributes<EvaluacionTemplate> EvaluacionTemplate { get; set; }
        public virtual DarkAttributes<View_empleado> View_empleado { get; set; }
        public virtual DarkAttributes<EvaluacionEmpleado> EvaluacionEmpleado { get; set; }
        public virtual DarkAttributes<EvaluacionRespuestas> EvaluacionRespuestas { get; set; }
        public virtual DarkAttributes<ExpedienteEmpleado> ExpedienteEmpleado { get; set; }
        public virtual DarkAttributes<ExpedienteArchivo> ExpedienteArchivo { get; set; }
        public virtual DarkAttributes<View_EmpleadoExpediente> View_EmpleadoExpediente { get; set; }
        public virtual DarkAttributes<FaltaJustificacion> FaltaJustificacion { get; set; }
        public virtual DarkAttributes<TurnosProduccion> TurnosProduccion { get; set; }
        public virtual DarkAttributes<TurnoEmpleado> TurnoEmpleado { get; set; }
        public virtual DarkAttributes<View_empleadoEnsamble> View_empleadoEnsamble { get; set; }
        public virtual DarkAttributes<EnsablesTurnos> EnsablesTurnos { get; set; }
        public virtual DarkAttributes<Nomina> Nomina { get; set; }
        public virtual DarkAttributes<InformacionCompania> InformacionCompania { get; set; }
        public virtual DarkAttributes<EmpleadoContrato> EmpleadoContrato { get; set; }
        public virtual DarkAttributes<BuzonQueja> BuzonQueja { get; set; }
        public virtual DarkAttributes<EvaluacionInstructor> EvaluacionInstructor { get; set; }


        private string CorreosBCC { get; set; }

        #endregion

        #region Constructtores
        public DarkManager(IConfiguration Configuration)
        {
            this.StringConnectionDb = Configuration.GetConnectionString("Default");
            this.DefaultAccess = Configuration.GetConnectionString("DefaultAccess");
            Server = Configuration.GetSection("Smtp").GetSection("Server").Value;
            Port = Configuration.GetSection("Smtp").GetSection("Port").Value;
            From = Configuration.GetSection("Smtp").GetSection("account").Value;
            User = Configuration.GetSection("Smtp").GetSection("User").Value;
            Password = Configuration.GetSection("Smtp").GetSection("Password").Value;
            UserSSL = Configuration.GetSection("Smtp").GetSection("Ssl").Value == "true" ? true : false;

            CorreosBCC = Configuration.GetSection("Smtp").GetSection("Bcc").Value;
            EmailServ_ = new EmailServ(Server, From,Port,User,Password,UserSSL);
            EmailServ_.AddListBCC(CorreosBCC);
        }
        public DarkManager(string DBconnection)
        {
            this.StringConnectionDb = DBconnection;
        }
        ~DarkManager()
        {
            //CloseConnection();
        }
        #endregion

        #region Base de datos
        public void RestartEmail()
        {
            EmailServ_ = new EmailServ(Server, From, Port, User, Password, UserSSL);
            EmailServ_.AddListBCC(CorreosBCC);
        }
        public string GetLastMessage()
        {
            return dBConnection.mensaje;
        }

        public void LoadObject(GpsManagerObjects gpsManagerObjects)
        {
            if (gpsManagerObjects == GpsManagerObjects.CatalogoOpciones)
            {
                CatalogoOpciones = new DarkAttributes<CatalogoOpciones>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.CatalogoOpcionesValores)
            {
                CatalogoOpcionesValores = new DarkAttributes<CatalogoOpcionesValores>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.Sociedad)
            {
                Sociedad = new DarkAttributes<Sociedad>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.Direccion)
            {
                Direccion = new DarkAttributes<Direccion>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.Departamento)
            {
                Departamento = new DarkAttributes<Departamento>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.Puesto)
            {
                Puesto = new DarkAttributes<Puesto>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.Persona)
            {
                Persona = new DarkAttributes<Persona>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.InformacionMedica)
            {
                InformacionMedica = new DarkAttributes<InformacionMedica>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.Empleado)
            {
                Empleado = new DarkAttributes<Empleado>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.PersonaContacto)
            {
                PersonaContacto = new DarkAttributes<PersonaContacto>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.IncidenciaPermiso)
            {
                IncidenciaPermiso = new DarkAttributes<IncidenciaPermiso>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.IncidenciaPermisoProcess)
            {
                IncidenciaPermisoProcess = new DarkAttributes<IncidenciaPermisoProcess>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.OrganigramaVersion)
            {
                OrganigramaVersion = new DarkAttributes<OrganigramaVersion>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.OrganigramaStructura)
            {
                OrganigramaStructura = new DarkAttributes<OrganigramaStructura>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.Usuario)
            {
                Usuario = new DarkAttributes<Usuario>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.Sala)
            {
                Sala = new DarkAttributes<Sala>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.SalaReservacion)
            {
                SalaReservacion = new DarkAttributes<SalaReservacion>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.RequisicionPersonal)
            {
                RequisicionPersonal = new DarkAttributes<RequisicionPersonal>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.RequisicionHabilidades)
            {
                RequisicionHabilidades = new DarkAttributes<RequisicionHabilidades>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.VacacionesDiasRegla)
            {
                VacacionesDiasRegla = new DarkAttributes<VacacionesDiasRegla>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.DiaFeriado)
            {
                DiaFeriado = new DarkAttributes<DiaFeriado>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.IncidenciaVacacion)
            {
                IncidenciaVacacion = new DarkAttributes<IncidenciaVacacion>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.IncidenciaProcess)
            {
                IncidenciaProcess = new DarkAttributes<IncidenciaProcess>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.Modulo)
            {
                Modulo = new DarkAttributes<Modulo>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.SubModulo)
            {
                SubModulo = new DarkAttributes<SubModulo>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.AccesosSistema)
            {
                AccesosSistema = new DarkAttributes<AccesosSistema>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.VacionesPeriodo)
            {
                VacionesPeriodo = new DarkAttributes<VacionesPeriodo>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.Evaluacion)
            {
                Evaluacion = new DarkAttributes<Evaluacion>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.EvaluacionSeccionPregnts)
            {
                EvaluacionSeccionPregnts = new DarkAttributes<EvaluacionSeccionPregnts>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.EvaluacioSeccion)
            {
                EvaluacioSeccion = new DarkAttributes<EvaluacioSeccion>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.EvaluacionTemplate)
            {
                EvaluacionTemplate = new DarkAttributes<EvaluacionTemplate>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.View_empleado)
            {
                View_empleado = new DarkAttributes<View_empleado>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.EvaluacionEmpleado)
            {
                EvaluacionEmpleado = new DarkAttributes<EvaluacionEmpleado>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.EvaluacionRespuestas)
            {
                EvaluacionRespuestas = new DarkAttributes<EvaluacionRespuestas>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.ExpedienteEmpleado)
            {
                ExpedienteEmpleado = new DarkAttributes<ExpedienteEmpleado>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.ExpedienteArchivo)
            {
                ExpedienteArchivo = new DarkAttributes<ExpedienteArchivo>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.View_EmpleadoExpediente)
            {
                View_EmpleadoExpediente = new DarkAttributes<View_EmpleadoExpediente>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.FaltaJustificacion)
            {
                FaltaJustificacion = new DarkAttributes<FaltaJustificacion>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.TurnosProduccion)
            {
                TurnosProduccion = new DarkAttributes<TurnosProduccion>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.TurnoEmpleado)
            {
                TurnoEmpleado = new DarkAttributes<TurnoEmpleado>(dBConnection);
            } 
            else if (gpsManagerObjects == GpsManagerObjects.View_empleadoEnsamble)
            {
                View_empleadoEnsamble = new DarkAttributes<View_empleadoEnsamble>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.EnsablesTurnos)
            {
                EnsablesTurnos = new DarkAttributes<EnsablesTurnos>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.Nomina)
            {
                Nomina = new DarkAttributes<Nomina>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.InformacionCompania)
            {
                InformacionCompania = new DarkAttributes<InformacionCompania>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.EmpleadoContrato)
            {
                EmpleadoContrato = new DarkAttributes<EmpleadoContrato>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.BuzonQueja)
            {
                BuzonQueja = new DarkAttributes<BuzonQueja>(dBConnection);
            }
            else if (gpsManagerObjects == GpsManagerObjects.EvaluacionInstructor)
            {
                EvaluacionInstructor = new DarkAttributes<EvaluacionInstructor>(dBConnection);
            }
        }
        public void OpenConnection()
        {
            dBConnection = new DBConnection(this.StringConnectionDb);
            dBConnection.OpenConnection();
        }
        public void CloseConnection()
        {
            if (dBConnection != null)
            {
                dBConnection.CloseDataBaseAccess();
            }
        }
        public void StartTransaction()
        {
            dBConnection.StartTransaction();
        }
        public void Commit()
        {
            dBConnection.Commit();
        }
        public void RolBack()
        {
            dBConnection.RolBack();
        }

        #endregion

        #region Control de accesos
        public void OpenConnectionAcces()
        {
            dBConnectionAccess = new DBConnection(this.DefaultAccess);
            dBConnectionAccess.OpenConnection();
        }
        public void CloseConnectionAccess()
        {
            if (dBConnectionAccess != null)
            {
                dBConnectionAccess.CloseDataBaseAccess();
            }
        }
        public SqlDataReader ExecuteStatementAccess(string Statement)
        {
            return dBConnectionAccess.GetDataReader(Statement);
        }
        
        #endregion
    }

    public enum GpsManagerObjects
    {
        CatalogoOpciones = 1,
        CatalogoOpcionesValores = 2,
        Sociedad = 3,
        Direccion = 4,
        Departamento = 5,
        Puesto = 6,
        Persona = 7,
        InformacionMedica = 8,
        Empleado = 9,
        PersonaContacto = 10,
        IncidenciaPermiso = 11,
        IncidenciaPermisoProcess = 12,
        OrganigramaVersion = 13,
        OrganigramaStructura = 14,
        Usuario = 15,
        Sala = 16,
        SalaReservacion = 17,
        RequisicionPersonal = 19,
        RequisicionHabilidades = 20,
        VacacionesDiasRegla = 21,
        DiaFeriado = 22,
        IncidenciaVacacion = 23,
        IncidenciaProcess = 24,
        Modulo = 25,
        SubModulo = 26,
        AccesosSistema = 27,
        VacionesPeriodo = 28,
        Evaluacion = 29,
        EvaluacionSeccionPregnts = 30,
        EvaluacioSeccion = 31,
        EvaluacionTemplate = 32,
        View_empleado = 33,
        EvaluacionEmpleado = 34,
        EvaluacionRespuestas = 35,
        ExpedienteEmpleado = 36,
        ExpedienteArchivo = 37,
        View_EmpleadoExpediente = 38,
        FaltaJustificacion = 39,
        TurnosProduccion = 40,
        TurnoEmpleado = 41,
        View_empleadoEnsamble = 42,
        EnsablesTurnos = 43,
        Nomina = 44,
        InformacionCompania = 45,
        EmpleadoContrato = 46,
        BuzonQueja = 47,
        EvaluacionInstructor = 48,
    }
}