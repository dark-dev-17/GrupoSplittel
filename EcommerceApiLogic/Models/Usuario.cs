using DbManagerDark.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceApiLogic.Models
{
    [DarkTable(Name = "login_cliente", IsMappedByLabels = true, IsStoreProcedure = false, IsView = false)]
    public class Usuario
    {
        [DarkColumn(Name = "id_cliente", IsMapped = true, IsKey = true)]
        public int IdCliente { get; set; }

        [DarkColumn(Name = "nombre", IsMapped = true, IsKey = false)]
        public string Nombre { get; set; }

        [DarkColumn(Name = "apellidos", IsMapped = true, IsKey = false)]
        public string Apellidos { get; set; }

        [DarkColumn(Name = "telefono", IsMapped = true, IsKey = false)]
        public string Telefono { get; set; }

        [DarkColumn(Name = "email", IsMapped = true, IsKey = false)]
        public string Email { get; set; }

        [DarkColumn(Name = "password", IsMapped = true, IsKey = false)]
        public string Password { get; set; }
    }
}
