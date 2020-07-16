using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;

namespace GPDataInformation.Models
{
    public class Puesto :IDataModel<Puesto>
    {
        public int IdPuesto { get; set; }
        [Required]
        public string DPU { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string DescripcionPuesto { get; set; }
        [Required]
        public int IdDepartamento { get; set; }
        [Required]
        public double SalarioMin { set; get; }
        [Required]
        public double SalarioMax { set; get; }
        [Required]
        public TimeSpan HoraEntrada { get; set; }
        [Required]
        public TimeSpan HoraSalida { get; set; }
        [Required]
        public int IdUbicacion { get; set; }
        public int? IdPuestoParent { get; set; }
        private DBConnection dBConnection;
        public Departamento Departamento { get; internal set; }
        public CatalogoOpcionesValores Ubicacion { get; internal set; }
        public Puesto PuestoParent { get; internal set; }

        public Puesto()
        {

        }

        public Puesto(DBConnection dBConnection)
        {
            this.dBConnection = dBConnection;
        }

        public bool Add()
        {
            return ActionsObject(PuestoActions.Add);
        }

        public bool Delete()
        {
            return ActionsObject(PuestoActions.Delete);
        }

        public bool Update()
        {
            return ActionsObject(PuestoActions.Edit);
        }

        private bool ActionsObject(PuestoActions actions)
        {
            List<ProcedureModel> procedureModels = new List<ProcedureModel>();
            procedureModels.Add(new ProcedureModel { Namefield = "IdPuesto", value = IdPuesto });
            procedureModels.Add(new ProcedureModel { Namefield = "DPU", value = DPU });
            procedureModels.Add(new ProcedureModel { Namefield = "Nombre", value = Nombre });
            procedureModels.Add(new ProcedureModel { Namefield = "DescripcionPuesto", value = DescripcionPuesto });
            procedureModels.Add(new ProcedureModel { Namefield = "IdDepartamento", value = IdDepartamento });
            procedureModels.Add(new ProcedureModel { Namefield = "SalarioMin", value = SalarioMin });
            procedureModels.Add(new ProcedureModel { Namefield = "SalarioMax", value = SalarioMax });
            procedureModels.Add(new ProcedureModel { Namefield = "HoraEntrada", value = HoraEntrada });
            procedureModels.Add(new ProcedureModel { Namefield = "HoraSalida", value = HoraSalida });
            procedureModels.Add(new ProcedureModel { Namefield = "IdUbicacion", value = IdUbicacion });
            procedureModels.Add(new ProcedureModel { Namefield = "IdPuestoParent", value = IdPuestoParent });
            procedureModels.Add(new ProcedureModel { Namefield = "ModeProcedure", value = actions });
            dBConnection.StartProcedure("Gps_Puesto", procedureModels);

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
            return dBConnection.GetIntegerValue("select MAX(IdPuesto) from Puesto");
        }

        public Puesto Get(int? id)
        {
            List<Puesto> Lista = DataReader(string.Format("select * from Puesto where IdPuesto = '{0}'", id));
            if (Lista.Count == 0)
            {
                return null;
            }
            return Lista.ElementAt(0);
        }

        public List<Puesto> Get()
        {
            return DataReader(string.Format("select * from Puesto"));
        }

        private List<Puesto> DataReader(string SqlStatements)
        {
            System.Data.SqlClient.SqlDataReader Data = dBConnection.GetDataReader(SqlStatements);
            List<Puesto> Response = new List<Puesto>();
            while (Data.Read())
            {
                Puesto elemento = new Puesto();
                elemento.IdPuesto = (int)Data.GetValue(Data.GetOrdinal("IdPuesto"));
                elemento.DPU = (string)Data.GetValue(Data.GetOrdinal("DPU"));
                elemento.Nombre = (string)Data.GetValue(Data.GetOrdinal("Nombre"));
                elemento.DescripcionPuesto = (string)Data.GetValue(Data.GetOrdinal("DescripcionPuesto"));
                elemento.IdDepartamento = (int)Data.GetValue(Data.GetOrdinal("IdDepartamento"));
                elemento.SalarioMin = (double)Data.GetValue(Data.GetOrdinal("SalarioMin"));
                elemento.SalarioMax = (double)Data.GetValue(Data.GetOrdinal("SalarioMax"));
                elemento.HoraEntrada = (TimeSpan)Data.GetValue(Data.GetOrdinal("HoraEntrada"));
                elemento.HoraSalida = (TimeSpan)Data.GetValue(Data.GetOrdinal("HoraSalida"));
                elemento.IdUbicacion = (int)Data.GetValue(Data.GetOrdinal("IdUbicacion"));
                elemento.IdPuestoParent = Data.GetValue(Data.GetOrdinal("IdPuestoParent")) is System.DBNull ? 0 : (int?)Data.GetValue(Data.GetOrdinal("IdPuestoParent"));
                
                Response.Add(elemento);
            }
            Data.Close();

            Response.ForEach(elemento =>
            {
                elemento.Departamento = new Departamento(dBConnection).Get(elemento.IdDepartamento);
            });
            Response.ForEach(elemento =>
            {
                elemento.PuestoParent = Response.Find(a => a.IdPuesto == elemento.IdPuestoParent);
            });
            return Response;
        }

        public void SetConnection(DBConnection dBConnection)
        {
            this.dBConnection = dBConnection;
        }

        
    }
    public enum PuestoActions
    {
        Add = 1,
        Edit = 2,
        Delete = 3
    }
}
