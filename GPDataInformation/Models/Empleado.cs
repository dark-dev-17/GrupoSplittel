using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;

namespace GPDataInformation.Models
{
    public class Empleado : IDataModel<Empleado>
    {
        public int IdEmpleado { get; set; }
        [Required]
        public int IdPersona { get; set; }
        [Required]
        public int NumeroNomina { get; set; }
        [Required]
        public int TipoNomina { get; set; }
        [Required]
        public int IdSociedad { get; set; }
        [Required]
        public int IdDepartamento { get; set; }
        [Required]
        public int IdPuesto { get; set; }
        [Required]
        public DateTime Ingreso { get; set; }
        [Required]
        public DateTime Egreso { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Extension { get; set; }
        [Required]
        public double Salario { set; get; }
        [Required]
        public int IdEstatus { get; set; }
        public DBConnection dBConnection { get; set; }
        public Empleado()
        {

        }
        public Empleado(DBConnection dBConnection)
        {
            this.dBConnection = dBConnection;
        }

        public bool Add()
        {
            return ActionsObject(EmpleadoActions.Add);
        }

        public bool Update()
        {
            return ActionsObject(EmpleadoActions.Edit);
        }

        public bool Delete()
        {
            return ActionsObject(EmpleadoActions.Delete);
        }

        public int GetLastId()
        {
            return dBConnection.GetIntegerValue("select max(IdEmpleado) from Persona");
        }

        public Empleado Get(int? id)
        {
            List<Empleado> Lista = DataReader(string.Format("select * from Empleado where IdPersona = '{0}'", id));
            if (Lista.Count == 0)
            {
                return null;
            }
            return Lista.ElementAt(0);
        }

        public List<Empleado> Get()
        {
            return DataReader(string.Format("select * from Empleado"));
        }
        private bool ActionsObject(EmpleadoActions actions)
        {
            List<ProcedureModel> procedureModels = new List<ProcedureModel>();
            procedureModels.Add(new ProcedureModel { Namefield = "IdEmpleado", value = IdEmpleado });
            procedureModels.Add(new ProcedureModel { Namefield = "IdPersona", value = IdPersona });
            procedureModels.Add(new ProcedureModel { Namefield = "NumeroNomina", value = NumeroNomina });
            procedureModels.Add(new ProcedureModel { Namefield = "TipoNomina", value = TipoNomina });
            procedureModels.Add(new ProcedureModel { Namefield = "IdSociedad", value = IdSociedad });
            procedureModels.Add(new ProcedureModel { Namefield = "IdDepartamento", value = IdDepartamento });
            procedureModels.Add(new ProcedureModel { Namefield = "IdPuesto", value = IdPuesto });
            procedureModels.Add(new ProcedureModel { Namefield = "Ingreso", value = Ingreso });
            procedureModels.Add(new ProcedureModel { Namefield = "Egreso", value = Egreso });
            procedureModels.Add(new ProcedureModel { Namefield = "Email", value = Email });
            procedureModels.Add(new ProcedureModel { Namefield = "Extension", value = Extension });
            procedureModels.Add(new ProcedureModel { Namefield = "Salario", value = Salario });
            procedureModels.Add(new ProcedureModel { Namefield = "IdEstatus", value = IdEstatus });
            procedureModels.Add(new ProcedureModel { Namefield = "ModeProcedure", value = actions });
            dBConnection.StartProcedure("Gps_Empleado", procedureModels);

            if (dBConnection.ErrorCode == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private List<Empleado> DataReader(string SqlStatements)
        {
            System.Data.SqlClient.SqlDataReader Data = dBConnection.GetDataReader(SqlStatements);
            List<Empleado> Response = new List<Empleado>();
            while (Data.Read())
            {
                Empleado elemento = new Empleado();
                elemento.IdEmpleado = (int)Data.GetValue(Data.GetOrdinal("IdEmpleado"));
                elemento.IdPersona = (int)Data.GetValue(Data.GetOrdinal("IdPersona"));
                elemento.NumeroNomina = (int)Data.GetValue(Data.GetOrdinal("NumeroNomina"));
                elemento.TipoNomina = (int)Data.GetValue(Data.GetOrdinal("TipoNomina"));
                elemento.IdSociedad = (int)Data.GetValue(Data.GetOrdinal("IdSociedad"));
                elemento.IdDepartamento = (int)Data.GetValue(Data.GetOrdinal("IdDepartamento"));
                elemento.IdPuesto = (int)Data.GetValue(Data.GetOrdinal("IdPuesto"));
                elemento.Ingreso = (DateTime)Data.GetValue(Data.GetOrdinal("Ingreso"));
                elemento.Egreso = (DateTime)Data.GetValue(Data.GetOrdinal("Egreso"));
                elemento.Email = (string)Data.GetValue(Data.GetOrdinal("Email"));
                elemento.Extension = (string)Data.GetValue(Data.GetOrdinal("Extension"));
                elemento.Salario = (double)Data.GetValue(Data.GetOrdinal("Salario"));
                elemento.IdEstatus = (int)Data.GetValue(Data.GetOrdinal("IdEstatus"));
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
    public enum EmpleadoActions
    {
        Add = 1,
        Edit = 2,
        Delete = 3
    }
}
