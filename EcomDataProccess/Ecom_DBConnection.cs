using MySql.Data.MySqlClient;
using System;

namespace EcomDataProccess
{
    public class Ecom_DBConnection
    {
        #region Propiedades
        private string ConnectionString;
        public string Message { get; set; }
        public MySqlConnection Connection { get; private set; }
        #endregion

        #region Constructores
        ~Ecom_DBConnection()
        {
            
        }
        public Ecom_DBConnection()
        {

        }
        public Ecom_DBConnection(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }
        #endregion

        #region Metodos
        public int ExecuteScalarInt(string stattement)
        {
            try
            {
                CheckConnection();
                MySqlCommand cmd = new MySqlCommand(stattement, Connection);
                object result = (object)cmd.ExecuteScalar();
                if (result is System.DBNull || result == null)
                {
                    return 0;
                }
                else
                {
                    if (result.GetType() == typeof(UInt32))
                    {
                        return Convert.ToInt32(result);
                    }
                    
                    return (int)result;
                }
            }
            catch (Ecom_Exception ex)
            {
                throw new Ecom_Exception(string.Format("Ecom_Exception - {0}", ex.Message));
            }
            catch (MySqlException ex)
            {
                throw new Ecom_Exception(string.Format("MySqlException - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new Ecom_Exception(string.Format("Exception - {0}", ex.Message));
            }
        }
        public MySqlDataReader DoQuery(string stattement)
        {
            try
            {
                CheckConnection();
                MySqlCommand command = new MySqlCommand(stattement, Connection);
                MySqlDataReader dataReader = command.ExecuteReader();
                return dataReader;

            }
            catch (Ecom_Exception ex)
            {
                throw new Ecom_Exception(string.Format("Ecom_Exception - {0}", ex.Message));
            }
            catch (MySqlException ex)
            {
                throw new Ecom_Exception(string.Format("MySqlException - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new Ecom_Exception(string.Format("Exception - {0}", ex.Message));
            }
        }
        public void CloseConnection()
        {
            if (Connection.State == System.Data.ConnectionState.Open)
            {
                Connection.Close();
            }
        }
        public void OpenConnection()
        {
            Connection = new MySqlConnection(ConnectionString);
            try
            {
                Connection.Open();
                CheckConnection();
            }
            catch (Ecom_Exception ex)
            {
                throw new Ecom_Exception(string.Format("Ecom_Exception - {0}", ex.Message));
            }
            catch (MySqlException ex)
            {
                throw new Ecom_Exception(string.Format("MySqlException - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new Ecom_Exception(string.Format("Exception - {0}", ex.Message));
            }
        }
        private void CheckConnection()
        {
            if (Connection.State != System.Data.ConnectionState.Open)
            {
                throw new Ecom_Exception("Sin conexion a base de datos MySQL");
            }
        }
        #endregion
    }
}
