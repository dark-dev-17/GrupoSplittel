using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcomDataProccess
{
    public class Ecom_ProductoSubCategoria
    {
        #region Propiedades
        [Display(Name = "SubCategoria")]
        public int Id { get; set; }
        [Display(Name = "SubCategoria")]
        public string Id_subcategoria { get; set; }
        [Display(Name = "Descripcion")]
        public string Description { get; set; }
        [Display(Name = "Categoria")]
        public string Id_categoria { get; set; }
        [Display(Name = "Tiene configurables")]
        public bool HasSubNivel { get; set; }
        [Display(Name = "Activo en E-commerce")]
        public bool IsActiveEcommerce { get; set; }
        [Display(Name = "Nombre del forlder de vistas")]
        public string FolderNameView { get; set; }
        [Display(Name = "Descripción larga")]
        public string LargeDescripton { get; set; }

        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_ProductoSubCategoria()
        {
            
        }
        public Ecom_ProductoSubCategoria()
        {

        }
        public Ecom_ProductoSubCategoria(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Update(int Mode)
        {
            try
            {
                if (string.IsNullOrEmpty(Id_subcategoria))
                {
                    throw new Ecom_Exception("Ninguna categoria seleccionada");
                }
                string Statement = string.Format("Admin_Subcategoria|" +
                    "Id@INT={0}" +
                    "&Id_subcategoria@VARCHAR={1}" +
                    "&Descriptionn@VARCHAR={2}" +
                    "&Id_categoria@VARCHAR={3}" +
                    "&HasSubNivel@VARCHAR={4}" +
                    "&IsActiveEcommerce@VARCHAR={5}" +
                    "&FolderNameView@VARCHAR={6}" +
                    "&LargeDescripton@TEXT={7}" +
                    "&ModeProcedure@INT={8}",
                        Id,
                        Id_subcategoria,
                        Description,
                        Id_categoria,
                        (HasSubNivel ? "SI" : "NO"),
                        (IsActiveEcommerce ? "si" : "no"),
                        FolderNameView, LargeDescripton, Mode);
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
        public bool GetById(string IdSubCategoria)
        {
            try
            {
                List<Ecom_ProductoSubCategoria> List = ReadDatReader(string.Format("SELECT * FROM menu_subcategorias where id_subcategoria = '{0}'", IdSubCategoria));
                if(List.Count == 1)
                {
                    List.ForEach(item => {
                        Id = item.Id;
                        Id_subcategoria = item.Id_subcategoria;
                        Description = item.Description;
                        Id_categoria = item.Id_categoria;
                        HasSubNivel = item.HasSubNivel;
                        IsActiveEcommerce = item.IsActiveEcommerce;
                        FolderNameView = item.FolderNameView;
                        LargeDescripton = item.LargeDescripton;
                    });
                    return true;
                }
                else
                {
                    Ecom_DBConnection_.Message = string.Format("La subcategoria '{0}' no existe", IdSubCategoria);
                    return false;
                }
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public List<Ecom_ProductoSubCategoria> Get(string IdCategoria)
        {
            try
            {
                return ReadDatReader(string.Format("SELECT * FROM menu_subcategorias where id_familia = '{0}'",IdCategoria));
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public List<Ecom_ProductoSubCategoria> Get()
        {
            try
            {
                return ReadDatReader(string.Format("SELECT * FROM menu_subcategorias"));
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        private List<Ecom_ProductoSubCategoria> ReadDatReader(string Statement)
        {
            List<Ecom_ProductoSubCategoria> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_ProductoSubCategoria>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_ProductoSubCategoria
                        {
                            Id = Data.IsDBNull(0) ? 0 : (int)Data.GetUInt32(0),
                            Id_subcategoria = Data.IsDBNull(1) ? "" : Data.GetString(1),
                            Description = Data.IsDBNull(2) ? "" : Data.GetString(2),
                            Id_categoria = Data.IsDBNull(3) ? "" : Data.GetString(3),
                            HasSubNivel = Data.IsDBNull(4) ? false : (Data.GetString(4) == "SI" ? true : false),
                            IsActiveEcommerce = Data.IsDBNull(5) ? false : (Data.GetString(5) == "si" ? true : false),
                            FolderNameView = Data.IsDBNull(6) ? "" : Data.GetString(6),
                            LargeDescripton = Data.IsDBNull(7) ? "" : Data.GetString(7),
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
        public void SetConnection(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion
    }
    }
