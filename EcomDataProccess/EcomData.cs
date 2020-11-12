using EcomDataProccess.Foro;
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
        public Ecom_FilesFtp Ecom_FilesFtp;

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
        public void StartFTP(string FTP_Server, string FTP_User, string FTP_Password)
        {
            Ecom_FilesFtp = new Ecom_FilesFtp(FTP_Server, FTP_User, FTP_Password);
        }
        public bool SendMailNotification(int idProcess,string BodyHTML, string AddressesoT)
        {
            Ecom_ProcesoEmail Ecom_ProcesoEmail_ = new Ecom_ProcesoEmail(Ecom_DBEcommerce);
            if (idProcess != 0)
            {
               
                if (Ecom_ProcesoEmail_.Get(idProcess))
                {
                    if (Ecom_ProcesoEmail_.IsActive)
                    {
                        Ecom_ProcesoEmail_.ListaTo.Add(AddressesoT);
                    }
                }
            }
            return Ecom_Email_.SendMailNotification(
                BodyHTML, 
                Ecom_Tools.ConvevrtListString(Ecom_ProcesoEmail_.ListaTo), 
                Ecom_Tools.ConvevrtListString(Ecom_ProcesoEmail_.ListaCC), 
                Ecom_Tools.ConvevrtListString(Ecom_ProcesoEmail_.ListaBCC)
             );
        }
        public List<Ecom_Notificacion> GetNotificacionsEcom(int USR_IdSplinnet, int USR_IdArea)
        {
            Ecom_Notificacion_ = new Ecom_Notificacion(Ecom_DBEcommerce);
            bool AccesMaster = validPermissAction(USR_IdSplinnet, 40);
            bool AccesArea = validPermissAction(USR_IdSplinnet, 41);
            bool AccesUser = validPermissAction(USR_IdSplinnet, 42);
            List<Ecom_Notificacion> Notificaciones = new List<Ecom_Notificacion>();
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
            return Notificaciones;
        }
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
            else if (objectSource == ObjectSource.DireccionFacturacion)
            {
                Ecom_DireccionFacturacion objeto = (Ecom_DireccionFacturacion)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.ProcesoEmail)
            {
                Ecom_ProcesoEmail objeto = (Ecom_ProcesoEmail)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.ProductoDescripcion)
            {
                Ecom_ProductoDescripcion objeto = (Ecom_ProductoDescripcion)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.ProductoFichaTecnica)
            {
                Ecom_ProductoFichaTecnica objeto = (Ecom_ProductoFichaTecnica)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.ProductoPatchCord)
            {
                Ecom_ProductoPatchCord objeto = (Ecom_ProductoPatchCord)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.ProductoMPO)
            {
                Ecom_ProductoMPO objeto = (Ecom_ProductoMPO)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.ProductoPigtail)
            {
                Ecom_ProductoPigtail objeto = (Ecom_ProductoPigtail)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.ProductoCabServicio)
            {
                Ecom_ProductoCabServicio objeto = (Ecom_ProductoCabServicio)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.ProductoDistribuidor)
            {
                Ecom_ProductoDistribuidor objeto = (Ecom_ProductoDistribuidor)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.ProductoJumperConector)
            {
                Ecom_ProductoJumperConector objeto = (Ecom_ProductoJumperConector)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.ProductoJumperCable)
            {
                Ecom_ProductoJumperCable objeto = (Ecom_ProductoJumperCable)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.ProductoDistribuidorPrecon)
            {
                Ecom_ProductoDistribuidorPrecon objeto = (Ecom_ProductoDistribuidorPrecon)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.Cliente)
            {
                Ecom_Cliente objeto = (Ecom_Cliente)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.Pedido)
            {
                Ecom_Pedido objeto = (Ecom_Pedido)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.ProductoDistribuidorPrecar)
            {
                Ecom_ProductoDistribuidorPrecar objeto = (Ecom_ProductoDistribuidorPrecar)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.HomeAnuncio)
            {
                Ecom_HomeAnuncio objeto = (Ecom_HomeAnuncio)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.BlogComentario)
            {
                Ecom_BlogComentario objeto = (Ecom_BlogComentario)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.ContentFile)
            {
                Ecom_ContentFile objeto = (Ecom_ContentFile)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.ContentFileType)
            {
                Ecom_ContentFileType objeto = (Ecom_ContentFileType)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.Ecom_Pregunta)
            {
                Ecom_Pregunta objeto = (Ecom_Pregunta)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.Ecom_RespuestaPregunta)
            {
                Ecom_RespuestaPregunta objeto = (Ecom_RespuestaPregunta)Modelo;
                objeto.SetConnection(Ecom_DBEcommerce);
                return objeto;
            }
            else if (objectSource == ObjectSource.Ecom_ConsultConsult)
            {
                Ecom_ConsultConsult objeto = (Ecom_ConsultConsult)Modelo;
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
            else if (objectSource == ObjectSource.DireccionFacturacion)
            {
                return new Ecom_DireccionFacturacion(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.ProcesoEmail)
            {
                return new Ecom_ProcesoEmail(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.ProductoDescripcion)
            {
                return new Ecom_ProductoDescripcion(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.ProductoFichaTecnica)
            {
                return new Ecom_ProductoFichaTecnica(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.ProductoPatchCord)
            {
                return new Ecom_ProductoPatchCord(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.ProductoMPO)
            {
                return new Ecom_ProductoMPO(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.ProductoPigtail)
            {
                return new Ecom_ProductoPigtail(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.ProductoCabServicio)
            {
                return new Ecom_ProductoCabServicio(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.ProductoDistribuidor)
            {
                return new Ecom_ProductoDistribuidor(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.ProductoJumperCable)
            {
                return new Ecom_ProductoJumperCable(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.ProductoJumperConector)
            {
                return new Ecom_ProductoJumperConector(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.ProductoDistribuidorPrecon)
            {
                return new Ecom_ProductoDistribuidorPrecon(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.Pedido)
            {
                return new Ecom_Pedido(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.ProductoDistribuidorPrecar)
            {
                return new Ecom_ProductoDistribuidorPrecar(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.HomeAnuncio)
            {
                return new Ecom_HomeAnuncio(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.BlogComentario)
            {
                return new Ecom_BlogComentario(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.ContentFile)
            {
                return new Ecom_ContentFile(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.ContentFileType)
            {
                return new Ecom_ContentFileType(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.Ecom_Pregunta)
            {
                return new Ecom_Pregunta(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.Ecom_RespuestaPregunta)
            {
                return new Ecom_RespuestaPregunta(Ecom_DBEcommerce);
            }
            else if (objectSource == ObjectSource.Ecom_ConsultConsult)
            {
                return new Ecom_ConsultConsult(Ecom_DBEcommerce);
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
