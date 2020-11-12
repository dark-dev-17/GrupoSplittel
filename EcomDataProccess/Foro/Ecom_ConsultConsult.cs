using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EcomDataProccess.Foro
{
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
            if(lista is null)
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
