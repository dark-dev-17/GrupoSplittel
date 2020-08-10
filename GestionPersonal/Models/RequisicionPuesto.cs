using GPSInformation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionPersonal.Models
{
    public class RequisicionPuesto
    {
        public RequisicionPersonal RequisicionPersonal { get; set; }
        public Puesto Puesto { get; set; }
    }
}
