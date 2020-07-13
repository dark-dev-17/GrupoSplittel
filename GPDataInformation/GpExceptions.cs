using System;

namespace GPDataInformation
{
    public class GpExceptions : Exception
    {
        public GpExceptions()
        {

        }

        public GpExceptions(string mensaje)
            : base(mensaje)
        {

        }
    }
}
