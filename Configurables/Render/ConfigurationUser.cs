using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Configurables.Render
{
    public class ConfigurationUserRe
    {
        public ConfigurationUser configurationUser { get; set; }
        public string Nombre { get; set; }
    }
    public class ConfigurationUser
    {
        public string Configurable { get; set; }
        public string ItemCodeexample { get; set; }
        public string ItemCode { get; set; }
        public List<BloquesForm> Blocks { get; set; }
        public List<DescriptionElement> Description { get; set; }
    }
    public class BloquesForm
    {
        public string BlockName { get; set; }
        public string BlockKey { get; set; }
        public string KeySelected { get; set; }
        public string KeySelectedUser { get; set; }
        public bool IsOpenUser { get; set; }
        public bool IsModificating { get; set; }
        public List<BloqueFormOptions> FormOption { get; set; }
    }
    public class BloqueFormOptions
    {
        public string Option { get; set; }
        public string Key { get; set; }
        public bool Active { get; set; }
    }
    public class DescriptionElement
    {
        public string Bloque { get; set; }
        public string Selected { get; set; }
    }
}
