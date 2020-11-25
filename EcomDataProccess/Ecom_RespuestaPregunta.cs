using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcomDataProccess
{
    public class Ecom_RespuestaPregunta
    {
        #region Constructores
        public int IdRespuesta { get; set; }
        public string TipoCreador { get; set; }
        public string Respuesta { get; set; }
        public int IdPregunta { get; set; }
        public int IdConsultor { get; set; }
        public DateTime Creado { get; set; }
        public DateTime Actualizado { get; set; }
        public IFormFile Adjunto { get; set; }
        public string RutaArchivo { get; set; }
        public string NombreConsultor { get; set; }
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_RespuestaPregunta()
        {

        }
        public Ecom_RespuestaPregunta()
        {

        }
        public Ecom_RespuestaPregunta(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Actions(Ecom_RespuestaPreguntaActions Ecom_RespuestaPreguntaActions)
        {
            try
            {
                Ecom_DBConnection_.StartProcedure("Admin_ConsultectPreguntaRespuesta");
                Ecom_DBConnection_.AddParameter(IdRespuesta, "IdRespuesta", "INT");
                Ecom_DBConnection_.AddParameter(IdPregunta, "IdPregunta", "INT");
                Ecom_DBConnection_.AddParameter(IdConsultor, "IdConsultor_", "INT");
                Ecom_DBConnection_.AddParameter(TipoCreador, "TipoCreador", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Respuesta, "Respuesta", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Creado, "Creado", "DATETIME");
                Ecom_DBConnection_.AddParameter(Actualizado, "Actualizado", "DATETIME");
                Ecom_DBConnection_.AddParameter(RutaArchivo, "RutaArchivo", "VARCHAR");

                Ecom_DBConnection_.AddParameter(Ecom_RespuestaPreguntaActions, "ModeProcedure", "INT");
                int result = Ecom_DBConnection_.ExecProcedure();
                if (result == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public List<Ecom_RespuestaPregunta> Get()
        {
            return ReadDatReader(string.Format("SELECT * FROM t42_consultecnico_respuestas"));
        }
        public List<Ecom_RespuestaPregunta> Get(int IdPregunta)
        {
            return ReadDatReader(string.Format("SELECT * FROM t42_consultecnico_respuestas where t41_pk01 = '{0}'", IdPregunta));
        }
        public int LastId()
        {
            return Ecom_DBConnection_.ExecuteScalarInt("SELECT max(t41_pk01) FROM t42_consultecnico_respuestas;");
        }
        public Ecom_RespuestaPregunta GetByid(int respuesta)
        {
            var result = ReadDatReader(string.Format("SELECT * FROM t42_consultecnico_respuestas where t42_pk01 = '{0}'", respuesta));
            return result.Count == 0 ? null : result.ElementAt(0);
        }
        private List<Ecom_RespuestaPregunta> ReadDatReader(string Statement)
        {
            List<Ecom_RespuestaPregunta> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_RespuestaPregunta>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_RespuestaPregunta
                        {
                            IdRespuesta = Data.IsDBNull(0) ? 0 : (int)Data.GetUInt32(0),
                            Respuesta = Data.IsDBNull(1) ? "" : Data.GetString(1),
                            TipoCreador = Data.IsDBNull(2) ? "" : Data.GetString(2),
                            IdPregunta = Data.IsDBNull(3) ? 0 : (int)Data.GetUInt32(3),
                            Creado = Data.IsDBNull(4) ? DateTime.Today : Data.GetDateTime(4),
                            Actualizado = Data.IsDBNull(5) ? DateTime.Today : Data.GetDateTime(5),
                            RutaArchivo = Data.IsDBNull(6) ? "" : Data.GetString(6),
                            IdConsultor = Data.IsDBNull(7) ? 0 : (int)Data.GetUInt32(7),
                        });
                    }
                    Data.Close();

                    List.ForEach(a => { 
                        if(a.TipoCreador == "FIBREMEX")
                        {
                            EcomDataProccess.Foro.Ecom_InternosUser Ecom_Usuario = new Foro.Ecom_InternosUser(Ecom_DBConnection_);
                            var res = Ecom_Usuario.GetConsultore(a.IdConsultor);
                            if(res != null)
                            {
                                a.NombreConsultor = res.NombreCompleto;
                            }
                        }
                    });
                }
                else
                {
                    Ecom_DBConnection_.Message = "Registro no encontrado";
                }
                return List;
            }
            catch (Ecom_Exception ex)
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
        public void SetConnection(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

    }
    public enum Ecom_RespuestaPreguntaActions
    {
        Agregar = 1,
        Update = 2,
        Eliminar = 4,
    }
}