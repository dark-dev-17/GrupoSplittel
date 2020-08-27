using System;
using System.Collections.Generic;
using System.Text;

namespace DbManagerDark.Exceptions
{
    public class DarkExceptionUser : Exception
    {
        public DarkExceptionUser()
        {

        }

        public DarkExceptionUser(string mensaje)
            : base(mensaje)
        {

        }
    }
}
