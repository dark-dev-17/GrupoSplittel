using EcomDataProccess;
using SAPDataProcess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin.Models
{
    public class Ecom_SAP
    {
        private readonly string EcomConnection = ConfigurationManager.AppSettings["Ecommerce_Database"].ToString();
        private readonly string SplitConnection = ConfigurationManager.AppSettings["Splinnet_Database"].ToString();
        private readonly string SAPConnection = ConfigurationManager.AppSettings["SAP_Database"].ToString();
        public SAP_DBConnection SAP_DBConnection_ { get; private set; }
        public Ecom_DBConnection Ecom_DBConnection_ { get; private set; }
        private string Mode;
        public List<SAPDataProcess.SAP_BussinessPartner> GetBussPartByEmp(int idSplinnet)
        {
            Ecom_DBConnection Ecom_DBConnection_ = null;
            SAPDataProcess.SAP_DBConnection SAP_DBConnection_ = null;
            List <SAPDataProcess.SAP_BussinessPartner> List = null;
            try
            {
                // obtener id de sap de empleado
                Ecom_DBConnection_ = new Ecom_DBConnection(SplitConnection);
                Ecom_DBConnection_.OpenConnection();
                Ecom_Usuario Ecom_Usuario_ = new Ecom_Usuario(Ecom_DBConnection_);
                bool IsExists = Ecom_Usuario_.Get(idSplinnet);
                Ecom_Usuario_.GetIdSap();
                Ecom_DBConnection_.CloseConnection();

                // obtener clientes de sap
                SAP_DBConnection_ = new SAPDataProcess.SAP_DBConnection(SAPConnection);
                SAP_DBConnection_.OpenConnection();
                List = new SAPDataProcess.SAP_BussinessPartner(SAP_DBConnection_).GetActivesBySalesEm(Ecom_Usuario_.Id_sap);
                SAP_DBConnection_.CloseDataBaseAccess();

                return List;
            }
            catch (EcomDataProccess.Ecom_Exception ex)
            {
                throw ex;
            }
            catch (SAPDataProcess.SAP_Excepcion ex)
            {
                throw ex;
            }
            finally
            {
                if (Ecom_DBConnection_ != null)
                {
                    Ecom_DBConnection_.CloseConnection();
                }
                if (SAP_DBConnection_ != null)
                {
                    SAP_DBConnection_.CloseDataBaseAccess();
                }
            }
        }
        
        ~Ecom_SAP()
        {
            if(SAP_DBConnection_ != null)
            {
                SAP_DBConnection_.CloseDataBaseAccess();
                SAP_DBConnection_ = null;
            }
            if (Ecom_DBConnection_ != null)
            {
                Ecom_DBConnection_.CloseConnection();
                Ecom_DBConnection_ = null;
            }
        }
        public Ecom_SAP()
        {

        }
        public Ecom_SAP(string mode)
        {
            Mode = mode;
            if (mode == "SAP")
            {
                SAP_DBConnection_ = new SAP_DBConnection(SAPConnection);
                SAP_DBConnection_.OpenConnection();
            }
            else if(mode == "Ecommerce")
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                Ecom_DBConnection_.OpenConnection();
            }
            else if(mode == "Ambas")
            {
                SAP_DBConnection_ = new SAP_DBConnection(SAPConnection);
                SAP_DBConnection_.OpenConnection();
                Ecom_DBConnection_ = new Ecom_DBConnection(EcomConnection);
                Ecom_DBConnection_.OpenConnection();
            }
            else
            {

            }
        }
        public List<Ecom_Pedido> GetEcom_Pedidos(string Tipo,string Cardcode)
        {
            try
            {
                if(Tipo == "Cotizacion")
                {
                    return new Ecom_Pedido(Ecom_DBConnection_).GetCotizacion(Cardcode);
                }
                else if (Tipo == "Pendiente")
                {
                    return new Ecom_Pedido(Ecom_DBConnection_).GetPending(Cardcode);
                }
                else if (Tipo == "Historico")
                {
                    return new Ecom_Pedido(Ecom_DBConnection_).GetByBussinessPartner(Cardcode);
                }
                else
                {
                    return null;
                }
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public List<SAP_Document> GetSAP_Document(string Tipo, string Cardcode)
        {
            try
            {
                if (Tipo == "EnProceso")
                {
                    return new SAP_Document(SAP_DBConnection_).GetInProcess(Cardcode);
                }
                else if (Tipo == "Rechazado")
                {
                    return new SAP_Document(SAP_DBConnection_).GetRejected(Cardcode);
                }
                else if (Tipo == "Historico")
                {
                    return new SAP_Document(SAP_DBConnection_).GetHistoric(Cardcode);
                }
                else
                {
                    return null;
                }
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public List<Ecom_Cliente> GetEcom_Cliente(string Cardcode)
        {
            return new Ecom_Cliente(Ecom_DBConnection_).Get(Cardcode);
        }
        public void CloseConections()
        {
            if (SAP_DBConnection_ != null)
            {
                SAP_DBConnection_.CloseDataBaseAccess();
                SAP_DBConnection_ = null;
            }
            if (Ecom_DBConnection_ != null)
            {
                Ecom_DBConnection_.CloseConnection();
                Ecom_DBConnection_ = null;
            }
        }
        public bool ValidAction(int id_user,int[] action)
        {
            
            try
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(SplitConnection);
                Ecom_DBConnection_.OpenConnection();
                return new Ecom_Usuario(Ecom_DBConnection_).AccessToAction(id_user, action);
            }
            catch (Ecom_Exception)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (Ecom_DBConnection_ != null)
                    Ecom_DBConnection_.CloseConnection();
            }
        }
        public List<Ecom_Modelo> Menu(int id_user)
        {
            List<Ecom_Modelo> List = new List<Ecom_Modelo>();
            try
            {
                Ecom_DBConnection_ = new Ecom_DBConnection(SplitConnection);
                Ecom_DBConnection_.OpenConnection();
                return new Ecom_Modelo(Ecom_DBConnection_).Get(id_user);
            }
            catch (Ecom_Exception)
            {
                return List;
            }
            catch (Exception)
            {
                return List;
            }
            finally
            {
                if (Ecom_DBConnection_ != null)
                    Ecom_DBConnection_.CloseConnection();
            }
        }
    }
}
