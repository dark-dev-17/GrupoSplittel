using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcomDataProccess
{
    public class Ecom_ContentFileType
    {
        #region Propiedades
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }
        [Required]
        [Display(Name = "Path de archivos")]
        public string RuteEcommerce { get; set; }
        public List<Ecom_ContentFile> ecom_ContentFiles { get; set; }

        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_ContentFileType()
        {
            
        }
        public Ecom_ContentFileType()
        {

        }
        public Ecom_ContentFileType(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Add()
        {
            try
            {
                return Action(Ecom_ContentFileTypeActions.Add);
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public bool Edit()
        {
            try
            {
                return Action(Ecom_ContentFileTypeActions.Add);
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public bool Delete()
        {
            try
            {
                return Action(Ecom_ContentFileTypeActions.Add);
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public bool UpdatePosition()
        {
            try
            {
                return Action(Ecom_ContentFileTypeActions.UpdatePosition);
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public int LastId()
        {
            try
            {
                return Ecom_DBConnection_.ExecuteScalarInt("select max(t38_pk01) from t38_FilesType");
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        private bool Action(Ecom_ContentFileTypeActions Ecom_ContentFileTypeActions)
        {
            try
            {
                Ecom_DBConnection_.StartProcedure("Admin_ContentFileType");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(Descripcion, "Descripcion", "VARCHAR");
                Ecom_DBConnection_.AddParameter(RuteEcommerce, "RuteEcommerce", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Ecom_ContentFileTypeActions, "ModeProcedure", "INT");
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
        public bool Get(int id)
        {
            List<Ecom_ContentFileType> List = ReadDatReader(string.Format("select * from t38_FilesType where t38_pk01 = '{0}'", id));
            if (List.Count > 0)
            {
                List.ForEach(item => {
                    Id = item.Id;
                    Descripcion = item.Descripcion;
                    RuteEcommerce = item.RuteEcommerce;
                });
                ecom_ContentFiles = new Ecom_ContentFile(Ecom_DBConnection_).GetContent(Id);
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<Ecom_ContentFileType> Get()
        {
            return ReadDatReader(string.Format("select * from t38_FilesType"));
        }
        private List<Ecom_ContentFileType> ReadDatReader(string Statement)
        {
            List<Ecom_ContentFileType> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_ContentFileType>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_ContentFileType
                        {
                            Id = Data.IsDBNull(0) ? 0 : (int) Data.GetUInt32(0),
                            Descripcion = Data.IsDBNull(1) ? "" :  Data.GetString(1),
                            RuteEcommerce = Data.IsDBNull(2) ? "" :  Data.GetString(2),
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
    public enum Ecom_ContentFileTypeActions
    {
        Add = 1,
        Edit = 2,
        Delete = 3,
        UpdatePosition=4
    }
}
