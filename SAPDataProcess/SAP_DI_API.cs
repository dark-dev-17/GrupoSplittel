using SAPbobsCOM;
using System;

namespace SAPDataProcess
{
    public class SAP_DI_API
    {
        #region Propiedades
        public Company OCompany { get; set; }
        public BoDataServerTypes BoDataServerTypes_ { get; set; }
        public BoSuppLangs BoSuppLangs_ { get; set; }
        public bool UseTrusted { get; set; }
        public string CompanyDB { get; set; }
        public string Password { get; set; }
        public string SLDServer { get; set; }
        public string DbUserName { get; set; }
        public string DbPassword { get; set; }
        public string UserName { get; set; }
        public string Server { get; set; }
        #endregion

        #region Constructores
        public SAP_DI_API()
        {

        }
        ~SAP_DI_API()
        {
            CloseConnection();
        }
        #endregion

        #region Metodos
        public void CloseConnection()
        {
            if (OCompany.Connected)
            {
                OCompany.Disconnect();
            }
        }
        public void OpenConnection()
        {
            OCompany = new SAPbobsCOM.Company();
            OCompany.DbServerType = BoDataServerTypes.dst_MSSQL2012;
            OCompany.Server = Server;
            OCompany.SLDServer = SLDServer;
            OCompany.UseTrusted = UseTrusted;
            OCompany.CompanyDB = CompanyDB;
            OCompany.DbUserName = DbUserName;
            OCompany.DbPassword = DbPassword;
            OCompany.UserName = UserName;
            OCompany.Password = Password;
            OCompany.language = BoSuppLangs.ln_English;

            int Result = OCompany.Connect();
            if(Result != 0)
            {
                throw new SAP_Excepcion(GetErrorMessage());
            }
        }
        public string GetErrorMessage()
        {
            int ErrorCode;
            string ErrorMessage;
            OCompany.GetLastError(out ErrorCode, out ErrorMessage);
            return string.Format("SAP DIAPI- {0}", ErrorMessage);
        }
        #endregion


    }
}
