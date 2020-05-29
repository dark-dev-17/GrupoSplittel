using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcomDataProccess
{
    public class Ecom_DireccionFacturacion
    {
        #region Propiedades
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Cliente")]
        [Required]
        public int Cliente { get; set; }
        [Display(Name = "Razón Social")]
        [Required]
        public string RazonSocial { get; set; }
        [Display(Name = "Tipo Facturación")]
        [Required]
        public string TipoFacturacion { get; set; }
        [Display(Name = "Rfc")]
        [Required]
        public string RFC { get; set; }
        [Display(Name = "Calle")]
        [Required]
        public string Calle { get; set; }
        [Display(Name = "No.Ext")]
        public string NoExterior { get; set; }
        [Display(Name = "No.Int")]
        public string NoInterior { get; set; }
        [Display(Name = "Código postal")]
        [Required]
        public string CodigoPostal { get; set; }
        [Display(Name = "Estado")]
        [Required]
        public string Estado { get; set; }
        [Display(Name = "Ciudad")]
        [Required]
        public string Ciudad { get; set; }
        [Display(Name = "Delegación")]
        [Required]
        public string Delegacion { get; set; }
        [Display(Name = "Colonia")]
        [Required]
        public string Colonia { get; set; }
        [Display(Name = "Activo en Ecommerce")]
        [Required]
        public bool IsActive { get; set; }
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_DireccionFacturacion()
        {
            
        }
        public Ecom_DireccionFacturacion()
        {

        }
        public Ecom_DireccionFacturacion(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Add()
        {
            try
            {
                //Ecom_DBConnection_.StartProcedure("Admin_blog");
                //Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                //Ecom_DBConnection_.AddParameter(Title, "Title", "VARCHAR");
                //Ecom_DBConnection_.AddParameter(Titlelargo, "Titlelargo", "VARCHAR");
                //Ecom_DBConnection_.AddParameter(ContentShort, "ContentShort", "TEXT");
                //Ecom_DBConnection_.AddParameter(Contentlarge, "Contentlarge", "TEXT");
                //Ecom_DBConnection_.AddParameter(Comillas, "Comillass", "VARCHAR");
                //Ecom_DBConnection_.AddParameter(ImageCoverPage, "ImageCoverPage", "VARCHAR");
                //Ecom_DBConnection_.AddParameter(ImageBlog, "ImageBlog", "VARCHAR");
                //Ecom_DBConnection_.AddParameter(DateBlog, "DateBlog", "DATETIME");
                //Ecom_DBConnection_.AddParameter((IsActiveEcommerce ? "si": "no"), "IsActiveEcommerce", "VARCHAR");
                //Ecom_DBConnection_.AddParameter(1, "ModeProcedure", "INT");
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
        public int GetLastId()
        {
            return Ecom_DBConnection_.ExecuteScalarInt("SELECT max(id) FROM datos_facturacion");
        }
        public bool Get(int id)
        {
            try
            {
                List<Ecom_DireccionFacturacion> List = ReadDatReader(string.Format("SELECT * FROM datos_facturacion where id = '{0}'", id));
                if (List.Count == 1)
                {
                    List.ForEach(item => {
                        Id = item.Id;
                        Cliente = item.Cliente;
                        RazonSocial = item.RazonSocial;
                        TipoFacturacion = item.TipoFacturacion;
                        RFC = item.RFC;
                        Calle = item.Calle;
                        NoExterior = item.NoExterior;
                        NoInterior = item.NoInterior;
                        CodigoPostal = item.CodigoPostal;
                        Estado = item.Estado;
                        Ciudad = item.Ciudad;
                        Delegacion = item.Delegacion;
                        Colonia = item.Colonia;
                        IsActive = item.IsActive;
                    });
                    return true;
                }
                else
                {
                    Ecom_DBConnection_.Message = string.Format("No se ha podido encontrar el registro seleccionado");
                    return false;
                }
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public List<Ecom_DireccionFacturacion> Get()
        {
            try
            {
                return ReadDatReader("SELECT * FROM datos_facturacion");
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public List<Ecom_DireccionFacturacion> GetCliente(int IdCliente_)
        {
            try
            {
                return ReadDatReader(string.Format("SELECT * FROM datos_facturacion where id_cliente = '{0}'", IdCliente_));
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        private List<Ecom_DireccionFacturacion> ReadDatReader(string Statement)
        {
            List<Ecom_DireccionFacturacion> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_DireccionFacturacion>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_DireccionFacturacion
                        {
                            Id = Data.IsDBNull(0) ? 0 : (int)Data.GetUInt32(0),
                            Cliente = Data.IsDBNull(1) ? 0 : Data.GetInt32(1),
                            RazonSocial = Data.IsDBNull(2) ? "" : Data.GetString(2),
                            TipoFacturacion = Data.IsDBNull(3) ? "" : Data.GetString(3),
                            RFC = Data.IsDBNull(4) ? "" : Data.GetString(4),
                            Calle = Data.IsDBNull(5) ? "" : Data.GetString(5),
                            NoExterior = Data.IsDBNull(6) ? "" : Data.GetString(6),
                            NoInterior = Data.IsDBNull(7) ? "" : Data.GetString(7),
                            CodigoPostal = Data.IsDBNull(8) ? "" : Data.GetString(8),
                            Estado = Data.IsDBNull(9) ? "" : Data.GetString(9),
                            Ciudad = Data.IsDBNull(10) ? "" : Data.GetString(10),
                            Delegacion = Data.IsDBNull(11) ? "" : Data.GetString(11),
                            Colonia = Data.IsDBNull(12) ? "" : Data.GetString(12),
                            IsActive = Data.IsDBNull(13) ? false : (Data.GetString(13) == "si" ? true : false),
                        });

                    }
                    Data.Close();
                }
                else
                {
                    Ecom_DBConnection_.Message = "Sin registros";
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
