using System;
using System.Collections.Generic;
using System.Text;

namespace FibremexConfiArt.V1
{
    public class Configurable
    {
        public int IdConfigurable { get; set; }
        public string Nombre { get; set; }
        public string Descripción { get; set; }
        public string ExpresionRegular { get; set; }
        public string CodigoBase { get; set; }
        public List<Elemento> Elementos { get; set; }
    }
    public enum TipoRegla
    {
        Activar = 1,
        DesActivar = 1,
    }
    public enum TipoElemento
    {
        ParteFija = 1,
        ParteOpcional = 2,
        ParteLibre = 3
    }
}
