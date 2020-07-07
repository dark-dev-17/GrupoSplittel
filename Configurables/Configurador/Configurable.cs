using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Configurables.Configurador
{
    public class ConfigurableConf
    {
        public string Configurable { get; set; }
        public string ItemCodeexample { get; set; }
        public string ItemCode { get; set; }
        public string Expresion { get { return GetExpressionRegular(); } }
        public List<ElementCode> Blocks { get; set; }
        public List<RestriccionElemento> Rectrictions { get; set; }
        public List<RestriccionCampoUsuario> FieldsFree { get; set; }

        public string GetExpressionRegular()
        {
            string Expression = "";
            int index = 0;
            Blocks.ForEach(Elemento => {
                string Valores = "";
                Elemento.Options.ForEach(val => {
                    Valores += val.Key + "|";
                });
                if (Elemento.IsFixed)
                {
                    Expression += "(?<" + Elemento.Key + ">" + Elemento.FixedValue + "){0,1}";
                    //Expression += "(?<" + Elemento.Key + ">" + Valores.Substring(0, Valores.Length-1) + "){0,1}";
                }
                if (Elemento.IsOpenUser == false && Elemento.IsFixed == false)
                {
                    Expression += "(?<" + Elemento.Key + ">" + Valores.Substring(0, Valores.Length - 1) + "){0,1}";
                }

                if (Elemento.IsOpenUser && Elemento.IsFixed == false)
                {
                    RestriccionCampoUsuario restriccionCampoUsuario = FieldsFree.Find(free => free.BlockKey == Elemento.Key);
                    if(restriccionCampoUsuario != null)
                    {
                        Expression += "(?<" + Elemento.Key + ">[0-9]{"+ restriccionCampoUsuario.NumberCeros+ ","+ restriccionCampoUsuario.NumberCeros + "}){0,1}";
                    }
                    else
                    {

                    }
                    
                }
            });
            return string.Format("^{0}$", Expression);
        }
    }
}
