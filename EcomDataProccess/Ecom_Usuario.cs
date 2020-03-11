using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace EcomDataProccess
{
    public class Ecom_Usuario
    {
        #region Propiedades
        public int IdSplinnet { private set; get; }
        public string Username { private set; get; }
        public string Password { private set; get; }
        public string Nombre { private set; get; }
        public string ApellidoPaterno { private set; get; }
        public string Apellidomaterno { private set; get; }
        public string Correo { private set; get; }
        public string Sociedad { private set; get; }
        public string Foto { private set; get; }
        public int IdArea { private set; get; }
        public List<int> Id_sap { private set; get; }
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_Usuario()
        {
            
        }
        public Ecom_Usuario()
        {

        }
        public Ecom_Usuario(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool AccessToAction(int IdSplinnet, int action)
        {
            string Statement = string.Format("SELECT * FROM t03_permisos where clienteKey = '{0}' and t02_pk01 = '{1}' and t03_f001 = '1'", IdSplinnet, action);
            bool result = false;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                if (Ecom_DBConnection_.ExecuteScalarInt(Statement) != 0)
                {
                    result = true;
                }
                return result;
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public bool AccessToAction(int IdSplinnet, int[] action)
        {
            bool access = false;
            foreach (int item in action)
            {
                access = AccessToAction(IdSplinnet, item);
                if (access) break;
            }
            return access;
        }
        public bool ValidLogin(string User, string password)
        {
            string Statement = string.Format("SELECT ID FROM signup where username = '{0}' and password = '{1}';", User, password);
            bool result = false;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                int Response = Ecom_DBConnection_.ExecuteScalarInt(Statement);
                if (Response != 0)
                {
                    Get(Response);
                    IdSplinnet = Response;
                    GetIdSap();
                    result = true;
                }
                return result;
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
            finally
            {
                
            }
        }
        public void GetIdSap()
        {
            Id_sap = new List<int>();
            string Statement = string.Format("SELECT id_sap FROM id_split_sap where id_splittel = '{0}';", IdSplinnet);
            MySqlDataReader Data = null;
            try
            {
                Data = Ecom_DBConnection_.DoQuery(Statement);
                while (Data.Read())
                {
                    Id_sap.Add(Data.IsDBNull(0) ? 0 : (int)Data.GetInt32(0));
                }
                Data.Close();
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
        public bool Get(int IdSplinnet_)
        {
            string Statement = string.Format("SELECT ID,username,email,nombre,apaterno,amaterno,id_area,sociedad,foto,password FROM signup where ID = '{0}';", IdSplinnet_);
            bool result = false;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        IdSplinnet = Data.IsDBNull(0) ? 0 : (int)Data.GetUInt32(0);
                        Username = Data.IsDBNull(1) ? "" : Data.GetString(1);
                        Correo = Data.IsDBNull(2) ? "" : Data.GetString(2);
                        Nombre = Data.IsDBNull(3) ? "" : Data.GetString(3);
                        ApellidoPaterno = Data.IsDBNull(4) ? "" : Data.GetString(4);
                        Apellidomaterno = Data.IsDBNull(5) ? "" : Data.GetString(5);
                        IdArea = Data.IsDBNull(6) ? 0 : Data.GetInt32(6);
                        Sociedad = Data.IsDBNull(7) ? "" : Data.GetString(7);
                        Foto = Data.IsDBNull(8) ? "" : Data.GetString(8);
                        Password = Data.IsDBNull(9) ? "" : Data.GetString(9);
                    }
                    Data.Close();
                    result = true;
                }
                else
                {
                    Ecom_DBConnection_.Message = "Usuario no encontrado";
                }
                return result;
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
        public List<Ecom_Usuario> Get()
        {
            string Statement = string.Format("SELECT ID,username,email,nombre,apaterno,amaterno,id_area,sociedad,foto,password FROM signup");
            MySqlDataReader Data = null;
            List<Ecom_Usuario> List = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_Usuario>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_Usuario {
                            IdSplinnet = Data.IsDBNull(0) ? 0 : (int)Data.GetUInt32(0),
                            Username = Data.IsDBNull(1) ? "" : Data.GetString(1),
                            Correo = Data.IsDBNull(2) ? "" : Data.GetString(2),
                            Nombre = Data.IsDBNull(3) ? "" : Data.GetString(3),
                            ApellidoPaterno = Data.IsDBNull(4) ? "" : Data.GetString(4),
                            Apellidomaterno = Data.IsDBNull(5) ? "" : Data.GetString(5),
                            IdArea = Data.IsDBNull(6) ? 0 : Data.GetInt32(6),
                            Sociedad = Data.IsDBNull(7) ? "" : Data.GetString(7),
                            Foto = Data.IsDBNull(8) ? "" : Data.GetString(8),
                            Password = Data.IsDBNull(9) ? "" : Data.GetString(9)
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
