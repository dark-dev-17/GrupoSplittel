﻿using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSInformation.Models.Produccion
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class GrupoCambios
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdGrupoCambios { get; set; }
        
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdGrupo { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Fecha { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Comentarios { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Creado { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Modificado { get; set; }

        [ColumnDB(IsMapped = false, IsKey = false)]
        public string GrupoName { get; set; }
    }
}
