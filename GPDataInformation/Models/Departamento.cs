using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;

namespace GPDataInformation.Models
{
    public class Departamento : IDataModel<Departamento>
    {
        public int IdDepartamento { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public int IdDireccion { get; set; }
        public DireccionOrganizacional Direccion { get; set; }
        
        private DBConnection dBConnection;
        public Departamento()
        {

        }

        public Departamento(DBConnection dBConnection)
        {
            this.dBConnection = dBConnection;
        }

        public bool Add()
        {
            return ActionsObject(DepartamentoActions.Add);
        }

        public bool Update()
        {
            return ActionsObject(DepartamentoActions.Edit);
        }

        public bool Delete()
        {
            return ActionsObject(DepartamentoActions.Delete);
        }

        private bool ActionsObject(DepartamentoActions actions)
        {
            List<ProcedureModel> procedureModels = new List<ProcedureModel>();
            procedureModels.Add(new ProcedureModel { Namefield = "IdDepartamento", value = IdDepartamento  });
            procedureModels.Add(new ProcedureModel { Namefield = "Nombre", value = Nombre });
            procedureModels.Add(new ProcedureModel { Namefield = "IdDireccion", value = IdDireccion });
            procedureModels.Add(new ProcedureModel { Namefield = "ModeProcedure", value = actions });
            dBConnection.StartProcedure("Gps_Departamento", procedureModels);
            if (dBConnection.ErrorCode == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetLastId()
        {
            return dBConnection.GetIntegerValue("select MAX(IdDepartamento) from Departamento");
        }

        public Departamento Get(int? id)
        {
            List<Departamento> Lista = DataReader(string.Format("select * from Departamento where IdDepartamento = '{0}'", id));
            if (Lista.Count == 0)
            {
                return null;
            }
            return Lista.ElementAt(0);
        }

        public List<Departamento> Get()
        {
            return DataReader(string.Format("select * from Departamento"));
        }

        private List<Departamento> DataReader(string SqlStatements)
        {
            System.Data.SqlClient.SqlDataReader Data = dBConnection.GetDataReader(SqlStatements);
            List<Departamento> Response = new List<Departamento>();
            while (Data.Read())
            {
                Departamento elemento = new Departamento();
                elemento.IdDepartamento = Data.GetInt32(0);
                elemento.Nombre = Data.IsDBNull(1) ? "" : Data.GetString(1);
                elemento.IdDireccion = Data.IsDBNull(2) ? 0 : Data.GetInt32(2);
                Response.Add(elemento);
            }
            Data.Close();
            Response.ForEach(elemento => {
                elemento.Direccion = new DireccionOrganizacional(dBConnection);
                elemento.Direccion = elemento.Direccion.Get(elemento.IdDireccion);
            });

            return Response;
        }

        public void SetConnection(DBConnection dBConnection)
        {
            this.dBConnection = dBConnection;
        }
    }
    public enum DepartamentoActions
    {
        Add = 1,
        Edit = 2,
        Delete = 3
    }
}
