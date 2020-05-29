using System;

namespace SAPDataProcess
{
    public class SAPData
    {
        #region Propiedades
        private string SAPStringConnection;
        private SAP_DBConnection SAP_DBConnection_;
        #endregion

        #region Constructores
        public SAPData()
        {

        }
        public SAPData(string SAPStringConnection_)
        {
            this.SAPStringConnection = SAPStringConnection_;
        }
        #endregion

        #region Metodos
        public object GetObject(SAPDataBaseObj sAPDataBaseObj)
        {
            if(SAP_DBConnection_ == null)
            {
                throw new SAP_Excepcion("Please open conection to Database refrences to 'SAP_DBConnection_' object");
            }
            if(sAPDataBaseObj == SAPDataBaseObj.Document)
            {
                return new SAP_Document(SAP_DBConnection_);
            }
            if (sAPDataBaseObj == SAPDataBaseObj.BussinesPartner)
            {
                return new SAP_BussinessPartner(SAP_DBConnection_);
            }
            if (sAPDataBaseObj == SAPDataBaseObj.VendorGroup)
            {
                return new SAP_VendorGroup(SAP_DBConnection_);
            }
            else
            {
                return null;
            }
        }
        public void OpenConnection(ConnectionSAP connectionSAP)
        {
            if(connectionSAP == ConnectionSAP.Database)
            {
                if (string.IsNullOrEmpty(SAPStringConnection))
                {
                    throw new SAP_Excepcion("Please set connection string to the variable 'SAPStringConnection'");
                }
                SAP_DBConnection_ = new SAP_DBConnection(SAPStringConnection);
                SAP_DBConnection_.OpenConnection();
            }
        }
        public void CloseConnection(ConnectionSAP connectionSAP)
        {
            if (connectionSAP == ConnectionSAP.Database)
            {
                if (SAP_DBConnection_ != null)
                {
                    SAP_DBConnection_.CloseDataBaseAccess();
                }
            }
        }
        #endregion
    }
}
