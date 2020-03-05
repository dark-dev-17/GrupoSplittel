using System;

namespace SAPDataProcess
{
    public class SAP_Tools 
    {
        public static string ReverseCadena(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        public static void ValidSQLConnection(SAP_DBConnection SAP_DBConnection_)
        {
            if (SAP_DBConnection_ == null)
            {
                throw new SAP_Excepcion("Is missing the object 'SAP_DBConnection_', please open the connection");
            }
        }
        public static void ValidSAPDI_API(SAP_DI_API SAP_DI_API_)
        {
            if (SAP_DI_API_ == null)
            {
                throw new SAP_Excepcion("Is missing the object 'SAP_DI_API_', please open the connection to SAP DI API");
            }
        }
        public static void ValidTypeAddress(string TypeAddress)
        {
            if (TypeAddress != "S" && TypeAddress != "B")
            {
                throw new SAP_Excepcion(string.Format("the selected value['{0}'] is not valid, you can only select [S - ShipTo] and [B - BillTo]", TypeAddress));
            }
        }
        public static void ValidTypeAddressLong(string TypeAddress)
        {
            if (TypeAddress != "ShipTo" && TypeAddress != "BillTo")
            {
                throw new SAP_Excepcion(string.Format("the selected value['{0}'] is not valid, you can only select [ShipTo] and [BillTo]", TypeAddress));
            }
        }
        public static void ValidStringParameter(string Parameter, string ParameterName)
        {
            if (string.IsNullOrWhiteSpace(Parameter) || string.IsNullOrEmpty(Parameter))
            {
                throw new SAP_Excepcion(string.Format("please enter the '{0}'", ParameterName));
            }
        }
        public static string ConvertTypeAddress(string TypeAddress)
        {
            return (TypeAddress == "ShipTo") ? "S" : "B";
        }
    }
}
