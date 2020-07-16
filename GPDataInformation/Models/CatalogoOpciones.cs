using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;

namespace GPDataInformation.Models
{
    public class CatalogoOpciones : IDataModel<CatalogoOpciones>
    {
        public int IdCatalogoOpciones { get; set; }
        [Required]
        public string Descripcion { get; set; }
        public DBConnection dBConnection { get; set; }

        public List<CatalogoOpcionesValores> Opciones { get; internal set; }

        public CatalogoOpciones()
        {

        }

        public CatalogoOpciones(DBConnection dBConnection)
        {
            this.dBConnection = dBConnection;
        }

        public bool Add()
        {
            throw new NotImplementedException();
        }

        public bool Update()
        {
            throw new NotImplementedException();
        }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        public int GetLastId()
        {
            throw new NotImplementedException();
        }

        public CatalogoOpciones Get(int? id)
        {
            List<CatalogoOpciones> Lista = DataReader(string.Format("select * from CatalogoOpciones where IdCatalogoOpciones = '{0}'", id));
            if (Lista.Count == 0)
            {
                return null;
            }
            return Lista.ElementAt(0);
        }

        private List<CatalogoOpciones> DataReader(string SqlStatements)
        {
            System.Data.SqlClient.SqlDataReader Data = dBConnection.GetDataReader(SqlStatements);
            List<CatalogoOpciones> Response = new List<CatalogoOpciones>();
            while (Data.Read())
            {
                CatalogoOpciones elemento = new CatalogoOpciones();
                elemento.Descripcion = (string)Data.GetValue(Data.GetOrdinal("Descripcion"));
                elemento.IdCatalogoOpciones = (int)Data.GetValue(Data.GetOrdinal("IdCatalogoOpciones"));

                Response.Add(elemento);
            }
            Data.Close();
            Response.ForEach(a => a.Opciones = new CatalogoOpcionesValores(dBConnection).Get(a.IdCatalogoOpciones));
            return Response;
        }

        public List<CatalogoOpciones> Get()
        {
            return DataReader(string.Format("select * from CatalogoOpciones"));
        }

        public void SetConnection(DBConnection dBConnection)
        {
            this.dBConnection = dBConnection;
        }
    }
}
