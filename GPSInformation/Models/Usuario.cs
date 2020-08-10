﻿using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class Usuario
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdUsuario { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        [Required]
        public string UserName { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Pass { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public bool Activo { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime UltimoIngreso { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }

    }
}