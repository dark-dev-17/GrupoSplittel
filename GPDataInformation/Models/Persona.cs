using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace GPDataInformation.Models
{
    public class Persona : IDataModel<Persona>
    {
        public int IdPersona { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string ApellidoPaterno { get; set; }
        [Required]
        public string ApellidoMaterno { get; set; }
        [Required]
        public DateTime Nacimiento { get; set; }
        [Required]
        public int IdGenero { get; set; }
        [Required]
        public int IdEstadoCivil { get; set; }
        [Required]
        public string RFC { get; set; }
        [Required]
        public string CURP { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string TelefonoPersonal { get; set; }
        [Required]
        public string TelefonoFijo { get; set; }
        [Required]
        public string CodigoPostal { get; set; }
        [Required]
        public string Colonia { get; set; }
        [Required]
        public string Calle { get; set; }
        public int Empleado { get; set; }
        public DBConnection dBConnection { get; set; }
        public Persona()
        {

        }
        public Persona(DBConnection dBConnection)
        {
            this.dBConnection = dBConnection;
        }
        
        public bool Add()
        {
            return ActionsObject(PersonaActions.Add);
        }

        public bool Update()
        {
            return ActionsObject(PersonaActions.Edit);
        }

        public bool Delete()
        {
            return ActionsObject(PersonaActions.Delete);
        }

        public int GetLastId()
        {
            return dBConnection.GetIntegerValue("select max(IdPersona) from Persona");
        }

        public Persona Get(int? id)
        {
            List<Persona> Lista = DataReader(string.Format("select * from Persona where IdPersona = '{0}'", id));
            if (Lista.Count == 0)
            {
                return null;
            }
            return Lista.ElementAt(0);
        }

        public List<Persona> Get()
        {
            return DataReader(string.Format("select * from Persona order by ApellidoPaterno"));
        }
        private bool ActionsObject(PersonaActions actions)
        {
            List<ProcedureModel> procedureModels = new List<ProcedureModel>();
            procedureModels.Add(new ProcedureModel { Namefield = "IdPersona", value = IdPersona });
            procedureModels.Add(new ProcedureModel { Namefield = "Nombre", value = Nombre });
            procedureModels.Add(new ProcedureModel { Namefield = "ApellidoPaterno", value = ApellidoPaterno });
            procedureModels.Add(new ProcedureModel { Namefield = "ApellidoMaterno", value = ApellidoMaterno });
            procedureModels.Add(new ProcedureModel { Namefield = "Nacimiento", value = Nacimiento });
            procedureModels.Add(new ProcedureModel { Namefield = "IdGenero", value = IdGenero });
            procedureModels.Add(new ProcedureModel { Namefield = "IdEstadoCivil", value = IdEstadoCivil });
            procedureModels.Add(new ProcedureModel { Namefield = "RFC", value = RFC });
            procedureModels.Add(new ProcedureModel { Namefield = "CURP", value = CURP });
            procedureModels.Add(new ProcedureModel { Namefield = "Email", value = Email });
            procedureModels.Add(new ProcedureModel { Namefield = "TelefonoPersonal", value = TelefonoPersonal });
            procedureModels.Add(new ProcedureModel { Namefield = "TelefonoFijo", value = TelefonoFijo });
            procedureModels.Add(new ProcedureModel { Namefield = "CodigoPostal", value = CodigoPostal });
            procedureModels.Add(new ProcedureModel { Namefield = "Colonia", value = Colonia });
            procedureModels.Add(new ProcedureModel { Namefield = "Calle", value = Calle });
            procedureModels.Add(new ProcedureModel { Namefield = "Empleado", value = Empleado });
            procedureModels.Add(new ProcedureModel { Namefield = "ModeProcedure", value = actions });
            dBConnection.StartProcedure("Gps_Persona", procedureModels);

            if (dBConnection.ErrorCode == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private List<Persona> DataReader(string SqlStatements)
        {
            System.Data.SqlClient.SqlDataReader Data = dBConnection.GetDataReader(SqlStatements);
            List<Persona> Response = new List<Persona>();
            while (Data.Read())
            {
                Persona elemento = new Persona();
                elemento.IdPersona = (int)Data.GetValue(Data.GetOrdinal("IdPersona"));
                elemento.Nombre = (string)Data.GetValue(Data.GetOrdinal("Nombre"));
                elemento.ApellidoPaterno = (string)Data.GetValue(Data.GetOrdinal("ApellidoPaterno"));
                elemento.ApellidoMaterno = (string)Data.GetValue(Data.GetOrdinal("ApellidoMaterno"));
                elemento.Nacimiento = (DateTime)Data.GetValue(Data.GetOrdinal("Nacimiento"));
                elemento.IdGenero = (int)Data.GetValue(Data.GetOrdinal("IdGenero"));
                elemento.IdEstadoCivil = (int)Data.GetValue(Data.GetOrdinal("IdEstadoCivil"));
                elemento.RFC = (string)Data.GetValue(Data.GetOrdinal("RFC"));
                elemento.CURP = (string)Data.GetValue(Data.GetOrdinal("CURP"));
                elemento.Email = (string)Data.GetValue(Data.GetOrdinal("Email"));
                elemento.TelefonoPersonal = (string)Data.GetValue(Data.GetOrdinal("TelefonoPersonal"));
                elemento.TelefonoFijo = (string)Data.GetValue(Data.GetOrdinal("TelefonoFijo"));
                elemento.CodigoPostal = (string)Data.GetValue(Data.GetOrdinal("CodigoPostal"));
                elemento.Colonia = (string)Data.GetValue(Data.GetOrdinal("Colonia"));
                elemento.Calle = (string)Data.GetValue(Data.GetOrdinal("Calle"));
                elemento.Empleado = (int)Data.GetValue(Data.GetOrdinal("Empleado"));
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
    public enum PersonaActions
    {
        Add = 1,
        Edit = 2,
        Delete = 3
    }
}
