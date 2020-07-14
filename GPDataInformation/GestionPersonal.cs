using GPDataInformation.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;

namespace GPDataInformation
{
    public class GestionPersonal
    {
        private DBConnection DBConnection { get; set; }
        public string StringConnectionDb { get; set; }
        public string Server { get; set; }
        public string From { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool UserSSL { get; set; }
        public GestionPersonal()
        {

        }
        public GestionPersonal(string StringConnectionDb, int idUser)
        {
            this.StringConnectionDb = StringConnectionDb;
        }
        public GestionPersonal(IConfiguration Configuration)
        {
            this.StringConnectionDb = Configuration.GetConnectionString("Default");
        }

        public void OpenConnection()
        {
            DBConnection = new DBConnection(this.StringConnectionDb);
            DBConnection.OpenConnection();
        }
        public void CloseConnection()
        {
            if(DBConnection != null)
            {
                DBConnection.CloseDataBaseAccess();
            }
        }
        public dynamic GetObject(ObjectsCompany objectsCompany)
        {
            if (objectsCompany == ObjectsCompany.Sociedad)
            {
                Sociedad sociedad = new Sociedad();
                sociedad.SetConnection(DBConnection);
                return sociedad;
            }
            else
            {
                throw new GpExceptions("GpExceptions - Objeto no valido");
            }
        }
    }
    public enum ObjectsCompany
    {
        Sociedad = 1,
        Direccion = 2,
        Area = 3,
        Puestos = 4
    }
}
