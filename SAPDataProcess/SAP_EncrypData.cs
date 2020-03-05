using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SAPDataProcess
{
    public class SAP_EncrypData
    {
        #region Propiedades
        private string Key { get; set; }
        private string Salt { get; set; }
        #endregion

        #region Constructor
        public SAP_EncrypData(string codigoBase)
        {
            this.Key = CreateKey(codigoBase);
            this.Salt = CreateSalt(codigoBase);
        }
        #endregion

        #region Metodos
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreateKey(string codigoBase)
        {
            string result = "";
            result = string.Format("{0}56{1}A9HHh", codigoBase, SAP_Tools.ReverseCadena(codigoBase));

            return result;
        }
        public string CreateSalt(string codigoBase)
        {
            string result = "";
            result = string.Format("{0}12{1}4576sdv", codigoBase, SAP_Tools.ReverseCadena(codigoBase));

            return result;
        }
        public string Encrypt(string value)
        {
            DeriveBytes rgb = new Rfc2898DeriveBytes(Key, Encoding.Unicode.GetBytes(Salt));
            SymmetricAlgorithm algorithm = new TripleDESCryptoServiceProvider();
            byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            byte[] rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);
            ICryptoTransform transform = algorithm.CreateEncryptor(rgbKey, rgbIV);
            using (MemoryStream buffer = new MemoryStream())
            {
                using (CryptoStream stream = new CryptoStream(buffer, transform, CryptoStreamMode.Write))
                {
                    using (StreamWriter writer = new StreamWriter(stream, Encoding.Unicode))
                    {
                        writer.Write(value);
                    }
                }
                return Convert.ToBase64String(buffer.ToArray());
            }
        }

        public string Decrypt(string value)
        {
            DeriveBytes rgb = new Rfc2898DeriveBytes(Key, Encoding.Unicode.GetBytes(Salt));
            SymmetricAlgorithm algorithm = new TripleDESCryptoServiceProvider();
            byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            byte[] rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);
            ICryptoTransform transform = algorithm.CreateDecryptor(rgbKey, rgbIV);
            using (MemoryStream buffer = new MemoryStream(Convert.FromBase64String(value)))
            {
                using (CryptoStream stream = new CryptoStream(buffer, transform, CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.Unicode))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
        #endregion
    }
}
