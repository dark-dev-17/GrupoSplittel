using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionPersonal.Models
{
    public class SalaReservaciones
    {
        public int Id { get; set; }
        public string Title  { get;set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
