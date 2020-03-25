using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcomDataProccess
{
    public class Ecom_ProductoCategoria
    {
        #region Propiedades
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Categoria")]
        public string Id_categoria { get; set; }
        [Display(Name = "Familia")]
        public string Description { get; set; }
        [Display(Name = "Imagen")]
        public string Imagen { get; set; }
        [Display(Name = "Visible E-commerce")]
        public bool IsActive { get; set; }
        [Display(Name = "Visible Info.Técnica")]
        public bool IsActiveMenu1 { get; set; }
        [Display(Name = "Visible Hoja en Técnicas")]
        public bool IsActiveMenu2 { get; set; }
        public string FolderName { get; set; }
        [Display(Name = "Descripción")]
        public string DescripcionLarga { get; set; }
        public int Total { get; set; }
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_ProductoCategoria()
        {
            
        }
        public Ecom_ProductoCategoria()
        {

        }
        public Ecom_ProductoCategoria(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Update(int Mode)
        {
            try
            {
                if(Id == 0)
                {
                    throw new Ecom_Exception("Ninguna categoria seleccionada");
                }
                string Statement = string.Format("Admin_categoria|" +
                    "Id@INT={0}" +
                    "&Id_categoria@VARCHAR={1}" +
                    "&Description@VARCHAR={2}" +
                    "&Imagen@TEXT={3}" +
                    "&IsActive@TEXT={4}" +
                    "&IsActiveMenu1@VARCHAR={5}" +
                    "&IsActiveMenu2@VARCHAR={6}" +
                    "&FolderName@VARCHAR={7}" +
                    "&DescripcionLarga@TEXT={8}" +
                    "&ModeProcedure@INT={9}",
                        Id,
                        Id_categoria,
                        Description,
                        Imagen,
                        (IsActive ? "si" : "no"),
                        (IsActiveMenu1 ? "si" : "no"), 
                        (IsActiveMenu2 ? "si" : "no"),
                        FolderName, DescripcionLarga, Mode);
                int result = Ecom_DBConnection_.ExecuteStoreProcedure(Statement);
                if (result == 0)
                {
                    return true;
                }
                else
                {
                    throw new Ecom_Exception(Ecom_DBConnection_.Message);
                }
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public bool Get(string id)
        {
            try
            {
                List<Ecom_ProductoCategoria> List = ReadDatReader(string.Format("SELECT * FROM menu_categorias where id_codigo = '{0}'",id));
                if(List.Count == 1)
                {
                    List.ForEach(item => {
                        Id = item.Id;
                        Id_categoria = item.Id_categoria;
                        Description = item.Description;
                        Imagen = item.Imagen;
                        IsActive = item.IsActive;
                        IsActiveMenu1 = item.IsActiveMenu1;
                        IsActiveMenu2 = item.IsActiveMenu2;
                        FolderName = item.FolderName;
                        DescripcionLarga = item.DescripcionLarga;
                    });
                    return true;
                }
                else
                {
                    Ecom_DBConnection_.Message = string.Format("La categoria '{0}' no fue encontrada",id);
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<Ecom_ProductoCategoria> Get()
        {
            try
            {
                return ReadDatReader("SELECT * FROM menu_categorias");
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        private List<Ecom_ProductoCategoria> ReadDatReader(string Statement)
        {
            List<Ecom_ProductoCategoria> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_ProductoCategoria>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_ProductoCategoria
                        {
                            Id = Data.IsDBNull(0) ? 0 : (int)Data.GetUInt32(0),
                            Id_categoria = Data.IsDBNull(1) ? "" : Data.GetString(1),
                            Description = Data.IsDBNull(2) ? "" : Data.GetString(2),
                            Imagen = Data.IsDBNull(3) ? "" : Data.GetString(3),
                            IsActive = Data.IsDBNull(4) ? false : (Data.GetString(4) == "si" ? true : false),
                            IsActiveMenu1 = Data.IsDBNull(5) ? false : (Data.GetString(5) == "si" ? true : false),
                            IsActiveMenu2 = Data.IsDBNull(6) ? false : (Data.GetString(6) == "si" ? true : false),
                            FolderName = Data.IsDBNull(7) ? "" : Data.GetString(7),
                            DescripcionLarga = Data.IsDBNull(8) ? "" : Data.GetString(8),
                        });
                    }
                    Data.Close();
                }
                else
                {
                    Ecom_DBConnection_.Message = "Usuario no encontrado";
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
        public List<Ecom_ProductoCategoria> GetQuoatationsDashboard(DateTime start, DateTime end, string ModeBussiness, string tipoDocumento)
        {
            string Statement = string.Format("Admin_QuotationsDashboard|startdate@DATETIME={0}&enddate@DATETIME={1}&tipoDocumento@VARCHAR={2}&ModeBussiness@VARCHAR={3}&ModeQuery@INT={4}",
                start.ToString("yyyy-MM-dd"),
                end.ToString("yyyy-MM-dd 23:59:59"),
                tipoDocumento,
                ModeBussiness,
                2);
            MySqlDataReader data = null;
            List<Ecom_ProductoCategoria> List;
            try
            {
                data = Ecom_DBConnection_.ExecuteStoreProcedureReader(Statement);
                List = new List<Ecom_ProductoCategoria>();
                while (data.Read())
                {
                    Ecom_ProductoCategoria categoria = new Ecom_ProductoCategoria();
                    categoria.Id_categoria = data.IsDBNull(0) ? "" : (data.GetString(0) + "");
                    categoria.Description = data.IsDBNull(1) ? "" : data.GetString(1);
                    categoria.Total = data.IsDBNull(2) ? 0 : (int)data.GetDouble(2);
                    List.Add(categoria);
                }
                return List;
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (data != null)
                {
                    data.Close();
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
