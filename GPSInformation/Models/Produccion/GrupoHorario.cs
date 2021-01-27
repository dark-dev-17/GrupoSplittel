using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSInformation.Models.Produccion
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class GrupoHorario
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdGrupoHorario { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdGrupo { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int Dia { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public TimeSpan Entrada { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public TimeSpan Salida { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public bool Descanso { get; set; } 
        [ColumnDB(IsMapped = true, IsKey = false)]
        public bool EsNoche { get; set; } 
        [ColumnDB(IsMapped = true, IsKey = false)]
        public bool EsCruce { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public double Horas { get; set; }



        [ColumnDB(IsMapped = false, IsKey = false)]
        public string TipoDia { get { return GetTipoDia(); } }

        private string GetTipoDia()
        {
            string Tipo = "";
            if (EsCruce)
            {
                Tipo = "Cambio de turno";
            }
            else
            {
                if (Descanso)
                {
                    Tipo = "Descanso";
                }
                else
                {
                    if (EsNoche)
                    {
                        Tipo = "Noche";
                    }
                    else
                    {
                        Tipo = "Dia";
                    }
                }
            }
            return Tipo;
        }
    }
}
