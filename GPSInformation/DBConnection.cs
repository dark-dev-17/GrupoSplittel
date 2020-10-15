using GPSInformation.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GPSInformation
{
    public class DBConnection
    {
        #region Propiedades
        private string ConnectionString;
        private DataTable DataTable;
        private SqlConnection SqlConnection;
        private SqlCommand Command;
        public string mensaje;
        public int ErrorCode;
        public bool IsTracsactionActive = false;
        private SqlTransaction tran;
        #endregion

        #region Constructores
        public DBConnection()
        {

        }
        public DBConnection(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }
        #endregion

        #region Metodos

        public void StartTransaction()
        {
            tran = SqlConnection.BeginTransaction();
            IsTracsactionActive = true;
        }

        public void Commit()
        {
            if(IsTracsactionActive == false)
            {
                throw new GpExceptions("Transactios are inactive");
            }
            tran.Commit();
        }

        public void RolBack()
        {
            if (IsTracsactionActive == false)
            {
                throw new GpExceptions("Transactios are inactive");
            }
            tran.Rollback();
            CloseDataBaseAccess();
        }

        public void StartInsert(string statement, List<ProcedureModel> DataModel)
        {
            string Evaluando = "";
            try
            {
                if (DataModel == null)
                {
                    throw new GpExceptions("Sin parametros SP");
                }

                SqlDataAdapter adapter = new SqlDataAdapter();
                if (IsTracsactionActive)
                {
                    Command = new SqlCommand(statement, SqlConnection, tran);
                }
                else
                {
                    Command = new SqlCommand(statement, SqlConnection);
                }
                
                DataModel.ForEach(param => {
                    Evaluando = param.Namefield;
                    if(param.value != null)
                    {
                        if (typeof(int) == param.value.GetType())
                        {
                            if ((int)param.value == 0)
                            {
                                SqlParameter sqlParameter = new SqlParameter("@" + param.Namefield, DBNull.Value);
                                sqlParameter.Direction = ParameterDirection.Input;
                                Command.Parameters.Add(sqlParameter);
                            }
                            else
                            {
                                SqlParameter sqlParameter = new SqlParameter("@" + param.Namefield, param.value);
                                sqlParameter.Direction = ParameterDirection.Input;
                                Command.Parameters.Add(sqlParameter);
                            }
                        }
                        else if (typeof(DateTime?) == param.value.GetType())
                        {
                            SqlParameter sqlParameter = new SqlParameter("@" + param.Namefield, DBNull.Value);
                            sqlParameter.Direction = ParameterDirection.Input;
                            Command.Parameters.Add(sqlParameter);
                        }
                        else
                        {
                            SqlParameter sqlParameter = new SqlParameter("@" + param.Namefield, param.value);
                            sqlParameter.Direction = ParameterDirection.Input;
                            Command.Parameters.Add(sqlParameter);
                        }
                    }
                    else
                    {
                        SqlParameter sqlParameter = new SqlParameter("@" + param.Namefield, DBNull.Value);
                        sqlParameter.Direction = ParameterDirection.Input;
                        Command.Parameters.Add(sqlParameter);
                    }
                });

                adapter.InsertCommand = Command;
                adapter.InsertCommand.ExecuteNonQuery();
                mensaje = "Registro guardado";
            }
            catch (SqlException ex)
            {
                throw new GpExceptions(string.Format("SqlException - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new GpExceptions(string.Format("SqlException - {0} - {1}", Evaluando, ex.Message));
            }
        }
        public void StartUpdate(string statement, List<ProcedureModel> DataModel)
        {

            string Evaluando = "";
            try
            {
                if (DataModel == null)
                {
                    throw new GpExceptions("Sin parametros SP");
                }

                SqlDataAdapter adapter = new SqlDataAdapter();
                if (IsTracsactionActive)
                {
                    Command = new SqlCommand(statement, SqlConnection, tran);
                }
                else
                {
                    Command = new SqlCommand(statement, SqlConnection);
                }
                DataModel.ForEach(param => {
                    Evaluando = param.Namefield;
                    if (param.value != null)
                    {
                        if (typeof(int) == param.value.GetType())
                        {
                            if ((int)param.value == 0)
                            {
                                SqlParameter sqlParameter = new SqlParameter("@" + param.Namefield, DBNull.Value);
                                sqlParameter.Direction = ParameterDirection.Input;
                                Command.Parameters.Add(sqlParameter);
                            }
                            else
                            {
                                SqlParameter sqlParameter = new SqlParameter("@" + param.Namefield, param.value);
                                sqlParameter.Direction = ParameterDirection.Input;
                                Command.Parameters.Add(sqlParameter);
                            }
                        }
                        else if (typeof(DateTime?) == param.value.GetType())
                        {
                            SqlParameter sqlParameter = new SqlParameter("@" + param.Namefield, DBNull.Value);
                            sqlParameter.Direction = ParameterDirection.Input;
                            Command.Parameters.Add(sqlParameter);
                        }
                        else
                        {
                            SqlParameter sqlParameter = new SqlParameter("@" + param.Namefield, param.value);
                            sqlParameter.Direction = ParameterDirection.Input;
                            Command.Parameters.Add(sqlParameter);
                        }
                    }
                    else
                    {
                        SqlParameter sqlParameter = new SqlParameter("@" + param.Namefield, DBNull.Value);
                        sqlParameter.Direction = ParameterDirection.Input;
                        Command.Parameters.Add(sqlParameter);
                    }
                });

                adapter.UpdateCommand = Command;
                adapter.UpdateCommand.ExecuteNonQuery();
                mensaje = "Registro actualizado";
            }
            catch (SqlException ex)
            {
                throw new GpExceptions(string.Format("SqlException - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new GpExceptions(string.Format("SqlException - {0} - {1}", Evaluando, ex.Message));
            }
        }
        public void StartDelete(string statement, List<ProcedureModel> DataModel)
        {
            try
            {
                if (DataModel == null)
                {
                    throw new GpExceptions("Sin parametros SP");
                }

                SqlDataAdapter adapter = new SqlDataAdapter();
                if (IsTracsactionActive)
                {
                    Command = new SqlCommand(statement, SqlConnection, tran);
                }
                else
                {
                    Command = new SqlCommand(statement, SqlConnection);
                }
                DataModel.ForEach(param => {
                    if (typeof(int) == param.value.GetType())
                    {
                        if ((int)param.value == 0)
                        {
                            SqlParameter sqlParameter = new SqlParameter("@" + param.Namefield, DBNull.Value);
                            sqlParameter.Direction = ParameterDirection.Input;
                            Command.Parameters.Add(sqlParameter);
                        }
                        else
                        {
                            SqlParameter sqlParameter = new SqlParameter("@" + param.Namefield, param.value);
                            sqlParameter.Direction = ParameterDirection.Input;
                            Command.Parameters.Add(sqlParameter);
                        }
                    }
                    else
                    {
                        SqlParameter sqlParameter = new SqlParameter("@" + param.Namefield, param.value);
                        sqlParameter.Direction = ParameterDirection.Input;
                        Command.Parameters.Add(sqlParameter);
                    }
                });

                adapter.DeleteCommand = Command;
                adapter.DeleteCommand.ExecuteNonQuery();
                mensaje = "Registro eliminado";
            }
            catch (SqlException ex)
            {
                throw new GpExceptions(string.Format("SqlException - {0}", ex.Message));
            }
        }
        public void StartProcedure(string ProcedureName, List<ProcedureModel> DataModel)
        {
            Command = new SqlCommand(ProcedureName, SqlConnection);
            Command.CommandType = CommandType.StoredProcedure;

            if(DataModel == null)
            {
                throw new GpExceptions("Sin parametros SP");
            }
            try
            {
                DataModel.ForEach(param => {
                    SqlParameter sqlParameter = new SqlParameter("@" + param.Namefield, param.value);
                    sqlParameter.Direction = ParameterDirection.Input;
                    Command.Parameters.Add(sqlParameter);
                });

                var MessageCode = Command.Parameters.Add("@MessageCode", SqlDbType.Int);
                MessageCode.Direction = ParameterDirection.Output;
                var MessageValue = Command.Parameters.Add("@MessageValue", SqlDbType.NVarChar, 200);
                MessageValue.Direction = ParameterDirection.Output;
                Command.ExecuteNonQuery();

                ErrorCode = (int)Command.Parameters["@MessageCode"].Value;
                mensaje = (string)Command.Parameters["@MessageValue"].Value;
            }
            catch (SqlException ex)
            {
                throw new GpExceptions(string.Format("SqlException - {0}", ex.Message));
            }
            

        }
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
                throw new GpExceptions(string.Format("SqlException - {0}", ex.Message));
            }
            catch (GpExceptions ex)
            {
                throw new GpExceptions(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new GpExceptions(string.Format("Exception - {0}", ex.Message));
            }
        }
        public int GetIntegerValue(string sqlStatement)
        {
            try
            {
                CheckConnection();
                if (IsTracsactionActive)
                {
                    using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, SqlConnection,tran))
                    {
                        return string.IsNullOrEmpty(sqlCommand.ExecuteScalar().ToString()) ? 0 : int.Parse(sqlCommand.ExecuteScalar().ToString());
                    }
                }
                else
                {
                    using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, SqlConnection))
                    {
                        return string.IsNullOrEmpty(sqlCommand.ExecuteScalar().ToString()) ? 0 : int.Parse(sqlCommand.ExecuteScalar().ToString());
                    }
                }
                
            }
            catch (SqlException ex)
            {
                throw new GpExceptions(string.Format("SqlException - {0}", ex.Message));
            }
            catch (GpExceptions ex)
            {
                throw new GpExceptions(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object GetValue(string sqlStatement)
        {
            try
            {
                CheckConnection();
                if (IsTracsactionActive)
                {
                    using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, SqlConnection, tran))
                    {
                        return sqlCommand.ExecuteScalar();
                    }
                }
                else
                {
                    using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, SqlConnection))
                    {
                        return sqlCommand.ExecuteScalar();
                    }
                }

            }
            catch (SqlException ex)
            {
                throw new GpExceptions(string.Format("SqlException - {0}", ex.Message));
            }
            catch (GpExceptions ex)
            {
                throw new GpExceptions(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetStringValue(string sqlStatement)
        {
            try
            {
                CheckConnection();
                if (IsTracsactionActive)
                {
                    using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, SqlConnection, tran))
                    {
                        return sqlCommand.ExecuteScalar().ToString();
                    }
                }
                else
                {
                    using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, SqlConnection))
                    {
                        return sqlCommand.ExecuteScalar().ToString();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new GpExceptions(string.Format("SqlException - {0}", ex.Message));
            }
            catch (GpExceptions ex)
            {
                throw new GpExceptions(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new GpExceptions(string.Format("Exception - {0}", ex.Message));
            }
        }
        public double GetDoublelValue(string sqlStatement)
        {
            try
            {
                CheckConnection();
                if (IsTracsactionActive)
                {
                    using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, SqlConnection, tran))
                    {
                        return string.IsNullOrEmpty(sqlCommand.ExecuteScalar().ToString()) ? 0 : double.Parse(sqlCommand.ExecuteScalar().ToString());
                    }
                }
                else
                {
                    using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, SqlConnection))
                    {
                        return string.IsNullOrEmpty(sqlCommand.ExecuteScalar().ToString()) ? 0 : double.Parse(sqlCommand.ExecuteScalar().ToString());
                    }
                }
                    
            }
            catch (SqlException ex)
            {
                throw new GpExceptions(string.Format("SqlException - {0}", ex.Message));
            }
            catch (GpExceptions ex)
            {
                throw new GpExceptions(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new GpExceptions(string.Format("Exception - {0}", ex.Message));
            }
        }
        public DateTime GetDateTimeValue(string sqlStatement)
        {
            try
            {
                CheckConnection();
                if (IsTracsactionActive)
                {
                    using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, SqlConnection, tran))
                    {
                        return DateTime.Parse(sqlCommand.ExecuteScalar().ToString());
                    }
                }
                else
                {
                    using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, SqlConnection))
                    {
                        return DateTime.Parse(sqlCommand.ExecuteScalar().ToString());
                    }
                }
                    
            }
            catch (SqlException ex)
            {
                throw new GpExceptions(string.Format("SqlException - {0}", ex.Message));
            }
            catch (GpExceptions ex)
            {
                throw new GpExceptions(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new GpExceptions(string.Format("Exception - {0}", ex.Message));
            }
        }
        public SqlDataReader GetDataReader(string sqlStatement)
        {
            try
            {
                CheckConnection();
                if (IsTracsactionActive)
                {
                    using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, SqlConnection, tran))
                    {
                        sqlCommand.CommandTimeout = 120;
                        SqlDataReader DataReader = sqlCommand.ExecuteReader();
                        if (!DataReader.HasRows)
                        {
                            mensaje = "No se encontraron registros!";
                            ErrorCode = 200;
                        }
                        return DataReader;
                    }
                }
                else
                {
                    using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, SqlConnection))
                    {
                        sqlCommand.CommandTimeout = 120;
                        SqlDataReader DataReader = sqlCommand.ExecuteReader();
                        if (!DataReader.HasRows)
                        {
                            mensaje = "No se encontraron registros!";
                            ErrorCode = 200;
                        }
                        return DataReader;
                    }
                }
                
            }
            catch (SqlException ex)
            {
                throw new GpExceptions(string.Format("SqlException - {0}", ex.Message));
            }
            catch (GpExceptions ex)
            {
                throw new GpExceptions(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new GpExceptions(string.Format("Exception - {0}", ex.Message));
            }
        }
        public void OpenConnection()
        {
            try
            {
                //if (SqlConnection.State == ConnectionState.Open && SqlConnection != null)
                //{
                //    SqlConnection.Close();
                //}
                SqlConnection = new SqlConnection(ConnectionString);
                SqlConnection.Open();
                CheckConnection();
            }
            catch (SqlException ex)
            {
                throw new GpExceptions(string.Format("SqlException - {0}", ex.Message));
            }
            catch (GpExceptions ex)
            {
                throw new GpExceptions(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new GpExceptions(string.Format("Exception - {0}", ex.Message));
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
                throw new GpExceptions("No database connection");
            }
        }

        public string GetMensaje()
        {
            return mensaje;
        }
        #endregion
    }

    public class ProcedureModel
    {
        public string Namefield { get; set; }
        public object value { get; set; }
    }
}
