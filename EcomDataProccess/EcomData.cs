using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace EcomDataProccess
{
    public class EcomData
    {
        #region Metodos
        public bool validPermissAction(int USR_IdSplinnet, int action)
        {
            try
            {
                return new Ecom_Usuario(Ecom_DBSplittel).AccessToAction(USR_IdSplinnet, action);
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public object SetObjectConnection(object Modelo, ObjectSource objectSource)
        {
            if (objectSource == ObjectSource.ProductoCategoria)
            {
                Ecom_ProductoCategoria objeto = (Ecom_ProductoCategoria)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.ProductoSubcategoria)
            {
                Ecom_ProductoSubCategoria objeto = (Ecom_ProductoSubCategoria)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.Blog)
            {
                Ecom_Blog objeto = (Ecom_Blog)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else
            {
                throw new Ecom_Exception(string.Format("Objeto no valido"));
            }
        }
        public object GetObject(ObjectSource objectSource)
        {
            if(objectSource == ObjectSource.ProductoConfigurable)
            {
                return new Ecom_ProductoConfigurable(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.ProductoFijo)
            {
                return new Ecom_Producto(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.ProductoCategoria)
            {
                return new Ecom_ProductoCategoria(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.ProductoSubcategoria)
            {
                return new Ecom_ProductoSubCategoria(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.Blog)
            {
                return new Ecom_Blog(Ecom_DBEcommerce);
            }
            else
            {
                throw new Ecom_Exception(string.Format("Objeto no valido"));
            }
        }
        public void Connect(ServerSource serverSource)
        {
            try
            {
                if (serverSource == ServerSource.Ecommerce)
                {
                    Ecom_Tools.ValidStringParameter(EcomConnection, "EcomConnection");
                    Ecom_DBEcommerce = new Ecom_DBConnection(EcomConnection);
                    Ecom_DBEcommerce.OpenConnection();
                }
                if (serverSource == ServerSource.Splitnet)
                {
                    Ecom_Tools.ValidStringParameter(SplitConnection, "SplitConnection");
                    Ecom_DBSplittel = new Ecom_DBConnection(SplitConnection);
                    Ecom_DBSplittel.OpenConnection();
                }
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public void Disconect(ServerSource serverSource)
        {
            if (serverSource == ServerSource.Ecommerce)
            {
                if (Ecom_DBEcommerce != null)
                {
                    Ecom_DBEcommerce.CloseConnection();
                }
            }
            if (serverSource == ServerSource.Splitnet)
            {
                if (Ecom_DBSplittel != null)
                {
                    Ecom_DBSplittel.CloseConnection();
                }
            }
        }
        public string GetLastMessage(ServerSource serverSource)
        {
            if (serverSource == ServerSource.Ecommerce)
            {
                return Ecom_DBEcommerce.Message;
            }
            else if (serverSource == ServerSource.Splitnet)
            {
                return Ecom_DBSplittel.Message;
            }
            else
            {
                return "";
            }
        }
        #endregion
        #region Constructores
        public EcomData()
        {
        }
        public EcomData(string EcomConnection, string SplitConnection)
        {
            this.EcomConnection = EcomConnection;
            this.SplitConnection = SplitConnection;
        }
        #endregion
        #region Propiedades
        private string EcomConnection { set; get; }
        private string SplitConnection { set; get; }
        private Ecom_DBConnection Ecom_DBEcommerce;
        private Ecom_DBConnection Ecom_DBSplittel;
        
        #endregion
    }
}
