using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SAPDataProcess
{
    public class SAP_BussinessPartner
    {
        #region Propiedades
        public string CardCode { set; get; }
        public string CardName { set; get; }
        public string ExtraDays { set; get; }
        public string DescriptPayment { set; get; }
        public double CreditLine { set; get; }
        public double Balance { set; get; }
        public string Phone2 { set; get; }
        public string E_Mail { set; get; }
        public string E_MailL_invoice { set; get; }
        public string E_MailL_account { set; get; }
        public string SlpName { set; get; }
        public string Email_employeSales { set; get; }
        public string Section { set; get; }
        public string MonexUSD { set; get; }
        public string MonexMXP { set; get; }
        public string Currency { set; get; }
        public bool IsActive { set; get; }
        public bool IsActiveEcomerce { set; get; }
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
        public List<SAP_BussinessPartner> GetActivesBySalesEm(List<int> IdsEmployee)
        {
            List<SAP_BussinessPartner> List = null;
            SqlDataReader data = null;

            try
            {
                SAP_Tools.ValidSQLConnection(SAP_DBConnection_);
                List = new List<SAP_BussinessPartner>();
                foreach (int IdEmployee in IdsEmployee)
                {
                    string SqlStatement = string.Format("EXEC [Eco_getCustomerBYSalesEmployer] @SlpCode = '{0}'", IdEmployee);
                    data = SAP_DBConnection_.GetDataReader(SqlStatement);
                    
                    while (data.Read())
                    {
                        SAP_BussinessPartner SAP_BussinessPartner_ = new SAP_BussinessPartner();
                        SAP_BussinessPartner_.CardCode = data.IsDBNull(0) ? "" : data.GetString(0) + "";
                        SAP_BussinessPartner_.CardName = data.IsDBNull(1) ? "" : data.GetString(1) + "";
                        SAP_BussinessPartner_.SlpName = data.IsDBNull(2) ? "" : data.GetString(2) + "";
                        SAP_BussinessPartner_.IsActive = data.IsDBNull(3) ? false : data.GetString(3) == "Si" ? true : false;
                        SAP_BussinessPartner_.IsActiveEcomerce = data.IsDBNull(4) ? false : data.GetString(4) == "Si" ? true : false;
                        List.Add(SAP_BussinessPartner_);
                    }
                    data.Close();
                }
                return List;
            }
            catch (SAP_Excepcion ex)
            {
                throw ex;
            }
            finally
            {
                if (data != null)
                    data.Close();
            }
        }
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
        public bool Get(string CardCode_)
        {
            DataTable data = null;
            bool result = false;
            string sqlStatement = string.Format("EXEC Eco_GetInfoGeneralCustomer @CardCode = '{0}'", CardCode_);
            try
            {
                SAP_Tools.ValidSQLConnection(SAP_DBConnection_);
                data = SAP_DBConnection_.GetData(sqlStatement);
                if (data.Rows.Count == 1)
                {
                    this.CardCode = data.Rows[0].ItemArray[0].ToString();
                    this.CardName = data.Rows[0].ItemArray[1].ToString();
                    this.ExtraDays = data.Rows[0].ItemArray[2].ToString();
                    this.DescriptPayment = data.Rows[0].ItemArray[3].ToString();
                    this.CreditLine = double.Parse(data.Rows[0].ItemArray[4].ToString());
                    this.Balance = double.Parse(data.Rows[0].ItemArray[5].ToString());
                    this.Phone2 = data.Rows[0].ItemArray[6].ToString();
                    this.E_Mail = data.Rows[0].ItemArray[7].ToString();
                    this.E_MailL_invoice = data.Rows[0].ItemArray[8].ToString();
                    this.E_MailL_account = data.Rows[0].ItemArray[9].ToString();
                    this.SlpName = data.Rows[0].ItemArray[10].ToString();
                    this.Email_employeSales = data.Rows[0].ItemArray[11].ToString();
                    this.Section = data.Rows[0].ItemArray[12].ToString();
                    this.MonexUSD = data.Rows[0].ItemArray[13].ToString();
                    this.MonexMXP = data.Rows[0].ItemArray[14].ToString();
                    this.Currency = data.Rows[0].ItemArray[15].ToString();
                    result = true;
                }
                return result;
            }
            catch (SAP_Excepcion ex)
            {
                throw ex;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
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
