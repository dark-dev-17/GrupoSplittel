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
        public string Text { set; get; }
        public string Icon { set; get; }
        public string ClassActive { set; get; }
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
                            Description = Data.IsDBNull(1) ? "" : Data.GetString(1),
                            Text = Data.IsDBNull(4) ? "" : Data.GetString(4),
                            Icon = Data.IsDBNull(5) ? "" : Data.GetString(5),
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
        public List<Ecom_Modelo> Get(int id_user)
        {
            string Statement = string.Format("SELECT * FROM admin_modulos where clienteKey = '{0}';", id_user);
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
                            Description = Data.IsDBNull(1) ? "" : Data.GetString(1),
                            Text = Data.IsDBNull(2) ? "" : Data.GetString(2),
                            Icon = Data.IsDBNull(3) ? "" : Data.GetString(3),
                        });

                    }
                    Data.Close();
                    List.ForEach(item => item.Acciones = new Ecom_Acciones(Ecom_DBConnection_).Get(item.Id, id_user));
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
