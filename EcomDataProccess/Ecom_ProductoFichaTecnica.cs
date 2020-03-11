using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace EcomDataProccess
{
    public class Ecom_ProductoFichaTecnica
    {
        #region Propiedades
        public int Id { get; set; }
        public string Ruta { get; set; }
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_ProductoFichaTecnica()
        {
            
        }
        public Ecom_ProductoFichaTecnica()
        {

        }
        public Ecom_ProductoFichaTecnica(Ecom_DBConnection Ecom_DBConnection_)
        {
        }
        #endregion

        #region Metodos
        private List<Ecom_ProductoFichaTecnica> ReadDatReader(string Statement)
        {
            List<Ecom_ProductoFichaTecnica> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_ProductoFichaTecnica>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_ProductoFichaTecnica
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
