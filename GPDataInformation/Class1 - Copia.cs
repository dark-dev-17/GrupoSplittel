using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GPDataInformation
{
    public class Class1
    {
        //public static void ValidDBobject(Ecom_DBConnection Ecom_DBConnection_)
        //{
        //    if (Ecom_DBConnection_ == null)
        //    {
        //        throw new Ecom_Exception("Sin referencia a base de datos");
        //    }
        //}
        //public static void ValidStringParameter(string Parameter, string ParameterName)
        //{
        //    if (string.IsNullOrWhiteSpace(Parameter) || string.IsNullOrEmpty(Parameter))
        //    {
        //        throw new Ecom_Exception(string.Format("please enter the '{0}'", ParameterName));
        //    }
        //}
        //public static void ValidIntParameter(int Parameter, string ParameterName)
        //{
        //    if (Parameter == 0)
        //    {
        //        throw new Ecom_Exception(string.Format("please enter the '{0}'", ParameterName));
        //    }
        //}
        //public static List<string> ProcessEmailList(string adress)
        //{
        //    List<string> lista = new List<string>();
        //    string[] allAddresses = adress.Split(";,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        //    foreach (string emailAddress in allAddresses)
        //    {
        //        lista.Add(emailAddress.Replace("'", "").Replace("\"", ""));
        //    }
        //    return lista;
        //}
        public static string ConvevrtListString(List<string> Emails)
        {
            string Cadena = "";
            if (Emails != null)
            {
                if (Emails.Count != 0)
                {
                    foreach (string EmailDir in Emails)
                    {
                        Cadena += EmailDir + ";";
                    }
                }
            }

            return Cadena;
        }
        public static bool IsValidEmail(string email)
        {
            try
            {
                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(strRegex);
                if (!re.IsMatch(email.Trim()))
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
