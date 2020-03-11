using System;

namespace EcomDataProccess
{
    public class Ecom_Tools
    {
        public static void ValidDBobject(Ecom_DBConnection Ecom_DBConnection_)
        {
            if(Ecom_DBConnection_ == null)
            {
                throw new Ecom_Exception("Sin referencia a base de datos");
            }
        }
    }
}
