using System;
using System.Collections.Generic;
using System.Text;

namespace DbManagerDark.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class DarkColumn : Attribute
    {
        public string Name { get; set; }
        public bool IsMapped { get; set; }
        public bool IsKey { get; set; }
    }
}
