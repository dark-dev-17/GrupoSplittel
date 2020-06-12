using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SAPDataProcess
{
    public class SAP_VendorGroup
    {
        #region Propiedades
        public int GroupCode { set; get; }
        public string GroupName { set; get; }
        
        private SAP_DBConnection SAP_DBConnection_;
        //private SAP_DI_API SAP_DI_API_;
        #endregion

        #region Constructores
        ~SAP_VendorGroup()
        {
            SAP_DBConnection_ = null;
            //SAP_DI_API_ = null;
        }
        public SAP_VendorGroup()
        {

        }
        public SAP_VendorGroup(SAP_DBConnection SAP_DBConnection_)
        {
            this.SAP_DBConnection_ = SAP_DBConnection_;
        }
        //public SAP_VendorGroup(SAP_DI_API SAP_DI_API_)
        //{
        //    this.SAP_DI_API_ = SAP_DI_API_;
        //}
        //public SAP_VendorGroup(SAP_DBConnection SAP_DBConnection_,SAP_DI_API SAP_DI_API_)
        //{
        //    this.SAP_DBConnection_ = SAP_DBConnection_;
        //    this.SAP_DI_API_ = SAP_DI_API_;
        //}
        #endregion

        #region Metodos
        public bool Get(int GroupCode_)
        {
            string SqlStatement = string.Format("SELECT T0.GroupCode,T0.GroupName FROM OCRG T0 ORDER BY T0.GroupName where T0.GroupCode = '{0}'", GroupCode_);
            List<SAP_VendorGroup> List = DataReader(SqlStatement);
            if (List.Count == 1)
            {
                List.ForEach(item => {
                    GroupCode = item.GroupCode;
                    GroupName = item.GroupName;
                });
                return true;
            }
            else
            {

                return false;
            }
        }
        public List<SAP_VendorGroup> Get()
        {
            try
            {
                string SqlStatement = string.Format("SELECT T0.GroupCode,T0.GroupName FROM OCRG T0 ORDER BY T0.GroupName");
                return DataReader(SqlStatement);
            }
            catch (SAP_Excepcion ex)
            {
                throw ex;
            }
        }
        private List<SAP_VendorGroup> DataReader(string SqlStatement)
        {
            List<SAP_VendorGroup> List = null;
            SqlDataReader data = null;

            try
            {
                SAP_Tools.ValidSQLConnection(SAP_DBConnection_);
                List = new List<SAP_VendorGroup>();
                
                data = SAP_DBConnection_.GetDataReader(SqlStatement);

                while (data.Read())
                {
                    SAP_VendorGroup SAP_VendorGroup_ = new SAP_VendorGroup();
                    SAP_VendorGroup_.GroupCode = data.IsDBNull(0) ? 0 : data.GetInt16(0);
                    SAP_VendorGroup_.GroupName = data.IsDBNull(1) ? "" : data.GetString(1) + "";
                    List.Add(SAP_VendorGroup_);
                }
                data.Close();
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
        
        #endregion
    }
}
