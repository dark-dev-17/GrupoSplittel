using System;

namespace EcomDataProccess
{
    public class Ecom_Exception : Exception
    {
        public Ecom_Exception()
        {

        }

        public Ecom_Exception(string mensaje)
            : base(mensaje)
        {

        }
    }
}
