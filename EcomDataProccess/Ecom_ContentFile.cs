using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcomDataProccess
{
    public class Ecom_ContentFile
    {
        #region Propiedades
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Seccion")]
        [Required]
        public int IdTipoContenido { get; set; }
        [Display(Name = "Ruta de archivo")]
        public string PathFile { get; set; }
        [Display(Name = "Descripcion web")]
        public string DescripcionWeb { get; set; }
        [Required]
        [Display(Name = "Url")]
        public string Url { get; set; }
        [Display(Name = "Posicion")]
        public int Position { get; set; }
        [Display(Name = "Visible")]
        public bool IsVisible { get; set; }
        [Display(Name = "Abrir en pestaña nueva")]
        public bool OpenNewTab { get; set; }
        [Display(Name = "Archivo")]
        public IFormFile Imagen { get; set; }

        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_ContentFile()
        {
            
        }
        public Ecom_ContentFile()
        {

        }
        public Ecom_ContentFile(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Add()
        {
            try
            {
                return Action(Ecom_ContentFileActions.Add);
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
                return Action(Ecom_ContentFileActions.Edit);
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
                return Action(Ecom_ContentFileActions.Delete);
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
                return Action(Ecom_ContentFileActions.UpdatePosition);
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
                return Ecom_DBConnection_.ExecuteScalarInt("select max(t39_pk01) from t39_FileSecciones");
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        private bool Action(Ecom_ContentFileActions ecom_ContentFileActions)
        {
            try
            {
                Ecom_DBConnection_.StartProcedure("Admin_ContentFile");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(PathFile, "PathFile", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Url, "Url", "VARCHAR");
                Ecom_DBConnection_.AddParameter(DescripcionWeb, "DescripcionWeb", "VARCHAR");
                Ecom_DBConnection_.AddParameter(IdTipoContenido, "IdTipoContenido", "INT");
                Ecom_DBConnection_.AddParameter(Position, "Position", "INT");
                Ecom_DBConnection_.AddParameter(IsVisible ? 1 : 0, "IsVisible", "INT");
                Ecom_DBConnection_.AddParameter(OpenNewTab ? 1 : 0, "OpenNewTab", "INT");
                Ecom_DBConnection_.AddParameter(ecom_ContentFileActions, "ModeProcedure", "INT");
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
            List<Ecom_ContentFile> List = ReadDatReader(string.Format("select * from t39_FileSecciones where t39_pk01 = '{0}'", id));
            if (List.Count > 0)
            {
                List.ForEach(item => {
                    Id = item.Id;
                    IdTipoContenido = item.IdTipoContenido;
                    PathFile = item.PathFile;
                    DescripcionWeb = item.DescripcionWeb;
                    Position = item.Position;
                    IsVisible = item.IsVisible;
                    Url = item.Url;
                    OpenNewTab = item.OpenNewTab;
                });
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<Ecom_ContentFile> GetContent(int id)
        {
            return  ReadDatReader(string.Format("select * from t39_FileSecciones where t38_pk01 = '{0}'", id));
        }
        private List<Ecom_ContentFile> ReadDatReader(string Statement)
        {
            List<Ecom_ContentFile> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_ContentFile>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_ContentFile
                        {
                            Id = Data.IsDBNull(0) ? 0 : (int) Data.GetUInt32(0),
                            PathFile = Data.IsDBNull(1) ? "" :  Data.GetString(1),
                            DescripcionWeb = Data.IsDBNull(2) ? "" :  Data.GetString(2),
                            IdTipoContenido = Data.IsDBNull(3) ? 0 :  Data.GetInt32(3),
                            Position = Data.IsDBNull(4) ? 0 :  Data.GetInt32(4),
                            IsVisible = Data.IsDBNull(5) ? false :  (Data.GetInt32(5) == 1 ? true: false),
                            Url = Data.IsDBNull(6) ? "" : Data.GetString(6),
                            OpenNewTab = Data.IsDBNull(7) ? false : (Data.GetInt32(7) == 1 ? true : false),
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
    public enum Ecom_ContentFileActions
    {
        Add = 1,
        Edit = 2,
        Delete = 3,
        UpdatePosition=4
    }
}
