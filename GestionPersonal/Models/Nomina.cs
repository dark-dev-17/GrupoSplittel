
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionPersonal.Models
{
    public class Nomina
    {
        public int IdNomina { get; set; }
        public string RFC { get; set; }
        public DateTime FechaTimbrado { get; set; }
        public DateTime FechaEmision { get; set; }
        public string Folio { get; set; }
        public DateTime FechaInicialPago { get; set; }
        public DateTime FechaFinalPago { get; set; }
        public int NumeroNomina { get; set; }
        public bool AceptadoEmpleado { get; set; }
        public string Comentarios { get; set; }
        public string NombreArchivo { get; set; }
        public double TotalNeto { get; set; }
        private DB_Connection dB_Connection;
        public Nomina()
        {

        }

        public Nomina(DB_Connection dB_Connection)
        {
            this.dB_Connection = dB_Connection;
        }

        public bool Add()
        {
            try
            {
                dB_Connection.StartProcedure("SP_Nomina");
                dB_Connection.AddParameter(IdNomina, "IdNomina", "INT");
                dB_Connection.AddParameter(RFC, "RFC", "VARCHAR");
                dB_Connection.AddParameter(FechaTimbrado, "FechaTimbrado", "DATETIME");
                dB_Connection.AddParameter(FechaEmision, "FechaEmision", "DATETIME");
                dB_Connection.AddParameter(Folio, "Folio", "VARCHAR");
                dB_Connection.AddParameter(FechaInicialPago, "FechaInicialPago", "DATETIME");
                dB_Connection.AddParameter(FechaFinalPago, "FechaFinalPago", "DATETIME");
                dB_Connection.AddParameter(NumeroNomina, "NumeroNomina", "INT");
                dB_Connection.AddParameter(Comentarios, "Comentarios", "VARCHAR");
                dB_Connection.AddParameter(NombreArchivo, "NombreArchivo", "VARCHAR");
                dB_Connection.AddParameter(TotalNeto, "TotalNeto", "DOUBLE");
                dB_Connection.AddParameter((AceptadoEmpleado ? "1" : "0"), "AceptadoEmpleado", "INT");
                dB_Connection.AddParameter(1, "ModeProcedure", "INT");
                int result = dB_Connection.ExecProcedure();
                if (result == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (GPDataInformation.GpExceptions ex)
            {
                throw ex;
            }
        }

        public Nomina Get(string RFC, int Idregisty)
        {
            var List = ReadDatReader(string.Format("SELECT * FROM t36_nominadocumentos where RFC_ = '{0}' and IdNominaDocumentos = '{1}'", RFC, Idregisty));
            if(List.Count == 0)
            {
                return null;
            }
            return List.ElementAt(0);
        }

        public List<Nomina> Get(string RFC)
        {
            return ReadDatReader(string.Format("SELECT * FROM t36_nominadocumentos where RFC_ = '{0}'",RFC));
        }

        public List<Nomina> Get()
        {
            return ReadDatReader(string.Format("SELECT * FROM t36_nominadocumentos;"));
        }

        private List<Nomina> ReadDatReader(string Statement)
        {
            List<Nomina> List = null;
            MySqlDataReader Data = null;
            try
            {
                Data = dB_Connection.DoQuery(Statement);
                List = new List<Nomina>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Nomina
                        {
                            IdNomina = Data.IsDBNull(0) ? 0 : (int)Data.GetUInt32(0),
                            RFC = Data.IsDBNull(1) ? "" : Data.GetString(1),
                            FechaTimbrado = Data.IsDBNull(2) ? DateTime.Now : Data.GetDateTime(2),
                            FechaEmision = Data.IsDBNull(3) ? DateTime.Now : Data.GetDateTime(3),
                            Folio = Data.IsDBNull(4) ? "" : Data.GetString(4),
                            FechaInicialPago = Data.IsDBNull(5) ? DateTime.Now : Data.GetDateTime(5),
                            FechaFinalPago = Data.IsDBNull(6) ? DateTime.Now : Data.GetDateTime(6),
                            NumeroNomina = Data.IsDBNull(7) ? 0 : Data.GetInt32(7),
                            AceptadoEmpleado = Data.IsDBNull(8) ? false : (Data.GetString(8) == "si" ? true : false),
                            Comentarios = Data.IsDBNull(9) ? "" : Data.GetString(9),
                            NombreArchivo = Data.IsDBNull(10) ? "" : Data.GetString(10),
                            TotalNeto = Data.IsDBNull(11) ? 0 : Data.GetDouble(11),
                        });
                    }
                    Data.Close();
                }
                else
                {
                    dB_Connection.Message = "Sin registroso";
                }
                return List;
            }
            catch (GPDataInformation.GpExceptions ex)
            {
                throw ex;
            }
            finally
            {
                if (Data != null)
                {
                    Data.Close();
                }
            }
        }
        public void SetConnection(DB_Connection dB_Connection)
        {
            this.dB_Connection = dB_Connection;
        }
    }
}
