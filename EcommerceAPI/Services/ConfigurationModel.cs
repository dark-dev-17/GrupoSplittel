using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.Services
{
    public class ConfigurationModel
    {
        public string ItemCode { get; set; }
        public bool Isvalid { get; set; }
        public List<GroupView> Optionss { get; set; }
    }
    public class GroupView
    {
        public string Name { get; set; }
        public string ValueSelected { get; set; }
        public bool IsSelect { get; set; }
        public List<Values> Values { get; set; }
        public string TipeGroup { get; set; }
        public string Mask { get; set; }
    }
    public class Values
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
