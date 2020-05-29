using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin.Models
{
    public class Ecommerce
    {
        #region Propiedades
        private readonly string EcomConnection = ConfigurationManager.AppSettings["Ecommerce_Database"].ToString();
        private readonly string SplitConnection = ConfigurationManager.AppSettings["Splinnet_Database"].ToString();
        private readonly string SAPConnection = ConfigurationManager.AppSettings["SAP_Database"].ToString();
        private readonly string Ecommerce_Domain = ConfigurationManager.AppSettings["Ecommerce_Domain"].ToString();
        private readonly string FTP_User = ConfigurationManager.AppSettings["FTP_User"].ToString();
        private readonly string FTP_Password = ConfigurationManager.AppSettings["FTP_Password"].ToString();
        private readonly string FTP_Server = ConfigurationManager.AppSettings["FTP_Server"].ToString();
        public EcomDataProccess.EcomData ecomData;
        public SAPDataProcess.SAPData sAPData;
        private int UserId;
        private int UserArea;
        public ISession session { get; private set; }
        #endregion

        #region Constructores
        public Ecommerce()
        {

        }

        public Ecommerce(ISession session)
        {
            this.session = session;
        }

        public Ecommerce(int UserId_, int UserArea_)
        {
            this.UserId = UserId_;
            this.UserArea = UserArea_;
        }
        #endregion

        #region Metodos
        public void StartLib(LibraryEcommerce OptionLib)
        {
            if(OptionLib == LibraryEcommerce.Ecommerce)
            {
                ecomData = new EcomDataProccess.EcomData(EcomConnection, SplitConnection);
            }
            if (OptionLib == LibraryEcommerce.SAPBussinessOne)
            {
                sAPData = new SAPDataProcess.SAPData(SAPConnection);
            }
            if (OptionLib == LibraryEcommerce.FTP_Ecommerce)
            {
                ecomData.StartFTP(FTP_Server, FTP_User, FTP_Password);
            }
        }
        public bool ValidActionUser(int IdPermiss)
        {
            return ecomData.validPermissAction((int)session.GetInt32("USR_IdSplinnet") ,IdPermiss);
        }
        public List<SAPDataProcess.SAP_BussinessPartner> GetBussinessPartnerByUser()
        {
            List<SAPDataProcess.SAP_BussinessPartner> sAP_BussinessPartners = new List<SAPDataProcess.SAP_BussinessPartner>();
            EcomDataProccess.Ecom_Usuario ecom_Usuario = (EcomDataProccess.Ecom_Usuario)ecomData.GetObject(EcomDataProccess.ObjectSource.Usuario);
            ecom_Usuario.IdSplinnet = (int)session.GetInt32("USR_IdSplinnet");
            ecom_Usuario.GetIdSap();
            if(ecom_Usuario.Id_sap.Count > 0)
            {
                SAPDataProcess.SAP_BussinessPartner sAP_BussinessPartner = (SAPDataProcess.SAP_BussinessPartner)sAPData.GetObject(SAPDataProcess.SAPDataBaseObj.BussinesPartner);
                sAP_BussinessPartners = sAP_BussinessPartner.GetActivesBySalesEm(ecom_Usuario.Id_sap);
            }
            return sAP_BussinessPartners;
        }
        public void SaveNotificationLog(string Tipo, string Descripcion, string Controller,string Action_,string Parameter, string ExceptionMessage)
        {
            ecomData.SaveNotification((int)session.GetInt32("USR_IdSplinnet"), (int)session.GetInt32("USR_IdArea"),Tipo, Descripcion,Controller,Action_,Parameter,ExceptionMessage);
        }
        #endregion
    }
    public enum LibraryEcommerce
    {
        Ecommerce = 1,
        SAPBussinessOne = 2,
        FTP_Ecommerce = 3,
    }
}
