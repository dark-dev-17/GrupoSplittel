﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace EcomDataProccess
{
    public class Ecom_DBConnection
    {
        #region Propiedades
        private string ConnectionString;
        public string Message { get; set; }
        private bool IsTracsactionActive { get; set; }
        public MySqlConnection Connection { get; private set; }
        public MySqlCommand Comando { get; private set; }
        private MySqlTransaction tran;
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

        #region Nuevos
        public void StartTransaction()
        {
            tran = Connection.BeginTransaction();
            IsTracsactionActive = true;
        }
        public void Commit()
        {
            if (IsTracsactionActive == false)
            {
                throw new Ecom_Exception("Transactios are inactive");
            }
            tran.Commit();
        }
        public void RolBack()
        {
            if (IsTracsactionActive == false)
            {
                throw new Ecom_Exception("Transactios are inactive");
            }
            tran.Rollback();
            CloseConnection();
        }
        public void StartInsert(string statement, List<ProcedureModel> DataModel)
        {
            string Evaluando = "";
            try
            {
                if (DataModel == null)
                {
                    throw new Ecom_Exception("Sin parametros SP");
                }

                MySqlDataAdapter adapter = new MySqlDataAdapter();
                if (IsTracsactionActive)
                {
                    Comando = new MySqlCommand(statement, Connection, tran);
                }
                else
                {
                    Comando = new MySqlCommand(statement, Connection);
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
                                Comando.Parameters.Add(sqlParameter);
                            }
                            else
                            {
                                MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, param.value);
                                sqlParameter.Direction = ParameterDirection.Input;
                                Comando.Parameters.Add(sqlParameter);
                            }
                        }
                        else if (typeof(DateTime?) == param.value.GetType())
                        {
                            MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, DBNull.Value);
                            sqlParameter.Direction = ParameterDirection.Input;
                            Comando.Parameters.Add(sqlParameter);
                        }
                        else
                        {
                            MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, param.value);
                            sqlParameter.Direction = ParameterDirection.Input;
                            Comando.Parameters.Add(sqlParameter);
                        }
                    }
                    else
                    {
                        MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, DBNull.Value);
                        sqlParameter.Direction = ParameterDirection.Input;
                        Comando.Parameters.Add(sqlParameter);
                    }
                });

                adapter.InsertCommand = Comando;
                adapter.InsertCommand.ExecuteNonQuery();
                Message = "Registro guardado";
            }
            catch (MySqlException ex)
            {
                throw new Ecom_Exception(string.Format("SqlException - {0}", ex.Message));
            }
            catch (Ecom_Exception ex)
            {
                throw new Ecom_Exception(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new Ecom_Exception(string.Format("Exception - {0}", ex.Message));
            }
        }
        public void StartUpdate(string statement, List<ProcedureModel> DataModel)
        {

            string Evaluando = "";
            try
            {
                if (DataModel == null)
                {
                    throw new Ecom_Exception("Sin parametros SP");
                }

                MySqlDataAdapter adapter = new MySqlDataAdapter();
                if (IsTracsactionActive)
                {
                    Comando = new MySqlCommand(statement, Connection, tran);
                }
                else
                {
                    Comando = new MySqlCommand(statement, Connection);
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
                                Comando.Parameters.Add(sqlParameter);
                            }
                            else
                            {
                                MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, param.value);
                                sqlParameter.Direction = ParameterDirection.Input;
                                Comando.Parameters.Add(sqlParameter);
                            }
                        }
                        else if (typeof(DateTime?) == param.value.GetType())
                        {
                            MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, DBNull.Value);
                            sqlParameter.Direction = ParameterDirection.Input;
                            Comando.Parameters.Add(sqlParameter);
                        }
                        else
                        {
                            MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, param.value);
                            sqlParameter.Direction = ParameterDirection.Input;
                            Comando.Parameters.Add(sqlParameter);
                        }
                    }
                    else
                    {
                        MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, DBNull.Value);
                        sqlParameter.Direction = ParameterDirection.Input;
                        Comando.Parameters.Add(sqlParameter);
                    }
                });

                adapter.UpdateCommand = Comando;
                adapter.UpdateCommand.ExecuteNonQuery();
                Message = "Registro actualizado";
            }
            catch (MySqlException ex)
            {
                throw new Ecom_Exception(string.Format("SqlException - {0}", ex.Message));
            }
            catch (Ecom_Exception ex)
            {
                throw new Ecom_Exception(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new Ecom_Exception(string.Format("Exception - {0}", ex.Message));
            }
        }
        public void StartDelete(string statement, List<ProcedureModel> DataModel)
        {
            try
            {
                if (DataModel == null)
                {
                    throw new Ecom_Exception("Sin parametros SP");
                }

                MySqlDataAdapter adapter = new MySqlDataAdapter();
                if (IsTracsactionActive)
                {
                    Comando = new MySqlCommand(statement, Connection, tran);
                }
                else
                {
                    Comando = new MySqlCommand(statement, Connection);
                }

                DataModel.ForEach(param => {
                    if (typeof(int) == param.value.GetType())
                    {
                        if ((int)param.value == 0)
                        {
                            MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, DBNull.Value);
                            sqlParameter.Direction = ParameterDirection.Input;
                            Comando.Parameters.Add(sqlParameter);
                        }
                        else
                        {
                            MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, param.value);
                            sqlParameter.Direction = ParameterDirection.Input;
                            Comando.Parameters.Add(sqlParameter);
                        }
                    }
                    else
                    {
                        MySqlParameter sqlParameter = new MySqlParameter("@" + param.Namefield, param.value);
                        sqlParameter.Direction = ParameterDirection.Input;
                        Comando.Parameters.Add(sqlParameter);
                    }
                });

                adapter.DeleteCommand = Comando;
                adapter.DeleteCommand.ExecuteNonQuery();
                Message = "Registro eliminado";
            }
            catch (MySqlException ex)
            {
                throw new Ecom_Exception(string.Format("SqlException - {0}", ex.Message));
            }
            catch (Ecom_Exception ex)
            {
                throw new Ecom_Exception(string.Format("SAP_Excepcion - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new Ecom_Exception(string.Format("Exception - {0}", ex.Message));
            }
        }
        #endregion

        #region Metodos
        public void StartProcedure(string ProcedureName)
        {
            CheckConnection();
            Comando = new MySqlCommand();
            Comando.Connection = Connection;
            Comando.CommandType = CommandType.StoredProcedure;
            Comando.CommandText = ProcedureName;
        }
        public void AddParameter(object value, string variable, string type)
        {
            if (type == "DATETIME")
            {
                Comando.Parameters.AddWithValue("@" + variable, value);
                Comando.Parameters["@" + variable].Direction = ParameterDirection.Input;
            }
            else if (type == "VARCHAR")
            {
                Comando.Parameters.AddWithValue("@" + variable, value);
                Comando.Parameters["@" + variable].Direction = ParameterDirection.Input;
            }
            else if(type == "INT")
            {
                Comando.Parameters.AddWithValue("@" + variable, Convert.ToInt32(value));
                Comando.Parameters["@" + variable].Direction = ParameterDirection.Input;
            }
            else if (type == "DOUBLE")
            {
                Comando.Parameters.AddWithValue("@" + variable, value);
                Comando.Parameters["@" + variable].Direction = ParameterDirection.Input;
            }
            else if(type == "TEXT")
            {
                Comando.Parameters.AddWithValue("@" + variable, value);
                Comando.Parameters["@" + variable].Direction = ParameterDirection.Input;
            }
            else
            {
                throw new Ecom_Exception(string.Format("{0} no es valido como parametro", type));
            }
        }
        public int ExecProcedure()
        {
            try
            {
                Comando.Parameters.AddWithValue("@CodeResponse", Int32.Parse("1"));
                Comando.Parameters["@CodeResponse"].Direction = ParameterDirection.Output;

                Comando.Parameters.AddWithValue("@MessageResponse", "1");
                Comando.Parameters["@MessageResponse"].Direction = ParameterDirection.Output;

                Comando.ExecuteNonQuery();

                int RequestStatus = (int)Comando.Parameters["@CodeResponse"].Value;
                if (RequestStatus == 0)
                {
                    Message = string.Format("{0}", (string)Comando.Parameters["@MessageResponse"].Value);
                }
                else
                {
                    Message = string.Format("{0}", (string)Comando.Parameters["@MessageResponse"].Value);
                }

                return (int)RequestStatus;

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
        public double ExecuteProcedureDouble(string Parameters, string output)
        {
            MySqlCommand cmd;
            try
            {
                CheckConnection();
                cmd = new MySqlCommand();
                cmd.Connection = Connection;
                processParameters(Parameters, cmd);
                //value returns
                cmd.Parameters.AddWithValue("@" + output, double.Parse("1"));
                cmd.Parameters["@" + output].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                double RequestStatus = (double)cmd.Parameters["@" + output].Value;

                return RequestStatus;
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
        public int ExecuteProcedureInttt(string Parameters, string output)
        {
            MySqlCommand cmd;
            try
            {
                CheckConnection();
                cmd = new MySqlCommand();
                cmd.Connection = Connection;
                processParameters(Parameters, cmd);
                //value returns
                cmd.Parameters.AddWithValue("@" + output, Int32.Parse("1"));
                cmd.Parameters["@" + output].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                int RequestStatus = (int)cmd.Parameters["@" + output].Value;

                return RequestStatus;
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
        public MySqlDataReader ExecuteStoreProcedureReader(string Parameters)
        {
            try
            {
                CheckConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Connection;
                processParameters(Parameters, cmd);
                MySqlDataReader dataReader = cmd.ExecuteReader();
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
        public int ExecuteStoreProcedure(string Parameters)
        {
            MySqlCommand cmd;
            try
            {
                CheckConnection();
                cmd = new MySqlCommand();
                cmd.Connection = Connection;
                processParameters(Parameters, cmd);

                cmd.Parameters.AddWithValue("@CodeResponse", Int32.Parse("1"));
                cmd.Parameters["@CodeResponse"].Direction = ParameterDirection.Output;

                cmd.Parameters.AddWithValue("@MessageResponse", "1");
                cmd.Parameters["@MessageResponse"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                int RequestStatus = (int)cmd.Parameters["@CodeResponse"].Value;
                if(RequestStatus == 0)
                {
                    Message = string.Format("{0}",(string)cmd.Parameters["@MessageResponse"].Value);
                }
                else
                {
                    Message = string.Format("{0}", (string)cmd.Parameters["@MessageResponse"].Value);
                }
                return (int)RequestStatus;
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
        private void processParameters(string Parameters, MySqlCommand cmd)
        {
            string ProcedureName = Parameters.Split('|')[0];
            string[] Paramet = Parameters.Split('|')[1].Split('&');

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = ProcedureName;

            foreach (string item in Paramet)
            {
                string variable = item.Split('@')[0];
                string variableValue = item.Split('@')[1];
                string type = variableValue.Split('=')[0];
                string value = variableValue.Split('=')[1];

                if (type == "DATETIME")
                {
                    cmd.Parameters.AddWithValue("@" + variable, DateTime.Parse(value));
                    cmd.Parameters["@" + variable].Direction = ParameterDirection.Input;
                }
                if (type == "VARCHAR")
                {
                    cmd.Parameters.AddWithValue("@" + variable, value);
                    cmd.Parameters["@" + variable].Direction = ParameterDirection.Input;
                }
                if (type == "INT")
                {
                    cmd.Parameters.AddWithValue("@" + variable, Int32.Parse(value));
                    cmd.Parameters["@" + variable].Direction = ParameterDirection.Input;
                }
                if (type == "DOUBLE")
                {
                    cmd.Parameters.AddWithValue("@" + variable, double.Parse(value));
                    cmd.Parameters["@" + variable].Direction = ParameterDirection.Input;
                }
                if (type == "TEXT")
                {
                    cmd.Parameters.AddWithValue("@" + variable, value);
                    cmd.Parameters["@" + variable].Direction = ParameterDirection.Input;
                }
            }
        }
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
        public string ExecuteScalarString(string stattement)
        {
            try
            {
                CheckConnection();
                MySqlCommand cmd = new MySqlCommand(stattement, Connection);
                object result = (object)cmd.ExecuteScalar();
                if (result is System.DBNull || result == null)
                {
                    return "";
                }
                else
                {
                    if (result.GetType() == typeof(string))
                    {
                        return Convert.ToString(result);
                    }
                    return (string)result;
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
        public int CountDataReader(MySqlDataReader dataReader)
        {
            int count = 0;
            MySqlDataReader dataReader1 = dataReader;
            while (dataReader1.Read())
            {
                count++;
            }
            return count;
        }
        public bool ExecuteProcedure(string Parameters, string returnValue)
        {
            try
            {
                CheckConnection();
                string ProcedureName = Parameters.Split('|')[0];
                string[] Paramet = Parameters.Split('|')[1].Split('&');
                MySqlCommand cmd = new MySqlCommand(ProcedureName, Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                //campo @ typevar = valor &
                foreach (string item in Paramet)
                {
                    string variable = item.Split('@')[0];
                    string type = item.Split('@')[1].Split('=')[0];
                    string value = item.Split('@')[1].Split('=')[1];
                    cmd.Parameters.AddWithValue("@" + variable, value);
                }
                MySqlDataReader dataReader = cmd.ExecuteReader();
                dataReader.Close();
                return false;
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
        #endregion
    }

    public class ProcedureModel
    {
        public string Namefield { get; set; }
        public object value { get; set; }
    }
}
