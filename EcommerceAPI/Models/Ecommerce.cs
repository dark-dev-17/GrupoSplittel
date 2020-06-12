using EcomDataProccess;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.Models
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
        #endregion

        #region Metodos
        public void StartLib(LibraryEcommerce OptionLib)
        {
            if (OptionLib == LibraryEcommerce.Ecommerce)
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
            return ecomData.validPermissAction((int)session.GetInt32("USR_IdSplinnet"), IdPermiss);
        }
        public void ReleaseObjects()
        {
            if (ecomData != null)
            {
                ecomData.Disconect(ServerSource.Ecommerce);
                ecomData.Disconect(ServerSource.Splitnet);
            }
            if (sAPData != null)
            {
                sAPData.CloseConnection(SAPDataProcess.ConnectionSAP.Database);
            }
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
