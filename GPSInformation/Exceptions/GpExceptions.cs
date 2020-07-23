using System;
using System.Collections.Generic;
using System.Text;

namespace GPSInformation.Exceptions
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
