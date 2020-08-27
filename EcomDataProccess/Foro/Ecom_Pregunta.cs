using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcomDataProccess.Foro
{
    public class Ecom_Pregunta
    {
        #region Atributos
        public int IdPregunta { get; set; }
        public string NombreCreador { get; set; }
        public string Correo { get; set; }
        public string Titulo { get; set; }
        public string IdCategoria { get; set; }
        public string Pregunta { get; set; }
        public DateTime Creado { get; set; }
        public DateTime Actualizado { get; set; }
        private Ecom_DBConnection Ecom_DBConnection_;
        public string CategoriaNombre { get; set; }
        #endregion

        #region Constructores
        ~Ecom_Pregunta()
        {

        }
        public Ecom_Pregunta()
        {

        }
        public Ecom_Pregunta(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Actions(Ecom_PreguntaActions ecom_PreguntaActions)
        {
            try
            {
                Ecom_DBConnection_.StartProcedure("Admin_ConsultectPregunta");
                Ecom_DBConnection_.AddParameter(IdPregunta, "IdPregunta", "INT");
                Ecom_DBConnection_.AddParameter(NombreCreador, "NombreCreador", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Correo, "Correo", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Titulo, "Titulo", "VARCHAR");
                Ecom_DBConnection_.AddParameter(IdCategoria, "IdCategoria", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Pregunta, "Pregunta", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Creado, "Creado", "DATETIME");
                Ecom_DBConnection_.AddParameter(Actualizado, "Actualizado", "DATETIME");
                
                Ecom_DBConnection_.AddParameter(ecom_PreguntaActions, "ModeProcedure", "INT");
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
        public List<Ecom_Pregunta> Get()
        {
            return ReadDatReader(string.Format("SELECT * FROM t41_consultecnico_pregunta"));
        }
        public Ecom_Pregunta Get(int IdPregunta)
        {
            var result = ReadDatReader(string.Format("SELECT * FROM t41_consultecnico_pregunta where t41_pk01 = '{0}'", IdPregunta));
            return result.Count == 0 ? null : result.ElementAt(0);
        }
        private List<Ecom_Pregunta> ReadDatReader(string Statement)
        {
            List<Ecom_Pregunta> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_Pregunta>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_Pregunta
                        {
                            IdPregunta = Data.IsDBNull(0) ? 0 : (int)Data.GetUInt32(0),
                            NombreCreador = Data.IsDBNull(1) ? "" : Data.GetString(1),
                            Correo = Data.IsDBNull(2) ? "" : Data.GetString(2),
                            Titulo = Data.IsDBNull(3) ? "" : Data.GetString(3),
                            IdCategoria = Data.IsDBNull(4) ? "" : Data.GetString(4),
                            Pregunta = Data.IsDBNull(5) ? "" : Data.GetString(5),
                            Creado = Data.IsDBNull(6) ? DateTime.Today : Data.GetDateTime(6),
                            Actualizado = Data.IsDBNull(7) ? DateTime.Today : Data.GetDateTime(7),
                        }); 
                    }
                    Data.Close();
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
    public enum Ecom_PreguntaActions
    {
        Agregar = 1,
        Update = 2,
        Eliminar = 4,
    }
}
