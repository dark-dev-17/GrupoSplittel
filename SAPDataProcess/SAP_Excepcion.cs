using System;

namespace SAPDataProcess
{
    public class SAP_Excepcion : Exception
    {
        public SAP_Excepcion()
        {

        }

        public SAP_Excepcion(string mensaje)
            : base(mensaje)
        {

        }

    }
}
