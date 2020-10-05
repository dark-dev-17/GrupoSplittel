using System;
using System.Collections.Generic;
using System.Text;

namespace FibremexConfiArt.V1
{
    public class Elemento
    {
        public int IdElemento { get; set; }
        public string IdElemClave { get { return "elem_" + IdElemento; } }
        public string Descripcion { get; set; }
        public TipoElemento TipoElemento { get; set; }
        public List<Valores> Valores { get; set; }
        public int Posicion { get; set; }
    }

    public class Evento
    {
        public int IdEvento { get; set; }
        public List<string> Detonantes { get; set; }
        public string AplicaElemento { get; set; }

    }
}
