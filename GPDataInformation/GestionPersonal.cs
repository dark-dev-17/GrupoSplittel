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
                objto.SetConnection(DBConnection);
                return objto;
            }
            if (objectsCompany == ObjectsCompany.Puesto)
            {
                Puesto objto = new Puesto();
                if (Modelo != null)
                {
                    objto = Modelo;
                }
                objto.SetConnection(DBConnection);
                return objto;
            }
            if (objectsCompany == ObjectsCompany.CatalogoOpciones)
            {
                CatalogoOpciones objto = new CatalogoOpciones();
                if (Modelo != null)
                {
                    objto = Modelo;
                }
                objto.SetConnection(DBConnection);
                return objto;
            }
            if (objectsCompany == ObjectsCompany.Persona)
            {
                Persona objto = new Persona();
                if (Modelo != null)
                {
                    objto = Modelo;
                }
                objto.SetConnection(DBConnection);
                return objto;
            }
            if (objectsCompany == ObjectsCompany.Empleado)
            {
                Empleado objto = new Empleado();
                if (Modelo != null)
                {
                    objto = Modelo;
                }
                objto.SetConnection(DBConnection);
                return objto;
            }
            if (objectsCompany == ObjectsCompany.PersonaContacto)
            {
                PersonaContacto objto = new PersonaContacto();
                if (Modelo != null)
                {
                    objto = Modelo;
                }
                objto.SetConnection(DBConnection);
                return objto;
            }
            if (objectsCompany == ObjectsCompany.InformacionMedica)
            {
                InformacionMedica objto = new InformacionMedica();
                if (Modelo != null)
                {
                    objto = Modelo;
                }
                objto.SetConnection(DBConnection);
                return objto;
            }if (objectsCompany == ObjectsCompany.SplittelEmpleado)
            {
                SplittelEmpleado objto = new SplittelEmpleado();
                if (Modelo != null)
                {
                    objto = Modelo;
                }
                objto.SetConnection(DBConnection);
                return objto;
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
        DireccionOrganizacional = 2,
        Area = 3,
        Puesto = 4,
        Departamento = 5,
        CatalogoOpciones = 6,
        Persona = 7,
        Empleado = 8,
        PersonaContacto = 9,
        InformacionMedica = 10,
        SplittelEmpleado = 11,
    }
}
