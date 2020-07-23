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
        #endregion
        #region Constructtores
        public DarkManager(IConfiguration Configuration)
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
            if (gpsManagerObjects == GpsManagerObjects.CatalogoOpciones)
            {
                CatalogoOpciones = new DarkAttributes<CatalogoOpciones>(dBConnection);
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
        CatalogoOpciones = 1
    }

}