using System;
using System.Data;
using System.Data.SqlClient;

namespace SAPDataProcess
{
    public class SAP_DBConnection
    {
        #region Propiedades
        private string ConnectionString;
        private DataTable DataTable;
        public SqlConnection SqlConnection;
        #endregion

        #region Constructores
        public SAP_DBConnection()
        {

        }
        public SAP_DBConnection(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }
        #endregion

        #region Metodos
        public DataTable GetData(string sqlStatement)
        {
            try
            {
                CheckConnection();
                using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, SqlConnection))
                {
                    sqlCommand.CommandTimeout = 120;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        DataTable = new DataTable();
                        sqlDataAdapter.Fill(DataTable);
                        sqlDataAdapter.Dispose();
                        return DataTable;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new SAP_Excepcion(string.Format("SqlException - {0}", ex.Message));
            }
            catch (SAP_Excepcion ex)
            {
                throw new SAP_Excepcion(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new SAP_Excepcion(string.Format("Exception - {0}", ex.Message));
            }
        }
        public int GetIntegerValue(string sqlStatement)
        {
            try
            {
                CheckConnection();
                using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, SqlConnection))
                {
                    return string.IsNullOrEmpty(sqlCommand.ExecuteScalar().ToString()) ? 0 : int.Parse(sqlCommand.ExecuteScalar().ToString());
                }
            }
            catch (SqlException ex)
            {
                throw new SAP_Excepcion(string.Format("SqlException - {0}", ex.Message));
            }
            catch (SAP_Excepcion ex)
            {
                throw new SAP_Excepcion(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new SAP_Excepcion(string.Format("Exception - {0}", ex.Message));
            }
        }
        public string GetStringValue(string sqlStatement)
        {
            try
            {
                CheckConnection();
                using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, SqlConnection))
                {
                    return sqlCommand.ExecuteScalar().ToString();
                }
            }
            catch (SqlException ex)
            {
                throw new SAP_Excepcion(string.Format("SqlException - {0}", ex.Message));
            }
            catch (SAP_Excepcion ex)
            {
                throw new SAP_Excepcion(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new SAP_Excepcion(string.Format("Exception - {0}", ex.Message));
            }
        }
        public double GetDoublelValue(string sqlStatement)
        {
            try
            {
                CheckConnection();
                using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, SqlConnection))
                {
                    return string.IsNullOrEmpty(sqlCommand.ExecuteScalar().ToString()) ? 0 : double.Parse(sqlCommand.ExecuteScalar().ToString());
                }
            }
            catch (SqlException ex)
            {
                throw new SAP_Excepcion(string.Format("SqlException - {0}", ex.Message));
            }
            catch (SAP_Excepcion ex)
            {
                throw new SAP_Excepcion(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new SAP_Excepcion(string.Format("Exception - {0}", ex.Message));
            }
        }
        public DateTime GetDateTimeValue(string sqlStatement)
        {
            try
            {
                CheckConnection();
                using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, SqlConnection))
                {
                    return DateTime.Parse(sqlCommand.ExecuteScalar().ToString());
                }
            }
            catch (SqlException ex)
            {
                throw new SAP_Excepcion(string.Format("SqlException - {0}", ex.Message));
            }
            catch (SAP_Excepcion ex)
            {
                throw new SAP_Excepcion(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new SAP_Excepcion(string.Format("Exception - {0}", ex.Message));
            }
        }
        public SqlDataReader GetDataReader(string sqlStatement)
        {
            try
            {
                CheckConnection();
                using(SqlCommand sqlCommand = new SqlCommand(sqlStatement, SqlConnection))
                {
                    sqlCommand.CommandTimeout = 120;
                    SqlDataReader DataReader = sqlCommand.ExecuteReader();
                    return DataReader;
                }
            }
            catch (SqlException ex)
            {
                throw new SAP_Excepcion(string.Format("SqlException - {0}", ex.Message));
            }
            catch (SAP_Excepcion ex)
            {
                throw new SAP_Excepcion(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new SAP_Excepcion(string.Format("Exception - {0}", ex.Message));
            }
        }
        public void OpenConnection()
        {
            try
            {
                SqlConnection = new SqlConnection(ConnectionString);
                SqlConnection.Open();
                CheckConnection();
            }
            catch (SqlException ex)
            {
                throw new SAP_Excepcion(string.Format("SqlException - {0}", ex.Message));
            }
            catch (SAP_Excepcion ex)
            {
                throw new SAP_Excepcion(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new SAP_Excepcion(string.Format("Exception - {0}", ex.Message));
            }
        }
        public void CloseDataBaseAccess()
        {
            if (SqlConnection.State == ConnectionState.Open)
                SqlConnection.Close();
        }
        private void CheckConnection()
        {
            if (SqlConnection.State != ConnectionState.Open)
            {
                throw new SAP_Excepcion("No database connection");
            }
        }
        #endregion
    }
}
