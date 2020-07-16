using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;

namespace GPDataInformation.Models
{
    public class InformacionMedica: IDataModel<InformacionMedica>
    {
        public int IdInformacionMedica { get; set; }
        [Required]
        public int IdPersona { get; set; }
        [Required]
        public int TipoSangre { get; set; }
        [Required]
        public int Alergias { get; set; }
        [Required]
        public double Altura { set; get; }
        [Required]
        public double Peso { set; get; }
        [Required]
        public double Talla { set; get; }
        [Required]
        public double IMC { set; get; }
        [Required]
        public string Comentarios { get; set; }

        public DBConnection dBConnection { get; set; }
        public InformacionMedica()
        {

        }
        public InformacionMedica(DBConnection dBConnection)
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
            return dBConnection.GetIntegerValue("select max(IdInformacionMedica) from InformacionMedica");
        }

        public InformacionMedica Get(int? id)
        {
            List<InformacionMedica> Lista = DataReader(string.Format("select * from InformacionMedica where IdPersona = '{0}'", id));
            if (Lista.Count == 0)
            {
                return null;
            }
            return Lista.ElementAt(0);
        }

        public List<InformacionMedica> Get()
        {
            return DataReader(string.Format("select * from InformacionMedica"));
        }

        public void SetConnection(DBConnection dBConnection)
        {
            this.dBConnection = dBConnection;
        }
        private bool ActionsObject(PersonaContactoActions actions)
        {
            List<ProcedureModel> procedureModels = new List<ProcedureModel>();
            procedureModels.Add(new ProcedureModel { Namefield = "IdInformacionMedica", value = IdInformacionMedica });
            procedureModels.Add(new ProcedureModel { Namefield = "IdPersona", value = IdPersona });
            procedureModels.Add(new ProcedureModel { Namefield = "TipoSangre", value = TipoSangre });
            procedureModels.Add(new ProcedureModel { Namefield = "Alergias", value = Alergias });
            procedureModels.Add(new ProcedureModel { Namefield = "Altura", value = Altura });
            procedureModels.Add(new ProcedureModel { Namefield = "Peso", value = Peso });
            procedureModels.Add(new ProcedureModel { Namefield = "Talla", value = Talla });
            procedureModels.Add(new ProcedureModel { Namefield = "IMC", value = IMC });
            procedureModels.Add(new ProcedureModel { Namefield = "Comentarios", value = Comentarios });
            procedureModels.Add(new ProcedureModel { Namefield = "ModeProcedure", value = actions });
            dBConnection.StartProcedure("Gps_InformacionMedica", procedureModels);

            if (dBConnection.ErrorCode == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private List<InformacionMedica> DataReader(string SqlStatements)
        {
            System.Data.SqlClient.SqlDataReader Data = dBConnection.GetDataReader(SqlStatements);
            List<InformacionMedica> Response = new List<InformacionMedica>();
            while (Data.Read())
            {
                InformacionMedica elemento = new InformacionMedica();
                elemento.IdInformacionMedica = (int)Data.GetValue(Data.GetOrdinal("IdInformacionMedica"));
                elemento.IdPersona = (int)Data.GetValue(Data.GetOrdinal("IdPersona"));
                elemento.TipoSangre = (int)Data.GetValue(Data.GetOrdinal("TipoSangre"));
                elemento.Alergias = (int)Data.GetValue(Data.GetOrdinal("Alergias"));
                elemento.Altura = (double)Data.GetValue(Data.GetOrdinal("Altura"));
                elemento.Peso = (double)Data.GetValue(Data.GetOrdinal("Peso"));
                elemento.Talla = (double)Data.GetValue(Data.GetOrdinal("Talla"));
                elemento.IMC = (double)Data.GetValue(Data.GetOrdinal("IMC"));
                elemento.Comentarios = (string)Data.GetValue(Data.GetOrdinal("Comentarios"));
                Response.Add(elemento);
            }
            Data.Close();
            return Response;
        }
    }
    public enum InformacionMedicaActions
    {
        Add = 1,
        Edit = 2,
        Delete = 3
    }
}
