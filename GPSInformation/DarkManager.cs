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
    }

}