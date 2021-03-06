﻿using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(Name = "Sociedad", IsMappedByLabels = false, IsStoreProcedure = true)]
    public class Sociedad
    {
        [Display(Name = "#")]
        [ColumnDB(Name = "IdSociedad", IsMapped = true, IsKey = true)]
        public int IdSociedad { get; set; }

        [Display(Name = "Descripción")]
        [Required]
        [ColumnDB(Name = "Descripcion", IsMapped = true, IsKey = false)]
        public string Descripcion { get; set; }

        [Display(Name = "Dirección")]
        [Required]
        [ColumnDB(Name = "Direccion", IsMapped = true, IsKey = false)]
        public string Direccion { get; set; }
    }
}
