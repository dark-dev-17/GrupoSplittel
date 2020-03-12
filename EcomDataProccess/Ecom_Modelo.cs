using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace EcomDataProccess
{
    public class Ecom_Modelo
    {
        #region Propiedades
        public int Id { set; get; }
        public int IdUser { set; get; }
        public string Description { set; get; }
        public bool isAccessAdmin { set; get; }
        public List<Ecom_Acciones> Acciones { set; get; }
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_Modelo()
        {
            
        }
        public Ecom_Modelo()
        {

        }
        public Ecom_Modelo(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public List<Ecom_Modelo> Get()
        {
            string Statement = string.Format("SELECT * FROM t01_admin_objeto;");
            List<Ecom_Modelo> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_Modelo>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_Modelo
                        {
                            Id = (int)Data.GetUInt32(0),
                            Description = Data.IsDBNull(1) ? "" : Data.GetString(1)
                        });

                    }
                    Data.Close();
                    List.ForEach(item => item.Acciones = new Ecom_Acciones(Ecom_DBConnection_).Get(item.Id));
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
