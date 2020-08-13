using GPSInformation.DBManagers;
using GPSInformation.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSInformation
{
    public class DarkManager
    {
        protected DBConnection dBConnection { get; set; }
        protected string StringConnectionDb { get; set; }
        protected string Server { get; set; }
        protected string From { get; set; }
        protected int Port { get; set; }
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
        #endregion
        #region Constructtores
        public DarkManager(IConfiguration Configuration)
        {
            this.StringConnectionDb = Configuration.GetConnectionString("Default");
        }
        ~DarkManager()
        {

        }
        #endregion

        #region Base de datos
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
    }
}