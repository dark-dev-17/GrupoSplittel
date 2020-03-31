using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcomDataProccess
{
    public class Ecom_Blog
    {
        #region Propiedades
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Titulo caratula")]
        [Required]
        public string Title { get; set; }
        [Display(Name = "Titulo contenido")]
        [Required]
        public string Titlelargo { get; set; }
        [Display(Name = "Contenido caratura")]
        [MaxLength(110, ErrorMessage = "Description cannot be longer than 110 characters.")]
        [Required]
        public string ContentShort { get; set; }
        [Display(Name = "Contenido completo")]
        [Required]
        public string Contentlarge { get; set; }
        [Display(Name = "Posicionamiento web")]
        [Required]
        public string Comillas { get; set; }
        [Display(Name = "Imagen caratura")]
        
        public string ImageCoverPage { get; set; }
        [Display(Name = "Imagen blog")]
        
        public string ImageBlog { get; set; }
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime DateBlog { get; set; }
        [Display(Name = "Visible en E-commerce")]
        [Required]
        public bool IsActiveEcommerce { get; set; }
        [Display(Name = "Caratula")]
    
        public IFormFile BlogCover { get; set; }
        [Display(Name = "Landing")]
        public IFormFile BlogImage { get; set; }
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_Blog()
        {
            
        }
        public Ecom_Blog()
        {

        }
        public Ecom_Blog(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Add()
        {
            try
            {
                Ecom_DBConnection_.StartProcedure("Admin_blog");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(Title, "Title", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Titlelargo, "Titlelargo", "VARCHAR");
                Ecom_DBConnection_.AddParameter(ContentShort, "ContentShort", "TEXT");
                Ecom_DBConnection_.AddParameter(Contentlarge, "Contentlarge", "TEXT");
                Ecom_DBConnection_.AddParameter(Comillas, "Comillass", "VARCHAR");
                Ecom_DBConnection_.AddParameter(ImageCoverPage, "ImageCoverPage", "VARCHAR");
                Ecom_DBConnection_.AddParameter(ImageBlog, "ImageBlog", "VARCHAR");
                Ecom_DBConnection_.AddParameter(DateBlog, "DateBlog", "DATETIME");
                Ecom_DBConnection_.AddParameter((IsActiveEcommerce ? "si": "no"), "IsActiveEcommerce", "VARCHAR");
                Ecom_DBConnection_.AddParameter(1, "ModeProcedure", "INT");
                int result = Ecom_DBConnection_.ExecProcedure();
                if(result == 0)
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
        public bool Update(int mode)
        {
            try
            {
                Ecom_DBConnection_.StartProcedure("Admin_blog");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(Title, "Title", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Titlelargo, "Titlelargo", "VARCHAR");
                Ecom_DBConnection_.AddParameter(ContentShort, "ContentShort", "TEXT");
                Ecom_DBConnection_.AddParameter(Contentlarge, "Contentlarge", "TEXT");
                Ecom_DBConnection_.AddParameter(Comillas, "Comillass", "VARCHAR");
                Ecom_DBConnection_.AddParameter(ImageCoverPage, "ImageCoverPage", "VARCHAR");
                Ecom_DBConnection_.AddParameter(ImageBlog, "ImageBlog", "VARCHAR");
                Ecom_DBConnection_.AddParameter(DateBlog, "DateBlog", "DATETIME");
                Ecom_DBConnection_.AddParameter((IsActiveEcommerce ? "si" : "no"), "IsActiveEcommerce", "VARCHAR");
                Ecom_DBConnection_.AddParameter(mode, "ModeProcedure", "INT");
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
            try
            {
                List<Ecom_Blog> List = ReadDatReader(string.Format("SELECT * FROM menu_blog where id = '{0}'", id));
                if (List.Count == 1)
                {
                    List.ForEach(item => {
                        Id = item.Id;
                        Title = item.Title;
                        Titlelargo = item.Titlelargo;
                        ContentShort = item.ContentShort;
                        Contentlarge = item.Contentlarge;
                        Comillas = item.Comillas;
                        ImageCoverPage = item.ImageCoverPage;
                        ImageBlog = item.ImageBlog;
                        DateBlog = item.DateBlog;
                        IsActiveEcommerce = item.IsActiveEcommerce;
                    });
                    return true;
                }
                else
                {
                    Ecom_DBConnection_.Message = string.Format("No se ha podido encontrar el blog seleccionado");
                    return false;
                }
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public List<Ecom_Blog> Get()
        {
            try
            {
                return ReadDatReader("SELECT * FROM menu_blog");
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        private List<Ecom_Blog> ReadDatReader(string Statement)
        {
            List<Ecom_Blog> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_Blog>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_Blog
                        {
                            Id = Data.IsDBNull(0) ? 0 : (int)Data.GetUInt32(0),
                            Title = Data.IsDBNull(1) ? "" : Data.GetString(1),
                            Titlelargo = Data.IsDBNull(2) ? "" : Data.GetString(2),
                            ContentShort = Data.IsDBNull(3) ? "" : Data.GetString(3),
                            Contentlarge = Data.IsDBNull(4) ? "" : Data.GetString(4),
                            Comillas = Data.IsDBNull(5) ? "" : Data.GetString(5),
                            ImageCoverPage = Data.IsDBNull(6) ? "" : Data.GetString(6),
                            ImageBlog = Data.IsDBNull(7) ? "" : Data.GetString(7),
                            DateBlog = Data.IsDBNull(8) ? DateTime.Now : Data.GetDateTime(8),
                            IsActiveEcommerce = Data.IsDBNull(9) ? false : (Data.GetString(9) == "si" ? true : false),
                        });

                    }
                    Data.Close();
                }
                else
                {
                    Ecom_DBConnection_.Message = "Sin registroso";
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
