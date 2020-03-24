using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace EcomDataProccess
{
    public class Ecom_ProductoSubCategoria
    {
        #region Propiedades
        public int Id { get; set; }
        public string Id_subcategoria { get; set; }
        public string Description { get; set; }
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
        }
        #endregion

        #region Metodos
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
        #endregion
    }
    }
