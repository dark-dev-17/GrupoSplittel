using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace EcomDataProccess
{
    public class EcomData
    {
        #region Propiedades
        private string EcomConnection { set; get; }
        private string SplitConnection { set; get; }
        private Ecom_DBConnection Ecom_DBEcommerce;
        private Ecom_DBConnection Ecom_DBSplittel;
        public Ecom_Email Ecom_Email_;
        private Ecom_Notificacion Ecom_Notificacion_;
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

        #region Metodos
        public List<Ecom_Notificacion> getNotifications(int USR_IdSplinnet, int USR_IdArea)
        {
            Connect(ServerSource.Ecommerce);
            Ecom_Notificacion_ = new Ecom_Notificacion(Ecom_DBEcommerce);
            Connect(ServerSource.Splitnet);
            bool AccesMaster = validPermissAction(USR_IdSplinnet, 40);
            bool AccesArea = validPermissAction(USR_IdSplinnet, 41);
            bool AccesUser = validPermissAction(USR_IdSplinnet, 42);
            List<Ecom_Notificacion> Notificaciones =  new List<Ecom_Notificacion>();
            if (AccesMaster)
            {
                Notificaciones = Ecom_Notificacion_.GetTipo("info");
            }
            if (AccesArea)
            {
                Notificaciones = Ecom_Notificacion_.GetArea(USR_IdArea);
            }
            if (AccesUser)
            {
                Notificaciones = Ecom_Notificacion_.GetUsuario(USR_IdSplinnet);
            }
            
            if (Notificaciones.Count > 0)
            {
                
                
                Notificaciones.ForEach(not => {
                    EcomDataProccess.Ecom_Usuario Ecom_Usuario = (EcomDataProccess.Ecom_Usuario)GetObject(EcomDataProccess.ObjectSource.Usuario);
                    Ecom_Usuario.Get(not.Usuario);
                    not.Ecom_Usuario_ = Ecom_Usuario;
                });
                
            }
            Disconect(ServerSource.Ecommerce);
            Disconect(ServerSource.Splitnet);
            return Notificaciones;
        }
        public void SaveNotification(int Usuario,int Area, string Tipo, string Descripcion, string Controller, string Action, string Parameter, string ExceptionMessage)
        {
            Ecom_Notificacion_ = new Ecom_Notificacion(Ecom_DBEcommerce);
            Ecom_Notificacion_.Usuario = Usuario;
            Ecom_Notificacion_.Tipo = Tipo;
            Ecom_Notificacion_.Area = Area;
            Ecom_Notificacion_.Descripcion = Descripcion;
            Ecom_Notificacion_.Controller = Controller;
            Ecom_Notificacion_.Action = Action;
            Ecom_Notificacion_.Parameter = Parameter;
            Ecom_Notificacion_.ExceptionMessage = ExceptionMessage;
            Ecom_Notificacion_.Add();
        }
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
            else if (objectSource == ObjectSource.PedidoLine)
            {
                Ecom_PedidoLine objeto = (Ecom_PedidoLine)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.Usuario)
            {
                Ecom_Usuario objeto = (Ecom_Usuario)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.UsuarioArea)
            {
                Ecom_UsuarioArea objeto = (Ecom_UsuarioArea)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.Notificacion)
            {
                Ecom_Notificacion objeto = (Ecom_Notificacion)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.DireccionEnvio)
            {
                Ecom_DireccionEnvio objeto = (Ecom_DireccionEnvio)Modelo;
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
            else if (objectSource == ObjectSource.PedidoLine)
            {
                return new Ecom_PedidoLine(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.Pedido)
            {
                return new Ecom_Pedido(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.Cliente)
            {
                return new Ecom_Cliente(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.Usuario)
            {
                return new Ecom_Usuario(Ecom_DBSplittel);
            }
            else if (objectSource == ObjectSource.UsuarioArea)
            {
                return new Ecom_UsuarioArea(Ecom_DBSplittel);
            }
            else if (objectSource == ObjectSource.Notificacion)
            {
                return new Ecom_Notificacion(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.DireccionEnvio)
            {
                return new Ecom_DireccionEnvio(Ecom_DBEcommerce);
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
        public Ecom_DBConnection GetConnection(ServerSource serverSource)
        {
            if (serverSource == ServerSource.Ecommerce)
            {
                return Ecom_DBEcommerce;
            }
            else if (serverSource == ServerSource.Splitnet)
            {
                return Ecom_DBSplittel;
            }
            else
            {
                return null;
            }
        }
        #endregion
        
    }
}
