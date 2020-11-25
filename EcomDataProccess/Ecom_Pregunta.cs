using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EcomDataProccess
{
    public class Ecom_Pregunta
    {
        #region Atributos
        [Display(Name ="No.Pregunta")]
        public int IdPregunta { get; set; }
        [Display(Name = "Creada por")]
        public string NombreCreador { get; set; }
        [Display(Name = "Correo")]
        public string Correo { get; set; }
        [Display(Name = "Titulo")]
        public string Titulo { get; set; }
        [Display(Name = "Categoria")]
        public string IdCategoria { get; set; }
        [Display(Name = "Pregunta")]
        public string Pregunta { get; set; }
        [Display(Name = "Creado")]
        public DateTime Creado { get; set; }
        [Display(Name = "Actualizado")]
        public DateTime Actualizado { get; set; }
        [Display(Name = "Activo")]
        public bool Active { get; set; }
        [Display(Name = "Fue respondida")]
        public bool HasRespuesta { get; set; }
        private Ecom_DBConnection Ecom_DBConnection_;
        public string CategoriaNombre { get; set; }
        [Display(Name = "No.Consultores")]
        public int NumberConsut { get; set; }
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
                Ecom_DBConnection_.AddParameter(Active ? 1 : 0, "Active_", "INT");

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
                            Active = Data.IsDBNull(8) ? false : Data.GetInt32(8) == 1 ? true: false,
                        });
                    }
                    Data.Close();
                    List.ForEach(a => {
                        Ecom_ProductoCategoria ecom_ProductoCategoria = new Ecom_ProductoCategoria(Ecom_DBConnection_);
                        if (ecom_ProductoCategoria.Get(a.IdCategoria))
                        {
                            a.CategoriaNombre = ecom_ProductoCategoria.Description;
                        }
                        else
                        {

                        }
                        Ecom_RespuestaPregunta ecom_RespuestaPregunta = new Ecom_RespuestaPregunta(Ecom_DBConnection_);
                        var respuestas = ecom_RespuestaPregunta.Get(a.IdPregunta);

                        a.HasRespuesta = respuestas.Where(c => c.TipoCreador == "FIBREMEX").ToList().Count > 0 ? true : false;

                       
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
    public enum Ecom_PreguntaActions
    {
        Agregar = 1,
        Update = 2,
        Eliminar = 4,
    }
}
