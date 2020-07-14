using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace GPDataInformation.Models
{
    public class Sociedad 
    {
        public int IdSociedad { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public string Direccion { get; set; }
        //public ICollection<Direccion> Direcciones { get; set; }

        private DBConnection dBConnection;

        public bool Add()
        {
            throw new NotImplementedException();
        }
        public bool Delete()
        {
            throw new NotImplementedException();
        }
        public bool Update()
        {
            throw new NotImplementedException();
        }
        public  Sociedad Get(int id)
        {
            return DataReader(string.Format("select * from sociedad where IdSociedad = '{0}'", id)).ElementAt(0);
        }
        public IEnumerable<Sociedad> Get()
        {
            return DataReader(string.Format("select * from sociedad"));
        }
        public int GetLastId()
        {
            return dBConnection.GetIntegerValue("select MAX(IdSociedad) from sociedad");
        }
        public void SetConnection(DBConnection dBConnection)
        {
            this.dBConnection = dBConnection;
        }
        private List<Sociedad> DataReader(string SqlStatements)
        {
            System.Data.SqlClient.SqlDataReader Data = dBConnection.GetDataReader(SqlStatements);
            List<Sociedad> Response = new List<Sociedad>();
            while (Data.Read())
            {
                Sociedad sociedad = new Sociedad();
                sociedad.IdSociedad = Data.GetInt32(0);
                sociedad.Descripcion = Data.IsDBNull(1) ? "" : Data.GetString(1);
                sociedad.Direccion = Data.IsDBNull(2) ? "" : Data.GetString(2);
                Response.Add(sociedad);
            }
            return Response;
        }
    }
}
