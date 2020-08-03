using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPSInformation.Models;

namespace GestionPersonal.Models
{
    public class Incidencias
    {
        public Persona persona { set; get; }
        public List<IncidenciaPermiso> permisos { set; get; }
    }
}
