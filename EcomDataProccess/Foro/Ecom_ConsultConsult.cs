using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EcomDataProccess.Foro
{
    public class Ecom_InternosUser
    {
        #region Atributos
        [Display(Name = "Id")]
        public int IdInternosUser { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Required]
        [Display(Name = "Imagen")]
        public string Imagen { get; set; }

        [Required]
        [Display(Name = "IdSplitnet")]
        public int IdSplitnet { get; set; }


        [Required]
        [Display(Name = "Correo")]
        public string Correo { get; set; }

        public string NombreCompleto { get { return Nombre + " " + Apellido; } }

        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructor
        ~Ecom_InternosUser()
        {

        }
        public Ecom_InternosUser()
        {

        }
        public Ecom_InternosUser(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public Ecom_InternosUser GetConsultore(int ID)
        {
            List<Ecom_InternosUser> result = ReadDatReader(String.Format("SELECT * FROM t43_usuarios_internos WHERE IdSplitnet = '{0}'",ID));

            if(result.Count == 1)
            {
                return result.ElementAt(0);
            }
            else
            {
                return null;
            }
        }
        public List<Ecom_InternosUser> GetConsultores()
        {
            var result = ReadDatReader("SELECT * FROM t43_usuarios_internos;");
            return result;
        }
        private List<Ecom_InternosUser> ReadDatReader(string Statement)
        {
            List<Ecom_InternosUser> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_InternosUser>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_InternosUser
                        {
                            IdInternosUser = Data.IsDBNull(0) ? 0 : (int)Data.GetUInt32(0),
                            Nombre = Data.IsDBNull(1) ? "" : Data.GetString(1),
                            Apellido = Data.IsDBNull(2) ? "" : Data.GetString(2),
                            Imagen = Data.IsDBNull(3) ? "" : Data.GetString(3),
                            IdSplitnet = Data.IsDBNull(4) ? 0 : Data.GetInt32(4),
                            Correo = Data.IsDBNull(5) ? "" : Data.GetString(5),
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




    public class Ecom_ConsultConsult
    {
        #region Atributos
        [Display(Name = "Id")]
        public int IdConsultConsult { get; set; }
        [Required]
        [Display(Name = "Pregunta")]
        public int IdPregunta { get; set; }
        [Required]
        [Display(Name = "Consultor")]
        public int IdConsultor { get; set; }

        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructor
        ~Ecom_ConsultConsult()
        {

        }
        public Ecom_ConsultConsult()
        {

        }
        public Ecom_ConsultConsult(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public List<int> GetConsultores(int IdPregunta_)
        {
            List<int> lista = new List<int>();
            ReadDatReader(string.Format("select * from consultor_consultor where IdPregunta = '{0}'", IdPregunta_)).ForEach(a => {
                lista.Add(a.IdConsultor);
            });
            return lista;
        }
        public List<int> getByConsultorr(int IdConsultor_)
        {
            List<int> lista = new List<int>();
            ReadDatReader(string.Format("select * from consultor_consultor where IdConsultor = '{0}'", IdConsultor_)).ForEach(a => {
                lista.Add(a.IdPregunta);
            });
            return lista;
        }
        public void Agregar(List<int> lista, int IdPregunta_)
        {
            Ecom_DBConnection_.StartTransaction();
            try
            {
                if (lista is null)
                {

                }
                else
                {
                    ReadDatReader(string.Format("select * from consultor_consultor where IdPregunta = '{0}'", IdPregunta_)).ForEach(a => {
                        Delete(a.IdConsultConsult);
                    });

                    lista.ForEach(a => {
                        IdPregunta = IdPregunta_;
                        IdConsultor = a;
                        Add();
                    });
                }
                Ecom_DBConnection_.Commit();
            }
            catch (Ecom_Exception ex)
            {
                Ecom_DBConnection_.RolBack();
                throw ex;
            }
            
        }
        private void Delete(int IdConsultConsult_)
        {
            string statement = string.Format("DELETE FROM consultor_consultor WHERE IdConsultor_consultor = @IdConsultor_consultor");
            List<ProcedureModel> procedureModels = new List<ProcedureModel>();
            procedureModels.Add(new ProcedureModel { Namefield = "@IdConsultor_consultor", value = "" + IdConsultConsult_ });
            Ecom_DBConnection_.StartDelete(statement, procedureModels);
        }
        private void Add()
        {
            string statement = string.Format("INSERT INTO consultor_consultor(IdConsultor_consultor,IdPregunta,IdConsultor) VALUES(NULL,@IdPregunta,@IdConsultor)");
            List<ProcedureModel> procedureModels = new List<ProcedureModel>();
            procedureModels.Add(new ProcedureModel { Namefield = "@IdPregunta", value = "" + IdPregunta });
            procedureModels.Add(new ProcedureModel { Namefield = "@IdConsultor", value = "" + IdConsultor });
            Ecom_DBConnection_.StartInsert(statement, procedureModels);
        }
        private List<Ecom_ConsultConsult> ReadDatReader(string Statement)
        {
            List<Ecom_ConsultConsult> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_ConsultConsult>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_ConsultConsult
                        {
                            IdConsultConsult = Data.IsDBNull(0) ? 0 : (int)Data.GetUInt32(0),
                            IdPregunta = Data.IsDBNull(1) ? 0 : Data.GetInt32(1),
                            IdConsultor = Data.IsDBNull(2) ? 0 : Data.GetInt32(2),
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
}
