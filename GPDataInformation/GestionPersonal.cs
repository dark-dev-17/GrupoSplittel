﻿using System;

namespace GPDataInformation
{
    public class GestionPersonal
    {
        private DBConnection DBConnection { get; set; }
        private Correo Correo { get; set; }
        private PersonaInfo personaInfo { get; set; }
        public string StringConnectionDb { get; set; }
        public string Server { get; set; }
        public string From { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool UserSSL { get; set; }
        public GestionPersonal(string StringConnectionDb, int idUser)
        {
            this.StringConnectionDb = StringConnectionDb;
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
                DBConnection.CloseConnection();
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