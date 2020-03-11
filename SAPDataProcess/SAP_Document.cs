using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SAPDataProcess
{
    public class SAP_Document
    {
        #region Propiedades
        public string DocEntry { get; set; }
        public string DocNum { get; set; }
        public DateTime DocDate { get; set; }
        public double DocTotal { get; set; }
        public string DocType { get; set; }
        public string CardCode { get; set; }
        public string DocCur { get; set; }
        public string TrackNo { get; set; }
        public string Cardname { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string DocNumEcommerce { get; set; }
        private SAP_DBConnection SAP_DBConnection_;
        private SAP_DI_API SAP_DI_API_;
        #endregion

        #region Constructores
        ~SAP_Document()
        {

        }
        public SAP_Document()
        {

        }
        public SAP_Document(SAP_DBConnection SAP_DBConnection_)
        {
            this.SAP_DBConnection_ = SAP_DBConnection_;
        }
        public SAP_Document(SAP_DI_API SAP_DI_API_)
        {
            this.SAP_DI_API_ = SAP_DI_API_;
        }
        public SAP_Document(SAP_DBConnection SAP_DBConnection_, SAP_DI_API SAP_DI_API_)
        {
            this.SAP_DBConnection_ = SAP_DBConnection_;
            this.SAP_DI_API_ = SAP_DI_API_;
        }
        #endregion

        #region Metodos
        public List<SAP_Document> GetRejected(string CardCode_)
        {
            string SqlStatement = string.Format("exec Eco_GetOrdersRejected @CardCode = '{0}', @DocDate = '2020-01-01'", CardCode_);
            List<SAP_Document> List = null;
            SqlDataReader Data = null;
            try
            {
                SAP_Tools.ValidStringParameter(CardCode_, "CardCode");
                SAP_Tools.ValidSQLConnection(SAP_DBConnection_); ;
                Data = SAP_DBConnection_.GetDataReader(SqlStatement);
                List = new List<SAP_Document>();
                while (Data.Read())
                {
                    List.Add(new SAP_Document
                    {
                        DocNum = Data.GetInt32(1) + "",
                        DocDate = DateTime.Parse(Data.GetString(2) + ""),
                        Status = Data.GetString(3),
                        DocType = Data.GetString(0),
                        Remarks = Data.IsDBNull(4) ? "" : Data.GetString(4),
                        DocNumEcommerce = Data.IsDBNull(5) ? "" : Data.GetInt32(5) + "",
                        CardCode = Data.IsDBNull(6) ? "" : Data.GetInt32(6) + "",
                        Cardname = Data.IsDBNull(7) ? "" : Data.GetInt32(7) + "",
                    });
                }
                return List;
            }
            catch (SAP_Excepcion ex)
            {
                throw ex;
            }
            finally
            {
                if (Data != null)
                    Data.Close();
            }
        }
        public List<SAP_Document> GetRejected()
        {
            string SqlStatement = string.Format("exec Eco_GetOrdersRejectedAll @DocDate = '2020-01-01'");
            List<SAP_Document> List = null;
            SqlDataReader Data = null;
            try
            {
                SAP_Tools.ValidSQLConnection(SAP_DBConnection_);
                Data = SAP_DBConnection_.GetDataReader(SqlStatement);
                List = new List<SAP_Document>();
                while (Data.Read())
                {
                    List.Add(new SAP_Document
                    {
                        DocNum = Data.GetInt32(1) + "",
                        DocDate = DateTime.Parse(Data.GetString(2) + ""),
                        Status = Data.GetString(3),
                        DocType = Data.GetString(0),
                        Remarks = Data.IsDBNull(4) ? "" : Data.GetString(4),
                        DocNumEcommerce = Data.IsDBNull(5) ? "" : Data.GetInt32(5) + "",
                        CardCode = Data.IsDBNull(6) ? "" : Data.GetInt32(6) + "",
                        Cardname = Data.IsDBNull(7) ? "" : Data.GetInt32(7) + "",
                    });
                }
                return List;
            }
            catch (SAP_Excepcion ex)
            {
                throw ex;
            }
            finally
            {
                if (Data != null)
                    Data.Close();
            }
        }
        public List<SAP_Document> GetHistoric(string CardCode_)
        {
            string SqlStatement = string.Format("exec [Eco_GetOrdersByCustomer] @CardCode = '{0}'", CardCode_);
            List<SAP_Document> List = null;
            SqlDataReader Data = null;
            try
            {
                SAP_Tools.ValidStringParameter(CardCode_, "CardCode");
                SAP_Tools.ValidSQLConnection(SAP_DBConnection_); ;
                Data = SAP_DBConnection_.GetDataReader(SqlStatement);
                List = new List<SAP_Document>();
                while (Data.Read())
                {
                    List.Add(new SAP_Document
                    {
                        DocNum = Data.GetInt32(2) + "",
                        DocEntry = Data.GetInt32(3) + "",
                        DocDate = Data.GetDateTime(4),
                        CardCode = Data.GetString(0),
                        Cardname = Data.GetString(1),
                        DocType = Data.GetString(5),
                        DocCur = Data.GetString(6),
                        DocTotal = double.Parse(Data.GetDecimal(7) + ""),
                        DocNumEcommerce = Data.IsDBNull(8) ? "" : Data.GetInt32(8) + ""
                    });
                }
                return List;
            }
            catch (SAP_Excepcion ex)
            {
                throw ex;
            }
            finally
            {
                if (Data != null)
                    Data.Close();
            }
        }
        public List<SAP_Document> GetHistoric()
        {
            string SqlStatement = string.Format("exec [Eco_GetOrdersAll] @CardCode = 'get'");
            List<SAP_Document> List = null;
            SqlDataReader Data = null;
            try
            {
                SAP_Tools.ValidSQLConnection(SAP_DBConnection_);
                Data = SAP_DBConnection_.GetDataReader(SqlStatement);
                List = new List<SAP_Document>();
                while (Data.Read())
                {
                    List.Add(new SAP_Document
                    {
                        DocNum = Data.GetInt32(2) + "",
                        DocEntry = Data.GetInt32(3) + "",
                        DocDate = Data.GetDateTime(4),
                        CardCode = Data.GetString(0),
                        Cardname = Data.GetString(1),
                        DocType = Data.GetString(5),
                        DocCur = Data.GetString(6),
                        DocTotal = double.Parse(Data.GetDecimal(7) + ""),
                        DocNumEcommerce = Data.IsDBNull(8) ? "" : Data.GetInt32(8) + ""
                    });
                }
                return List;
            }
            catch (SAP_Excepcion ex)
            {
                throw ex;
            }
            finally
            {
                if (Data != null)
                    Data.Close();
            }
        }
        public List<SAP_Document> GetInProcess(string CardCode_)
        {
            string SqlStatement = string.Format("exec [Eco_GetOrderInProcessEcommerce] @CardCode = '{0}'", CardCode_ );
            List<SAP_Document> List = null;
            SqlDataReader Data = null;
            try
            {
                SAP_Tools.ValidStringParameter(CardCode_, "CardCode");
                SAP_Tools.ValidSQLConnection(SAP_DBConnection_); ;
                Data = SAP_DBConnection_.GetDataReader(SqlStatement);
                List = new List<SAP_Document>();
                while (Data.Read())
                {
                    List.Add(new SAP_Document
                    {
                        DocNum = Data.GetInt32(1) + "",
                        DocEntry = Data.GetInt32(2) + "",
                        DocDate = Data.GetDateTime(3),
                        CardCode = Data.GetString(0),
                        DocType = Data.GetString(5),
                        DocTotal = double.Parse(Data.GetDecimal(6) + ""),
                        DocNumEcommerce = Data.IsDBNull(4) ? "" : Data.GetInt32(4) + "",
                        DocCur = Data.GetString(7),
                        Status = Data.GetInt32(9) + "",
                        TrackNo = Data.IsDBNull(8) ? "" : Data.GetString(8) + "",
                        //Cardname = Data.IsDBNull(10) ? "" : Data.GetString(10) + ""
                    });
                }
                return List;
            }
            catch (SAP_Excepcion ex)
            {
                throw ex;
            }
            finally
            {
                if (Data != null)
                    Data.Close();
            }
        }
        public List<SAP_Document> GetInProcess()
        {
            string SqlStatement = string.Format("exec [Eco_GetOrderInProcessEcommerceAll] @CardCode = 'get'");
            List<SAP_Document> List = null;
            SqlDataReader Data = null;
            try
            {
                SAP_Tools.ValidSQLConnection(SAP_DBConnection_); 
                Data = SAP_DBConnection_.GetDataReader(SqlStatement);
                List = new List<SAP_Document>();
                while (Data.Read())
                {
                    List.Add(new SAP_Document
                    {
                        DocNum = Data.GetInt32(1) + "",
                        DocEntry = Data.GetInt32(2) + "",
                        DocDate = Data.GetDateTime(3),
                        CardCode = Data.GetString(0),
                        DocType = Data.GetString(5),
                        DocTotal = double.Parse(Data.GetDecimal(6) + ""),
                        DocNumEcommerce = Data.IsDBNull(4) ? "" : Data.GetInt32(4) + "",
                        DocCur = Data.GetString(7),
                        Status = Data.GetInt32(9) + "",
                        TrackNo = Data.IsDBNull(8) ? "" : Data.GetString(8) + "",
                        Cardname = Data.IsDBNull(10) ? "" : Data.GetString(10) + ""
                    });
                }
                return List;
            }
            catch (SAP_Excepcion ex)
            {
                throw ex;
            }
            finally
            {
                if (Data != null)
                    Data.Close();
            }
        }
        public string GetStatusDescription()
        {
            if (Int32.Parse(Status) == 1)
            {
                return "En proceso";
            }
            else if (Int32.Parse(Status) == 2)
            {
                return "Surtiendo";
            }
            else if (Int32.Parse(Status) == 3)
            {
                return "Embarcando";
            }
            else if (Int32.Parse(Status) == 4)
            {
                return "Enviando";
            }
            else if (Int32.Parse(Status) == 5)
            {
                return "Entregando";
            }
            else
            {
                return "Error";
            }
        }
        public bool GetSapEstatus(int DocEcommerce)
        {
            string SqlStatement = string.Format("exec [Eco_GetOrderStatusEcommerce] @DocNumEcommerce = '{0}'", DocEcommerce);
            SqlDataReader Data = null;
            bool Result = false;
            try
            {
                SAP_Tools.ValidSQLConnection(SAP_DBConnection_);
                Data = SAP_DBConnection_.GetDataReader(SqlStatement);
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        DocEntry = Data.GetInt32(1) + "";
                        DocNumEcommerce = DocEcommerce + "";
                        CardCode = Data.IsDBNull(0) ? "" : Data.GetString(0) + ""; ;
                        Status = Data.GetInt32(3) + "";
                        TrackNo = Data.IsDBNull(2) ? "" : Data.GetString(2) + "";
                    }
                    Result = true;
                }
                
                return Result;
            }
            catch (SAP_Excepcion ex)
            {
                throw ex;
            }
            finally
            {
                if (Data != null)
                    Data.Close();
            }
        }
        #endregion
    }
}
