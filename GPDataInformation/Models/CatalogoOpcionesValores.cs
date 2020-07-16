using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPDataInformation.Models
{
    public class CatalogoOpcionesValores:  IDataModel<CatalogoOpcionesValores>
    {
        public int IdCatalogoOpcionesValores { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public int IdCatalogoOpciones { get; set; }
        public DBConnection dBConnection { get; set; }

        public CatalogoOpcionesValores()
        {

        }

        public CatalogoOpcionesValores(DBConnection dBConnection)
        {
            this.dBConnection = dBConnection;
        }

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

        public CatalogoOpcionesValores Get(int? id)
        {
            throw new NotImplementedException();
        }

        public List<CatalogoOpcionesValores> Get(int id)
        {
            return DataReader(string.Format("select * from CatalogoOpcionesValores where IdCatalogoOpciones = '{0}'", id));
        }
        public List<CatalogoOpcionesValores> Get()
        {
            return DataReader(string.Format("select * from CatalogoOpcionesValores"));
        }
        public int GetLastId()
        {
            throw new NotImplementedException();
        }

        private List<CatalogoOpcionesValores> DataReader(string SqlStatements)
        {
            System.Data.SqlClient.SqlDataReader Data = dBConnection.GetDataReader(SqlStatements);
            List<CatalogoOpcionesValores> Response = new List<CatalogoOpcionesValores>();
            while (Data.Read())
            {
                CatalogoOpcionesValores elemento = new CatalogoOpcionesValores();
                elemento.IdCatalogoOpcionesValores = (int)Data.GetValue(Data.GetOrdinal("IdCatalogoOpcionesValores"));
                elemento.Descripcion = (string)Data.GetValue(Data.GetOrdinal("Descripcion"));
                elemento.IdCatalogoOpciones = (int)Data.GetValue(Data.GetOrdinal("IdCatalogoOpciones"));

                Response.Add(elemento);
            }
            Data.Close();
            return Response;
        }

        public void SetConnection(DBConnection dBConnection)
        {
            this.dBConnection = dBConnection;
        }

        
    }
}
