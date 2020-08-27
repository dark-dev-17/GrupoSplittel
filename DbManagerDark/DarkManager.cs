using DbManagerDark.DbManager;
using DbManagerDark.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace DbManagerDark
{
    public class DarkManager
    {
        #region Propiedades
        protected DarkConnectionSqlSever ConnectionSqlSever;
        protected DarkConnectionMySQL ConnectionMySQL;
        protected IConfiguration configuration;
        protected DarkMode darkMode;
        #endregion

        #region Constructores
        public DarkManager()
        {

        }
        public DarkManager(IConfiguration configuration, DarkMode darkMode)
        {
            this.configuration = configuration;
            this.darkMode = darkMode;
            Start();
        }
        #endregion

        #region metodos
        public void Start()
        {
            try
            {
                bool IsDevMode = configuration.GetConnectionString("IsDevelopment") == "true" ? true : false;
                if (DarkMode.WebServices == darkMode)
                {
                    string ConnectionSqlServer = IsDevMode ? configuration.GetConnectionString("DevSapBussinesOne") : configuration.GetConnectionString("ProductionSapBussinesOne");
                    ConnectionSqlSever = new DarkConnectionSqlSever(ConnectionSqlServer);
                }
                else if (DarkMode.Ecommerce == darkMode)
                {
                    string ConnectionMysql = IsDevMode ? configuration.GetConnectionString("DevEcommerce") : configuration.GetConnectionString("ProductionEcommerce");
                    ConnectionMySQL = new DarkConnectionMySQL(ConnectionMysql);
                }
                else if (DarkMode.Ambos == darkMode)
                {
                    string ConnectionSqlServer = IsDevMode ? configuration.GetConnectionString("DevSapBussinesOne") : configuration.GetConnectionString("ProductionSapBussinesOne");
                    ConnectionSqlSever = new DarkConnectionSqlSever(ConnectionSqlServer);

                    string ConnectionMysql = IsDevMode ? configuration.GetConnectionString("DevEcommerce") : configuration.GetConnectionString("ProductionEcommerce");
                    ConnectionMySQL = new DarkConnectionMySQL(ConnectionMysql);
                }
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
            if (DarkMode.Ecommerce == darkMode)
            {
                ConnectionMySQL.OpenConnection();
            }
            else if (DarkMode.WebServices == darkMode)
            {
                ConnectionSqlSever.OpenConnection();
            }
            else if (DarkMode.Ambos == darkMode)
            {
                ConnectionMySQL.OpenConnection();
                ConnectionSqlSever.OpenConnection();
            }
        }
        public void CloseConnection()
        {
            if (DarkMode.WebServices == darkMode)
            {
                ConnectionSqlSever.CloseDataBaseAccess();
            }
            else if (DarkMode.Ecommerce == darkMode)
            {
                ConnectionMySQL.CloseDataBaseAccess();
            }
            else if (DarkMode.Ambos == darkMode)
            {
                ConnectionMySQL.CloseDataBaseAccess();
                ConnectionSqlSever.CloseDataBaseAccess();
            }
        }
        public void StartTransaction()
        {
            if (DarkMode.WebServices == darkMode)
            {
                ConnectionSqlSever.StartTransaction();
            }
            else if (DarkMode.Ecommerce == darkMode)
            {
                ConnectionMySQL.StartTransaction();
            }
            else if (DarkMode.Ambos == darkMode)
            {
                ConnectionMySQL.StartTransaction();
                ConnectionSqlSever.StartTransaction();
            }
        }
        public void Commit()
        {
            if (DarkMode.WebServices == darkMode)
            {
                ConnectionSqlSever.Commit();
            }
            else if (DarkMode.Ecommerce == darkMode)
            {
                ConnectionMySQL.Commit();
            }
            else if (DarkMode.Ambos == darkMode)
            {
                ConnectionMySQL.Commit();
                ConnectionSqlSever.Commit();
            }
        }
        public void RolBack()
        {
            if (DarkMode.WebServices == darkMode)
            {
                ConnectionSqlSever.RolBack();
            }
            else if (DarkMode.Ecommerce == darkMode)
            {
                ConnectionMySQL.RolBack();
            }
            else if (DarkMode.Ambos == darkMode)
            {
                ConnectionMySQL.RolBack();
                ConnectionSqlSever.RolBack();
            }
        }
        #endregion
    }

    public enum DarkMode
    {
        Ecommerce = 1,
        WebServices = 2,
        Ambos = 3
    }
}
