using GPDataInformation.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPDataInformation
{
    public class GpsManager
    {
        #region atributos de configuracion
        protected DBConnection dBConnection { get; set; }
        protected string StringConnectionDb { get; set; }
        protected string Server { get; set; }
        protected string From { get; set; }
        protected int Port { get; set; }
        protected string User { get; set; }
        protected string Password { get; set; }
        protected bool UserSSL { get; set; }
        #endregion

        #region Variables de acceso
        public virtual DbManager<CatalogoOpciones> CatalogoOpciones { get; set; }
        public virtual DbManager<CatalogoOpcionesValores> CatalogoOpcionesValores { get; set; }
        public virtual DbManager<InformacionMedica> InformacionMedica { get; set; }
        public virtual DbManager<Empleado> Empleado { get; set; }
        public virtual DbManager<Persona> Persona { get; set; }
        public virtual DbManager<PersonaContacto> PersonaContacto { get; set; }
        public virtual DbManager<Sociedad> Sociedad { get; set; }
        public virtual DbManager<DireccionOrganizacional> Direccion { get; set; }
        public virtual DbManager<Departamento> Departamento { get; set; }
        public virtual DbManager<Puesto> Puesto { get; set; }
        #endregion


        #region Constructtores
        public GpsManager(IConfiguration Configuration)
        {
            this.StringConnectionDb = Configuration.GetConnectionString("Default");
        }
        #endregion

        #region Base de datos
        public string GetLastMessage()
        {
            return dBConnection.mensaje;
        }

        public void LoadObject(GpsManagerObjects gpsManagerObjects)
        {
            if(gpsManagerObjects == GpsManagerObjects.Persona)
            {
                Persona = new DbManager<Persona>(dBConnection);
            }
            if (gpsManagerObjects == GpsManagerObjects.InformacionMedica)
            {
                InformacionMedica = new DbManager<InformacionMedica>(dBConnection);
            }
            if (gpsManagerObjects == GpsManagerObjects.Empleado)
            {
                Empleado = new DbManager<Empleado>(dBConnection);
            }
            if (gpsManagerObjects == GpsManagerObjects.PersonaContacto)
            {
                PersonaContacto = new DbManager<PersonaContacto>(dBConnection);
            }
            if (gpsManagerObjects == GpsManagerObjects.Sociedad)
            {
                Sociedad = new DbManager<Sociedad>(dBConnection);
            }
            if (gpsManagerObjects == GpsManagerObjects.Direccion)
            {
                Direccion = new DbManager<DireccionOrganizacional>(dBConnection);
            }
            if (gpsManagerObjects == GpsManagerObjects.Departamento)
            {
                Departamento = new DbManager<Departamento>(dBConnection);
            }
            if (gpsManagerObjects == GpsManagerObjects.Puesto)
            {
                Puesto = new DbManager<Puesto>(dBConnection);
            }
        }

        public void OpenConnection()
        {
            dBConnection = new DBConnection(this.StringConnectionDb);
            dBConnection.OpenConnection();

            CatalogoOpciones = new DbManager<CatalogoOpciones>(dBConnection);
            CatalogoOpcionesValores = new DbManager<CatalogoOpcionesValores>(dBConnection);
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
        Sociedad = 1,
        Direccion = 2,
        Departamento = 3,
        Puesto = 4,

        Persona = 6,
        InformacionMedica = 7,
        Empleado = 8,
        PersonaContacto = 9
    }
}
