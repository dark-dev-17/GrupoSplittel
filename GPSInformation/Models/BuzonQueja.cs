using GPSInformation.Attributes;
using GPSInformation.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class BuzonQueja
    {
        [Display(Name = "Folio")]
        [DisplayFormat(DataFormatString = "SQ{0:0000}", ApplyFormatInEditMode = false)]
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdBuzonQueja { get; set; }

        [Required]
        [Display(Name = "Comentario")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Comentario { get; set; }

        [Display(Name = "Creado")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Creacion { get; set; }

        [Display(Name = "Fuente")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string SourceCliente { get; set; }
        
    }
}
