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
        public string GetLastMessage()
        {
            return DBConnection.mensaje;
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
        public dynamic GetObject(ObjectsCompany objectsCompany, dynamic Modelo)
        {
            if (objectsCompany == ObjectsCompany.Sociedad)
            {
                Sociedad sociedad = new Sociedad();
                if (Modelo != null)
                {
                    sociedad = Modelo;
                }
                sociedad.SetConnection(DBConnection);
                return sociedad;
            }
            if (objectsCompany == ObjectsCompany.DireccionOrganizacional)
            {
                DireccionOrganizacional objto = new DireccionOrganizacional();
                if (Modelo != null)
                {
                    objto = Modelo;
                }
                //dbSetGps.Add();
                objto.SetConnection(DBConnection);
                return objto;
            }
            if (objectsCompany == ObjectsCompany.Departamento)
            {
                Departamento objto = new Departamento();
                if (Modelo != null)
                {
                    objto = Modelo;
                }
                //dbSetGps.Add();
                objto.SetConnection(DBConnection);
                return objto;
            }
            else
            {
                throw new GpExceptions("GpExceptions - Objeto no valido");
            }
            
        }

        public virtual DbSetGps<DireccionOrganizacional> dbSetGps { get; set; }

    }
    public class DbSetGps<T> 
    {
        private T Local;

        DbSetGps()
        {

        }

        public bool Add()
        {
            throw new NotImplementedException();
        }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        public GPDataInformation.DbSetGps<T> Get(int? id)
        {
            throw new NotImplementedException();
        }

        public System.Collections.Generic.List<GPDataInformation.DbSetGps<T>> Get()
        {
            throw new NotImplementedException();
        }

        public int GetLastId()
        {
            throw new NotImplementedException();
        }

        public void SetConnection(DBConnection dBConnection)
        {
            throw new NotImplementedException();
        }

        public bool Update()
        {
            throw new NotImplementedException();
        }
    }
    public enum ObjectsCompany
    {
        Sociedad = 1,
        DireccionOrganizacional = 2,
        Area = 3,
        Puestos = 4,
        Departamento = 5
    }
}
