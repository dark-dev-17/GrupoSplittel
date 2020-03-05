using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SAPDataProcess
{
    public class SAP_Address
    {
        #region Propiedades
        public string Street { get; set; }
        public string StreetNo { get; set; }
        public string Block { get; set; }
        public string County { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string FederalTaxID { get; set; }
        public string CardName { get; set; }
        public bool Default { get; set; }
        public string AddressType { get; set; }
        public string AddressName { get; set; }
        public SAP_ContactPerson ContactPerson { get; set; }
        private SAP_DBConnection SAP_DBConnection_;
        private SAP_DI_API SAP_DI_API_;
        private string Message;
        #endregion

        #region Constructores
        ~SAP_Address()
        {
            SAP_DBConnection_ = null;
            SAP_DI_API_ = null;
        }
        public SAP_Address()
        {

        }
        public SAP_Address(SAP_DBConnection SAP_DBConnection_)
        {
            this.SAP_DBConnection_ = SAP_DBConnection_;
        }
        public SAP_Address(SAP_DI_API SAP_DI_API_)
        {
            this.SAP_DI_API_ = SAP_DI_API_;
        }
        public SAP_Address(SAP_DBConnection SAP_DBConnection_, SAP_DI_API SAP_DI_API_)
        {
            this.SAP_DBConnection_ = SAP_DBConnection_;
            this.SAP_DI_API_ = SAP_DI_API_;
        }
        #endregion

        #region Metodos
        public bool Create(string CardCode)
        {
            bool Result = false;
            try
            {
                SAP_Tools.ValidTypeAddressLong(AddressType);
                SAP_Tools.ValidSAPDI_API(SAP_DI_API_);

                SAPbobsCOM.BusinessPartners oCustomer = new SAP_BussinessPartner(SAP_DI_API_).GetBusinessPartner(CardCode);
                SAPbobsCOM.BPAddresses ListAddress = oCustomer.Addresses;

                BoAddressType boAddressType = AddressType == "BillTo" ? BoAddressType.bo_BillTo : BoAddressType.bo_ShipTo;
                int NoAddresses = 0;

                for (int i = 0; i < ListAddress.Count; i++)
                {
                    ListAddress.SetCurrentLine(i);
                    NoAddresses = i;
                    if (ListAddress.AddressName == AddressName.Trim() && boAddressType == ListAddress.AddressType)
                    {
                        throw new SAP_Excepcion(string.Format("The address '{0}' already exists", AddressName.Trim()));
                    }
                }
                ListAddress.Add();
                ListAddress.SetCurrentLine(NoAddresses + 1);
                ListAddress.Street = Street;
                ListAddress.StreetNo = StreetNo;
                ListAddress.Block = Block;
                ListAddress.County = County;
                ListAddress.ZipCode = ZipCode;
                ListAddress.State = State;
                ListAddress.City = City;
                ListAddress.AddressName = AddressName;
                ListAddress.AddressType = boAddressType;

                if (oCustomer.Update() == 0)
                {
                    Result = true;
                }
                else
                {
                    Message = SAP_DI_API_.GetErrorMessage();
                    Result = false;
                }
                return Result;
            }
            catch (SAP_Excepcion ex)
            {
                throw ex;
            }
        }
        public bool Detele(string CardCode)
        {
            bool Result = false;
            try
            {
                SAP_Tools.ValidTypeAddressLong(AddressType);
                SAP_Tools.ValidSAPDI_API(SAP_DI_API_);

                SAPbobsCOM.BusinessPartners oCustomer = new SAP_BussinessPartner(SAP_DI_API_).GetBusinessPartner(CardCode);
                SAPbobsCOM.BPAddresses ListAddress = oCustomer.Addresses;

                BoAddressType boAddressType = AddressType == "BillTo" ? BoAddressType.bo_BillTo : BoAddressType.bo_ShipTo;
                for (int i = 0; i < ListAddress.Count; i++)
                {
                    ListAddress.SetCurrentLine(i);
                    if (ListAddress.AddressName == AddressName.Trim() && boAddressType == ListAddress.AddressType)
                    {
                        Result = true;
                        ListAddress.Delete();
                        break;
                    }
                }

                if (!Result)
                {
                    throw new SAP_Excepcion(string.Format("The address '{0}' doesn't exist", AddressName.Trim()));
                }
                if (oCustomer.Update() == 0)
                {
                    Result = true;
                }
                else
                {
                    Message = SAP_DI_API_.GetErrorMessage();
                    Result = false;
                }
                return Result;
            }
            catch (SAP_Excepcion ex)
            {
                throw ex;
            }
        }
        public bool Update(string CardCode)
        {
            
            bool Result = false;
            try
            {
                SAP_Tools.ValidTypeAddressLong(AddressType);
                SAP_Tools.ValidSAPDI_API(SAP_DI_API_);

                SAPbobsCOM.BusinessPartners oCustomer = new SAP_BussinessPartner(SAP_DI_API_).GetBusinessPartner(CardCode);
                SAPbobsCOM.BPAddresses ListAddress = oCustomer.Addresses;
                BoAddressType boAddressType = AddressType == "BillTo" ? BoAddressType.bo_BillTo : BoAddressType.bo_ShipTo;
                for (int i = 0; i < ListAddress.Count; i++)
                {
                    ListAddress.SetCurrentLine(i);
                    if(ListAddress.AddressName == AddressName.Trim() && boAddressType == ListAddress.AddressType)
                    {
                        Result = true;
                        ListAddress.Street = Street;
                        ListAddress.StreetNo = StreetNo;
                        ListAddress.Block = Block;
                        ListAddress.County = County;
                        ListAddress.ZipCode = ZipCode;
                        ListAddress.State = State;
                        ListAddress.City = City;
                        ListAddress.AddressName = AddressName;
                        break;
                    }
                }
                if(oCustomer.Update() == 0)
                {
                    Result = true;
                }
                else
                {
                    Message = SAP_DI_API_.GetErrorMessage();
                    Result = false;
                }
                return Result;
            }
            catch (SAP_Excepcion ex)
            {
                throw ex;
            }
        }
        public bool GetByAddressName(string CardCode, string TypeAddress,string AddressNam_)
        {
            string SqlStatement = string.Format("exec [Eco_GetAddressByCustomerAddresName] @CardCode = '{0}', @AdresType = '{1}', @AddressName = '{2}'", CardCode, TypeAddress, AddressNam_);
            bool IsExists = false;
            SqlDataReader data = null;
            try
            {
                SAP_Tools.ValidStringParameter(AddressNam_, "AddressName");
                SAP_Tools.ValidStringParameter(CardCode, "CardCode");
                SAP_Tools.ValidTypeAddress(TypeAddress);

                data = SAP_DBConnection_.GetDataReader(SqlStatement);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        AddressName = data.IsDBNull(0) ? "" : data.GetString(0) + "";
                        Street = data.IsDBNull(1) ? "" : data.GetString(1) + "";
                        StreetNo = data.IsDBNull(2) ? "" : data.GetString(2) + "";
                        Block = data.IsDBNull(3) ? "" : data.GetString(3) + "";
                        County = data.IsDBNull(4) ? "" : data.GetString(4) + "";
                        ZipCode = data.IsDBNull(5) ? "" : data.GetString(5) + "";
                        State = data.IsDBNull(6) ? "" : data.GetString(6) + "";
                        FederalTaxID = data.IsDBNull(7) ? "" : data.GetString(7) + "";
                        City = data.IsDBNull(8) ? "" : data.GetString(8) + "";
                        CardName = data.IsDBNull(9) ? "" : data.GetString(9) + "";
                        Default = data.GetString(10) + "" == "default" ? true : false;
                        SAP_ContactPerson SAP_ContactPerson_ = new SAP_ContactPerson();
                        SAP_ContactPerson_.Name = data.IsDBNull(11) ? "" : data.GetString(11) + "";
                        SAP_ContactPerson_.Telphone = data.IsDBNull(12) ? "" : data.GetString(12) + "";
                        SAP_ContactPerson_.Email = data.IsDBNull(13) ? "" : data.GetString(13) + "";
                        ContactPerson = SAP_ContactPerson_;
                    }
                    data.Close();
                    IsExists = true;
                }
                else
                {
                    Message = string.Format("The address '{0}' doesn't exist", AddressNam_);
                }
                return IsExists;
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
        public List<SAP_Address> GetList(string CardCode,string TypeAddress)
        {
            List<SAP_Address> List;
            SqlDataReader data = null;

            string SqlStatement = string.Format("exec Eco_GetAddressByCustomer @CardCode = '{0}', @AdresType = '{1}'", CardCode, TypeAddress);

            SAP_Tools.ValidStringParameter(CardCode, "CardCode");
            SAP_Tools.ValidTypeAddress(TypeAddress);

            try
            {
                data = SAP_DBConnection_.GetDataReader(SqlStatement);
                List = new List<SAP_Address>();
                while (data.Read())
                {
                    SAP_Address SAP_Address_ = new SAP_Address();
                    SAP_Address_.AddressName = data.IsDBNull(0) ? "" : data.GetString(0) + "";
                    SAP_Address_.Street = data.IsDBNull(1) ? "" : data.GetString(1) + "";
                    SAP_Address_.StreetNo = data.IsDBNull(2) ? "" : data.GetString(2) + "";
                    SAP_Address_.Block = data.IsDBNull(3) ? "" : data.GetString(3) + "";
                    SAP_Address_.County = data.IsDBNull(4) ? "" : data.GetString(4) + "";
                    SAP_Address_.ZipCode = data.IsDBNull(5) ? "" : data.GetString(5) + "";
                    SAP_Address_.State = data.IsDBNull(6) ? "" : data.GetString(6) + "";
                    SAP_Address_.FederalTaxID = data.IsDBNull(7) ? "" : data.GetString(7) + "";
                    SAP_Address_.City = data.IsDBNull(8) ? "" : data.GetString(8) + "";
                    SAP_Address_.CardName = data.IsDBNull(9) ? "" : data.GetString(9) + "";
                    SAP_Address_.Default = data.GetString(10) + "" == "default" ? true : false;
                    SAP_ContactPerson SAP_ContactPerson_ = new SAP_ContactPerson();
                    SAP_ContactPerson_.Name = data.IsDBNull(11) ? "" : data.GetString(11) + "";
                    SAP_ContactPerson_.Telphone = data.IsDBNull(12) ? "" : data.GetString(12) + "";
                    SAP_ContactPerson_.Email = data.IsDBNull(13) ? "" : data.GetString(13) + "";
                    SAP_Address_.ContactPerson = SAP_ContactPerson_;
                    List.Add(SAP_Address_);
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
        public string GetMessage()
        {
            return Message;
        }
        public void SetConnectio(SAP_DBConnection SAP_DBConnection_)
        {
            this.SAP_DBConnection_ = SAP_DBConnection_;
        }
        #endregion
    }
}
