using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSInformation.Models.Produccion
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class GrupoCambios
    {
        /// <summary>
        /// Id
        /// </summary>
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdGrupoCambios { get; set; }
        
        /// <summary>
        /// Id de la persona
        /// </summary>
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }

        /// <summary>
        /// Id del grupo de produccion
        /// </summary>
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdGrupo { get; set; }

        /// <summary>
        /// Fecha a apicar el cambio
        /// </summary>
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Comentarios del cambio
        /// </summary>
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Comentarios { get; set; }

        /// <summary>
        /// Fecha de creacio
        /// </summary>
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Creado { get; set; }

        /// <summary>
        /// fecha de modificacion
        /// </summary>
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Modificado { get; set; }

        /// <summary>
        /// Nombre del grupo actual
        /// </summary>
        [ColumnDB(IsMapped = false, IsKey = false)]
        public string GrupoName { get; set; }

        [ColumnDB(IsMapped = false, IsKey = false)]
        public DateTime FechaInicio { get; set; }
    }
}
