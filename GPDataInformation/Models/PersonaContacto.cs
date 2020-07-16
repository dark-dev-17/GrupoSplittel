using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;

namespace GPDataInformation.Models
{
    public class PersonaContacto : IDataModel<PersonaContacto>
    {
        public int IdPersonaContacto { get; set; }
        [Required]
        public int IdPersona { get; set; }
        [Required]
        public string NombreCompleto { get; set; }
        [Required]
        public int IdParentezco { get; set; }
        [Required]
        public string Telefono { get; set; }
        [Required]
        public string CodigoPostal { get; set; }
        [Required]
        public string Direccion { get; set; }
        public DBConnection dBConnection { get; set; }
        public PersonaContacto()
        {

        }
        public PersonaContacto(DBConnection dBConnection)
        {
            this.dBConnection = dBConnection;
        }

        public bool Add()
        {
            return ActionsObject(PersonaContactoActions.Add);
        }

        public bool Update()
        {
            return ActionsObject(PersonaContactoActions.Edit);
        }

        public bool Delete()
        {
            return ActionsObject(PersonaContactoActions.Delete);
        }

        public int GetLastId()
        {
            return dBConnection.GetIntegerValue("select max(IdInformacionMedica) from PersonaContacto");
        }

        public PersonaContacto Get(int? id)
        {
            List<PersonaContacto> Lista = DataReader(string.Format("select * from PersonaContacto where IdPersonaContacto = '{0}'", id));
            if (Lista.Count == 0)
            {
                return null;
            }
            return Lista.ElementAt(0);
        }

        public List<PersonaContacto> Get()
        {
            return DataReader(string.Format("select * from PersonaContacto"));
        }
        public List<PersonaContacto> GetList(int? id)
        {
            return DataReader(string.Format("select * from PersonaContacto where IdPersona = '{0}'", id));
        }
        private bool ActionsObject(PersonaContactoActions actions)
        {
            List<ProcedureModel> procedureModels = new List<ProcedureModel>();
            procedureModels.Add(new ProcedureModel { Namefield = "IdPersonaContacto ", value = IdPersonaContacto });
            procedureModels.Add(new ProcedureModel { Namefield = "IdPersona ", value = IdPersona });
            procedureModels.Add(new ProcedureModel { Namefield = "NombreCompleto ", value = NombreCompleto });
            procedureModels.Add(new ProcedureModel { Namefield = "IdParentezco ", value = IdParentezco });
            procedureModels.Add(new ProcedureModel { Namefield = "Telefono ", value = Telefono });
            procedureModels.Add(new ProcedureModel { Namefield = "CodigoPostal ", value = CodigoPostal });
            procedureModels.Add(new ProcedureModel { Namefield = "Direccion ", value = Direccion });
            procedureModels.Add(new ProcedureModel { Namefield = "ModeProcedure", value = actions });
            dBConnection.StartProcedure("Gps_PersonaContacto", procedureModels);

            if (dBConnection.ErrorCode == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private List<PersonaContacto> DataReader(string SqlStatements)
        {
            System.Data.SqlClient.SqlDataReader Data = dBConnection.GetDataReader(SqlStatements);
            List<PersonaContacto> Response = new List<PersonaContacto>();
            while (Data.Read())
            {
                PersonaContacto elemento = new PersonaContacto();
                elemento.IdPersonaContacto = (int)Data.GetValue(Data.GetOrdinal("IdPersonaContacto"));
                elemento.IdPersona = (int)Data.GetValue(Data.GetOrdinal("IdPersona"));
                elemento.NombreCompleto = (string)Data.GetValue(Data.GetOrdinal("NombreCompleto"));
                elemento.IdParentezco = (int)Data.GetValue(Data.GetOrdinal("IdParentezco"));
                elemento.Telefono = (string)Data.GetValue(Data.GetOrdinal("Telefono"));
                elemento.CodigoPostal = (string)Data.GetValue(Data.GetOrdinal("CodigoPostal"));
                elemento.Direccion = (string)Data.GetValue(Data.GetOrdinal("Direccion"));
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
    public enum PersonaContactoActions
    {
        Add = 1,
        Edit = 2,
        Delete = 3
    }
}
