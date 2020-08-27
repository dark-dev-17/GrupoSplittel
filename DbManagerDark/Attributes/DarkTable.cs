using System;
using System.Collections.Generic;
using System.Text;

namespace DbManagerDark.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class DarkTable : Attribute
    {
        public string Name { get; set; }
        public bool IsMappedByLabels { get; set; }
        public bool IsStoreProcedure { get; set; }
        public bool IsView { get; set; }
    }
}
