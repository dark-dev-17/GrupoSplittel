using DbManagerDark.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceApiLogic.Models
{
    [DarkTable(Name = "cotizacion_encabezado", IsMappedByLabels = true, IsStoreProcedure = false, IsView = false)]
    public class Pedido
    {
        [DarkColumn(Name = "id",IsMapped = true, IsKey = true)]
        public int IdPedido { get; set; }

        [DarkColumn(Name = "id_cliente", IsMapped = true, IsKey = false)]
        public int IdCliente { get; set; }

        [DarkColumn(Name = "subtotal", IsMapped = true, IsKey = false)]
        public float SubTotal { get; set; }

        [DarkColumn(Name = "iva", IsMapped = true, IsKey = false)]
        public float Iva { get; set; }

        [DarkColumn(Name = "total", IsMapped = true, IsKey = false)]
        public float Total { get; set; }

        [DarkColumn(Name = "activo", IsMapped = true, IsKey = false)]
        public string Activo { get; set; }

        [DarkColumn(Name = "estatus", IsMapped = true, IsKey = false)]
        public string estatus { get; set; }
    }
}
