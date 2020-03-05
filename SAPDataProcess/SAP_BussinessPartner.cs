using System;

namespace SAPDataProcess
{
    public class SAP_BussinessPartner
    {
        #region Propiedades
        private SAP_DBConnection SAP_DBConnection_;
        private SAP_DI_API SAP_DI_API_;
        #endregion

        #region Constructores
        ~SAP_BussinessPartner()
        {
            SAP_DBConnection_ = null;
            SAP_DI_API_ = null;
        }
        public SAP_BussinessPartner()
        {

        }
        public SAP_BussinessPartner(SAP_DBConnection SAP_DBConnection_)
        {
            this.SAP_DBConnection_ = SAP_DBConnection_;
        }
        public SAP_BussinessPartner(SAP_DI_API SAP_DI_API_)
        {
            this.SAP_DI_API_ = SAP_DI_API_;
        }
        public SAP_BussinessPartner(SAP_DBConnection SAP_DBConnection_,SAP_DI_API SAP_DI_API_)
        {
            this.SAP_DBConnection_ = SAP_DBConnection_;
            this.SAP_DI_API_ = SAP_DI_API_;
        }
        #endregion

        #region Metodos
        public SAPbobsCOM.BusinessPartners GetBusinessPartner(string CardCode)
        {
            try
            {
                SAP_Tools.ValidSAPDI_API(SAP_DI_API_);
                SAPbobsCOM.BusinessPartners oCustomer = (SAPbobsCOM.BusinessPartners)SAP_DI_API_.OCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
                if (oCustomer.GetByKey(CardCode))
                {
                    return oCustomer;
                }
                else
                {
                    throw new SAP_Excepcion(string.Format("Bussines Partnert '{0}' not found", CardCode));
                }
            }
            catch (SAP_Excepcion ex)
            {
                throw ex;
            }
            
        }
        public string GetPasswordDB(string CardCode)
        {
            try
            {
                string Statement = string.Format("SELECT U_PdwB2B FROM OCRD WHERE CardCode = '{0}'", CardCode);
                return SAP_DBConnection_.GetStringValue(Statement);
            }
            catch (SAP_Excepcion ex)
            {
                throw ex;
            }
            finally
            {
                
            }
        }
        #endregion
    }
}
