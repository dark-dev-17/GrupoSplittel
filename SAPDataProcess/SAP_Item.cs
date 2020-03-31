using System;

namespace SAPDataProcess
{
    public class SAP_Item
    {
        #region Propiedades
        public string CardCode { get; set; }
        public string Password { get; set; }
        public string Society { get; set; }
        private SAP_DBConnection SAP_DBConnection_;
        #endregion
        public SAP_Item()
        {

        }
        public SAP_Item(SAP_DBConnection SAP_DBConnection_)
        {
            this.SAP_DBConnection_ = SAP_DBConnection_;
        }
        #region Constructores

        #endregion

        #region Metodos
        public void ValidCredentials()
        {
            try
            {
                if (Society != "FIBREMEX" && Society != "OPTRONICS")
                {
                    throw new SAP_Excepcion(string.Format("The parameter Society[{0}] is not valid", Society));
                }
                if (!string.IsNullOrWhiteSpace(CardCode) || !string.IsNullOrWhiteSpace(Password))
                {
                    SAP_EncrypData encrypData = new SAP_EncrypData(CardCode);
                    //contrasena que envia el cliente
                    string DecryptClient = encrypData.Decrypt(Password);
                    //contrasena que se extrae de SAP sobre el socio de negocios
                    string DecryptServer = new  SAP_BussinessPartner(SAP_DBConnection_).GetPasswordDB(CardCode);

                    if (!string.IsNullOrWhiteSpace(DecryptServer))
                    {
                        if (DecryptClient.Trim() != encrypData.Decrypt(DecryptServer.Trim()))
                        {
                            throw new SAP_Excepcion(string.Format("The credentials for the bussiness partner[{0} are invalid]", CardCode));
                        }
                    }
                    else
                    {
                        throw new SAP_Excepcion(string.Format("No access to SAP Bussines One", CardCode));
                    }
                }
                else
                {
                    throw new SAP_Excepcion(string.Format("Valid Credentials - Some fields are empty", CardCode));
                }
            }
            catch (SAP_Excepcion ex)
            {
                throw ex;
            }
            catch (System.Security.Cryptography.CryptographicException ex)
            {
                throw new SAP_Excepcion(string.Format("The specified account password is not correct"));
            }
            catch (Exception Ex)
            {
                throw new SAP_Excepcion(string.Format("Exception - {0}", Ex.Message));
            }
            finally
            {
            }
        }
        #endregion

    }
}
