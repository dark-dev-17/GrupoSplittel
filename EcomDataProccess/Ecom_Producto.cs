﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace EcomDataProccess
{
    public class Ecom_Producto
    {
        #region Propiedades
        public int IdProducto { get; private set; }
        public string ItemCode { get; private set; }
        public string Description { get; private set; }
        public string LargeDescription { get; private set; }
        public Ecom_ProductoCategoria Category { get; private set; }
        public Ecom_ProductoSubCategoria SubCategory { get; private set; }
        public Ecom_ProductoFichaTecnica FichaTecnica { get; private set; }
        public string DataSheetPath { get; private set; }
        public double UnitPrice { get; private set; }
        public double Discount { get; private set; }
        public double Stock { get; private set; }
        public double Quantity { get; private set; }
        public bool IsActiveEcomerce { get; private set; }
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_Producto()
        {
            
        }
        public Ecom_Producto()
        {

        }
        public Ecom_Producto(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Get(string codigo_)
        {
            bool result = false;
            try
            {
                string Statement = string.Format("SELECT * FROM Admin_producto_categoria_subcategoria where codigo = '{0}'", codigo_);
                List<Ecom_Producto> List = ReadDatReader(Statement);
                if(List.Count == 1)
                {
                    List.ForEach(item =>
                    {
                        IdProducto = item.IdProducto;
                        ItemCode = item.ItemCode;
                        Description = item.Description;
                        UnitPrice = item.UnitPrice;
                        Discount = item.Discount;
                        Stock = item.Stock;
                        IsActiveEcomerce = item.IsActiveEcomerce;
                        Category = item.Category;
                        SubCategory = item.SubCategory;
                        LargeDescription = item.LargeDescription;
                        FichaTecnica = item.FichaTecnica;
                    });
                    result = true;
                }
                return result;
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public List<Ecom_Producto> Get()
        {
            try
            {
                string Statement = string.Format("SELECT * FROM Admin_producto_categoria_subcategoria;");
                return ReadDatReader(Statement);
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        private List<Ecom_Producto> ReadDatReader(string Statement)
        {
            List<Ecom_Producto> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_Producto>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_Producto
                        {
                            IdProducto = (int)Data.GetUInt32(0),
                            ItemCode = Data.IsDBNull(1) ? "" : Data.GetString(1),
                            Description = Data.IsDBNull(2) ? "" : Data.GetString(2),
                            UnitPrice = Data.IsDBNull(3) ? 0 : Data.GetFloat(3),
                            Discount = Data.IsDBNull(15) ? 0 : Data.GetFloat(15),//
                            Stock = Data.IsDBNull(4) ? 0 : Data.GetFloat(4),
                            IsActiveEcomerce = Data.IsDBNull(16) ? false : Data.GetString(16) == "si" ? true : false,//
                            Category = new Ecom_ProductoCategoria { Description = Data.IsDBNull(6) ? "" : Data.GetString(6), Id = Data.IsDBNull(5) ? 0 : Data.GetInt32(5) },
                            SubCategory = new Ecom_ProductoSubCategoria { Description = Data.IsDBNull(10) ? "" : Data.GetString(10), Id = Data.IsDBNull(8) ? 0 : Data.GetInt32(8) },
                            LargeDescription = Data.IsDBNull(14) ? "Sin descripción" : Data.GetString(14),
                            FichaTecnica = new Ecom_ProductoFichaTecnica { Id = Data.IsDBNull(12) ? 0 : Data.GetInt32(12), Ruta = Data.IsDBNull(13) ? "" : Data.GetString(13) },
                        });

                    }
                    Data.Close();
                }
                else
                {
                    Ecom_DBConnection_.Message = "No existen registros";
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
        #endregion
    }
    }
