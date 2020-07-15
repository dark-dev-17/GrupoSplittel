using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;

namespace GPDataInformation.Models
{
    public class DireccionOrganizacional :IDataModel<DireccionOrganizacional>
    {
        public int IdDireccion { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        [Display(Name = "Sociedad")]
        public int IdSociedad { get; set; }
        public Sociedad Sociedad { get; set; }
        [Display(Name = "Direccion parent")]
        public int DireccionParent { get; set; }
        public DireccionOrganizacional DireccionPa { get; set; }
        private DBConnection dBConnection;
        public DireccionOrganizacional()
        {

        }
        public DireccionOrganizacional(DBConnection dBConnection)
        {
            this.dBConnection = dBConnection;
        }
        public bool Add()
        {
            return ActionsObject(DireccionOrganizacionalActions.Add);
        }
        public bool Delete()
        {
            return ActionsObject(DireccionOrganizacionalActions.Delete);
        }
        public DireccionOrganizacional Get(int? id)
        {
            List<DireccionOrganizacional> Lista = DataReader(string.Format("select * from Direccion where IdDireccion = '{0}'", id));
            if(Lista.Count == 0)
            {
                return null;
            }
            return Lista.ElementAt(0);
        }
        public bool Update()
        {
            return ActionsObject(DireccionOrganizacionalActions.Edit);
        }
        private bool ActionsObject(DireccionOrganizacionalActions actions)
        {
            List<ProcedureModel> procedureModels = new List<ProcedureModel>();
            procedureModels.Add(new ProcedureModel { Namefield = "IdDireccion", value = IdDireccion });
            procedureModels.Add(new ProcedureModel { Namefield = "Nombre", value = Nombre });
            procedureModels.Add(new ProcedureModel { Namefield = "IdSociedad", value = IdSociedad });
            procedureModels.Add(new ProcedureModel { Namefield = "DireccionParent", value = DireccionParent });
            procedureModels.Add(new ProcedureModel { Namefield = "ModeProcedure", value = actions });
            dBConnection.StartProcedure("Gps_DireccionOrganizacional", procedureModels);
            if (dBConnection.ErrorCode == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<DireccionOrganizacional> Get()
        {
            return DataReader(string.Format("select * from Direccion order by Nombre"));
        }

        public int GetLastId()
        {
            return dBConnection.GetIntegerValue("select max(IdDireccion) from Direccion");
        }
        private List<DireccionOrganizacional> DataReader(string SqlStatements)
        {
            System.Data.SqlClient.SqlDataReader Data = dBConnection.GetDataReader(SqlStatements);
            List<DireccionOrganizacional> Response = new List<DireccionOrganizacional>();
            while (Data.Read())
            {
                DireccionOrganizacional elemento = new DireccionOrganizacional();
                elemento.IdDireccion = Data.GetInt32(0);
                elemento.Nombre = Data.IsDBNull(1) ? "" : Data.GetString(1);
                elemento.IdSociedad = Data.IsDBNull(2) ? 0 : Data.GetInt32(2);
                elemento.DireccionParent = Data.IsDBNull(3) ? 0 : Data.GetInt32(3);
                Response.Add(elemento);
            }
            Data.Close();
            Response.ForEach(elemento => {
                elemento.Sociedad = new Sociedad(dBConnection);
                elemento.Sociedad = elemento.Sociedad.Get(elemento.IdSociedad);
            });

            return Response;
        }
        public void SetConnection(DBConnection dBConnection)
        {
            this.dBConnection = dBConnection;
        }

        
    }

    public enum DireccionOrganizacionalActions
    {
        Add = 1,
        Edit = 2,
        Delete = 3
    }
}
