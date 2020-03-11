using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace EcomDataProccess
{
    public class Base
    {
        #region Propiedades
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Base()
        {
            
        }
        public Base()
        {

        }
        public Base(Ecom_DBConnection Ecom_DBConnection_)
        {
        }
        #endregion

        #region Metodos
        private List<Base> ReadDatReader(string Statement)
        {
            List<Base> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Base>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Base
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
