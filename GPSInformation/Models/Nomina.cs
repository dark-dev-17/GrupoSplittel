using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class Nomina
    {
        [Display(Name = "#")]
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdNomina { get; set; }

        [Display(Name = "RFC")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string RFC { get; set; }

        [Display(Name = "Fe.Timbrado")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime FechaTimbrado { get; set; }

        [Display(Name = "Fe.Emisión")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime FechaEmision { get; set; }

        [Display(Name = "Folio")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Folio { get; set; }

        [Display(Name = "Fe.Inicio Pago")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime FechaInicialPago { get; set; }

        [Display(Name = "Fe.Fin Pago")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime FechaFinalPago { get; set; }

        [Display(Name = "Fe.Pago")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime FechaPago { get; set; }

        [Display(Name = "No.Nomina")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int NumeroNomina { get; set; }

        [Display(Name = "Aceptado")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public bool AceptadoEmpleado { get; set; }

        [Display(Name = "Comentarios")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Comentarios { get; set; }

        [Display(Name = "Archivo")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string NombreArchivo { get; set; }

        [Display(Name = "Monto($)")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public double TotalNeto { get; set; }
    }
}
