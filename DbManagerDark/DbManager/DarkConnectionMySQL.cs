using DbManagerDark.Exceptions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DbManagerDark.DbManager
{
    public class DarkConnectionMySQL
    {
        #region Propiedades
        private string ConnectionString;
        private DataTable DataTable;
        private MySqlConnection SqlConnection;
        private MySqlCommand Command;
        public string mensaje;
        public int ErrorCode;
        public bool IsTracsactionActive = false;
        private MySqlTransaction tran;
        #endregion

        #region Constructores
        public DarkConnectionMySQL()
        {

        }
        public DarkConnectionMySQL(string ConnectionString)
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
            if (IsTracsactionActive == false)
            {
                throw new DarkExceptionSystem("Transactios are inactive");
            }
            tran.Commit();
        }
        public void RolBack()
        {
            if (IsTracsactionActive == false)
            {
                throw new DarkExceptionSystem("Transactios are inactive");
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
                    throw new DarkExceptionSystem("Sin parametros SP");
                }

                MySqlDataAdapter adapter = new MySqlDataAdapter();
                if (IsTracsactionActive)
                {
                    Command = new MySqlCommand(statement, SqlConnection, tran);
                }
                else
                {
                    Command = new MySqlCommand(statement, SqlConnection);
                }

                DataModel.ForEach(param => {
                    Evaluando = param.Namefield;
                    if (param.value != null)
                    {
                        if (typeof(int) == param.value.GetType())
                        {
                            if ((int)param.value == 0)
                            {
                                MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, DBNull.Value);
                                sqlParameter.Direction = ParameterDirection.Input;
                                Command.Parameters.Add(sqlParameter);
                            }
                            else
                            {
                                MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, param.value);
                                sqlParameter.Direction = ParameterDirection.Input;
                                Command.Parameters.Add(sqlParameter);
                            }
                        }
                        else if (typeof(DateTime?) == param.value.GetType())
                        {
                            MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, DBNull.Value);
                            sqlParameter.Direction = ParameterDirection.Input;
                            Command.Parameters.Add(sqlParameter);
                        }
                        else
                        {
                            MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, param.value);
                            sqlParameter.Direction = ParameterDirection.Input;
                            Command.Parameters.Add(sqlParameter);
                        }
                    }
                    else
                    {
                        MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, DBNull.Value);
                        sqlParameter.Direction = ParameterDirection.Input;
                        Command.Parameters.Add(sqlParameter);
                    }
                });

                adapter.InsertCommand = Command;
                adapter.InsertCommand.ExecuteNonQuery();
                mensaje = "Registro guardado";
            }
            catch (MySqlException ex)
            {
                throw new DarkExceptionSystem(string.Format("SqlException - {0}", ex.Message));
            }
            catch (DarkExceptionSystem ex)
            {
                throw new DarkExceptionSystem(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new DarkExceptionSystem(string.Format("Exception - {0}", ex.Message));
            }
        }
        public void StartUpdate(string statement, List<ProcedureModel> DataModel)
        {

            string Evaluando = "";
            try
            {
                if (DataModel == null)
                {
                    throw new DarkExceptionSystem("Sin parametros SP");
                }

                MySqlDataAdapter adapter = new MySqlDataAdapter();
                if (IsTracsactionActive)
                {
                    Command = new MySqlCommand(statement, SqlConnection, tran);
                }
                else
                {
                    Command = new MySqlCommand(statement, SqlConnection);
                }
                DataModel.ForEach(param => {
                    Evaluando = param.Namefield;
                    if (param.value != null)
                    {
                        if (typeof(int) == param.value.GetType())
                        {
                            if ((int)param.value == 0)
                            {
                                MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, DBNull.Value);
                                sqlParameter.Direction = ParameterDirection.Input;
                                Command.Parameters.Add(sqlParameter);
                            }
                            else
                            {
                                MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, param.value);
                                sqlParameter.Direction = ParameterDirection.Input;
                                Command.Parameters.Add(sqlParameter);
                            }
                        }
                        else if (typeof(DateTime?) == param.value.GetType())
                        {
                            MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, DBNull.Value);
                            sqlParameter.Direction = ParameterDirection.Input;
                            Command.Parameters.Add(sqlParameter);
                        }
                        else
                        {
                            MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, param.value);
                            sqlParameter.Direction = ParameterDirection.Input;
                            Command.Parameters.Add(sqlParameter);
                        }
                    }
                    else
                    {
                        MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, DBNull.Value);
                        sqlParameter.Direction = ParameterDirection.Input;
                        Command.Parameters.Add(sqlParameter);
                    }
                });

                adapter.UpdateCommand = Command;
                adapter.UpdateCommand.ExecuteNonQuery();
                mensaje = "Registro actualizado";
            }
            catch (MySqlException ex)
            {
                throw new DarkExceptionSystem(string.Format("SqlException - {0}", ex.Message));
            }
            catch (DarkExceptionSystem ex)
            {
                throw new DarkExceptionSystem(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new DarkExceptionSystem(string.Format("Exception - {0}", ex.Message));
            }
        }
        public void StartDelete(string statement, List<ProcedureModel> DataModel)
        {
            try
            {
                if (DataModel == null)
                {
                    throw new DarkExceptionSystem("Sin parametros SP");
                }

                MySqlDataAdapter adapter = new MySqlDataAdapter();
                if (IsTracsactionActive)
                {
                    Command = new MySqlCommand(statement, SqlConnection, tran);
                }
                else
                {
                    Command = new MySqlCommand(statement, SqlConnection);
                }
                DataModel.ForEach(param => {
                    if (typeof(int) == param.value.GetType())
                    {
                        if ((int)param.value == 0)
                        {
                            MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, DBNull.Value);
                            sqlParameter.Direction = ParameterDirection.Input;
                            Command.Parameters.Add(sqlParameter);
                        }
                        else
                        {
                            MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, param.value);
                            sqlParameter.Direction = ParameterDirection.Input;
                            Command.Parameters.Add(sqlParameter);
                        }
                    }
                    else
                    {
                        MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, param.value);
                        sqlParameter.Direction = ParameterDirection.Input;
                        Command.Parameters.Add(sqlParameter);
                    }
                });

                adapter.DeleteCommand = Command;
                adapter.DeleteCommand.ExecuteNonQuery();
                mensaje = "Registro eliminado";
            }
            catch (MySqlException ex)
            {
                throw new DarkExceptionSystem(string.Format("SqlException - {0}", ex.Message));
            }
            catch (DarkExceptionSystem ex)
            {
                throw new DarkExceptionSystem(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new DarkExceptionSystem(string.Format("Exception - {0}", ex.Message));
            }
        }
        public void StartProcedure(string ProcedureName, List<ProcedureModel> DataModel)
        {
            Command = new MySqlCommand(ProcedureName, SqlConnection);
            Command.CommandType = CommandType.StoredProcedure;

            if (DataModel == null)
            {
                throw new DarkExceptionSystem("Sin parametros SP");
            }
            try
            {
                DataModel.ForEach(param => {
                    MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, param.value);
                    sqlParameter.Direction = ParameterDirection.Input;
                    Command.Parameters.Add(sqlParameter);
                });

                var MessageCode = Command.Parameters.Add("@MessageCode", MySqlDbType.Int32);
                MessageCode.Direction = ParameterDirection.Output;
                var MessageValue = Command.Parameters.Add("@MessageValue", MySqlDbType.VarChar, 200);
                MessageValue.Direction = ParameterDirection.Output;
                Command.ExecuteNonQuery();

                ErrorCode = (int)Command.Parameters["@MessageCode"].Value;
                mensaje = (string)Command.Parameters["@MessageValue"].Value;
            }
            catch (MySqlException ex)
            {
                throw new DarkExceptionSystem(string.Format("SqlException - {0}", ex.Message));
            }
            catch (DarkExceptionSystem ex)
            {
                throw new DarkExceptionSystem(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new DarkExceptionSystem(string.Format("Exception - {0}", ex.Message));
            }


        }
        public DataTable GetData(string sqlStatement)
        {
            try
            {
                CheckConnection();
                using (MySqlCommand MySqlCommand = new MySqlCommand(sqlStatement, SqlConnection))
                {
                    MySqlCommand.CommandTimeout = 120;
                    using (MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(MySqlCommand))
                    {
                        DataTable = new DataTable();
                        sqlDataAdapter.Fill(DataTable);
                        sqlDataAdapter.Dispose();
                        return DataTable;
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new DarkExceptionSystem(string.Format("SqlException - {0}", ex.Message));
            }
            catch (DarkExceptionSystem ex)
            {
                throw new DarkExceptionSystem(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new DarkExceptionSystem(string.Format("Exception - {0}", ex.Message));
            }
        }
        public int GetIntegerValue(string sqlStatement)
        {
            try
            {
                CheckConnection();
                if (IsTracsactionActive)
                {
                    using (MySqlCommand MySqlCommand = new MySqlCommand(sqlStatement, SqlConnection, tran))
                    {
                        return string.IsNullOrEmpty(MySqlCommand.ExecuteScalar().ToString()) ? 0 : int.Parse(MySqlCommand.ExecuteScalar().ToString());
                    }
                }
                else
                {
                    using (MySqlCommand MySqlCommand = new MySqlCommand(sqlStatement, SqlConnection))
                    {
                        return string.IsNullOrEmpty(MySqlCommand.ExecuteScalar().ToString()) ? 0 : int.Parse(MySqlCommand.ExecuteScalar().ToString());
                    }
                }

            }
            catch (MySqlException ex)
            {
                throw new DarkExceptionSystem(string.Format("SqlException - {0}", ex.Message));
            }
            catch (DarkExceptionSystem ex)
            {
                throw new DarkExceptionSystem(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new DarkExceptionSystem(string.Format("Exception - {0}", ex.Message));
            }
        }
        public string GetStringValue(string sqlStatement)
        {
            try
            {
                CheckConnection();
                if (IsTracsactionActive)
                {
                    using (MySqlCommand MySqlCommand = new MySqlCommand(sqlStatement, SqlConnection, tran))
                    {
                        return MySqlCommand.ExecuteScalar().ToString();
                    }
                }
                else
                {
                    using (MySqlCommand MySqlCommand = new MySqlCommand(sqlStatement, SqlConnection))
                    {
                        return MySqlCommand.ExecuteScalar().ToString();
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new DarkExceptionSystem(string.Format("SqlException - {0}", ex.Message));
            }
            catch (DarkExceptionSystem ex)
            {
                throw new DarkExceptionSystem(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new DarkExceptionSystem(string.Format("Exception - {0}", ex.Message));
            }
        }
        public double GetDoublelValue(string sqlStatement)
        {
            try
            {
                CheckConnection();
                if (IsTracsactionActive)
                {
                    using (MySqlCommand MySqlCommand = new MySqlCommand(sqlStatement, SqlConnection, tran))
                    {
                        return string.IsNullOrEmpty(MySqlCommand.ExecuteScalar().ToString()) ? 0 : double.Parse(MySqlCommand.ExecuteScalar().ToString());
                    }
                }
                else
                {
                    using (MySqlCommand MySqlCommand = new MySqlCommand(sqlStatement, SqlConnection))
                    {
                        return string.IsNullOrEmpty(MySqlCommand.ExecuteScalar().ToString()) ? 0 : double.Parse(MySqlCommand.ExecuteScalar().ToString());
                    }
                }

            }
            catch (MySqlException ex)
            {
                throw new DarkExceptionSystem(string.Format("SqlException - {0}", ex.Message));
            }
            catch (DarkExceptionSystem ex)
            {
                throw new DarkExceptionSystem(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new DarkExceptionSystem(string.Format("Exception - {0}", ex.Message));
            }
        }
        public DateTime GetDateTimeValue(string sqlStatement)
        {
            try
            {
                CheckConnection();
                if (IsTracsactionActive)
                {
                    using (MySqlCommand MySqlCommand = new MySqlCommand(sqlStatement, SqlConnection, tran))
                    {
                        return DateTime.Parse(MySqlCommand.ExecuteScalar().ToString());
                    }
                }
                else
                {
                    using (MySqlCommand MySqlCommand = new MySqlCommand(sqlStatement, SqlConnection))
                    {
                        return DateTime.Parse(MySqlCommand.ExecuteScalar().ToString());
                    }
                }

            }
            catch (MySqlException ex)
            {
                throw new DarkExceptionSystem(string.Format("SqlException - {0}", ex.Message));
            }
            catch (DarkExceptionSystem ex)
            {
                throw new DarkExceptionSystem(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new DarkExceptionSystem(string.Format("Exception - {0}", ex.Message));
            }
        }
        public MySqlDataReader GetDataReader(string sqlStatement)
        {
            try
            {
                CheckConnection();
                if (IsTracsactionActive)
                {
                    using (MySqlCommand MySqlCommand = new MySqlCommand(sqlStatement, SqlConnection, tran))
                    {
                        MySqlCommand.CommandTimeout = 120;
                        MySqlDataReader DataReader = MySqlCommand.ExecuteReader();
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
                    using (MySqlCommand MySqlCommand = new MySqlCommand(sqlStatement, SqlConnection))
                    {
                        MySqlCommand.CommandTimeout = 120;
                        MySqlDataReader DataReader = MySqlCommand.ExecuteReader();
                        if (!DataReader.HasRows)
                        {
                            mensaje = "No se encontraron registros!";
                            ErrorCode = 200;
                        }
                        return DataReader;
                    }
                }

            }
            catch (MySqlException ex)
            {
                throw new DarkExceptionSystem(string.Format("SqlException - {0}", ex.Message));
            }
            catch (DarkExceptionSystem ex)
            {
                throw new DarkExceptionSystem(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new DarkExceptionSystem(string.Format("Exception - {0}", ex.Message));
            }
        }
        public void OpenConnection()
        {
            SqlConnection = new MySqlConnection(ConnectionString);
            try
            {
                SqlConnection.Open();
                CheckConnection();
            }
            catch (MySqlException ex)
            {
                throw new DarkExceptionSystem(string.Format("SqlException - {0}", ex.Message));
            }
            catch (DarkExceptionSystem ex)
            {
                throw new DarkExceptionSystem(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new DarkExceptionSystem(string.Format("Exception - {0}", ex.Message));
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
                throw new DarkExceptionSystem("No database connection");
            }
        }
        public string GetMensaje()
        {
            return mensaje;
        }
        #endregion
    }
}
