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
        private DBConnection dBConnection;
        public Sociedad()
        {

        }
        public Sociedad(DBConnection dBConnection)
        {
            this.dBConnection = dBConnection;
        }
        public bool Add()
        {
            return ActionsObject(SociedadActions.Add);
        }
        public bool Delete()
        {
            return ActionsObject(SociedadActions.Delete);
        }
        public bool Update()
        {
            return ActionsObject(SociedadActions.Edit);
        }
        private bool ActionsObject(SociedadActions actions)
        {
            List<ProcedureModel> procedureModels = new List<ProcedureModel>();
            procedureModels.Add(new ProcedureModel { Namefield = "IdSociedad", value = IdSociedad });
            procedureModels.Add(new ProcedureModel { Namefield = "Descripcion", value = Descripcion });
            procedureModels.Add(new ProcedureModel { Namefield = "Direccion", value = Direccion });
            procedureModels.Add(new ProcedureModel { Namefield = "ModeProcedure", value = actions });
            dBConnection.StartProcedure("Gps_Sociedad", procedureModels);
            if(dBConnection.ErrorCode == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public  Sociedad Get(int id)
        {
            return DataReader(string.Format("select * from sociedad where IdSociedad = '{0}'", id)).ElementAt(0);
        }
        public List<Sociedad> Get()
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
            Data.Close();
            return Response;
        }
    }
    public enum SociedadActions
    {
        Add = 1,
        Edit = 2,
        Delete = 3
    }
}
