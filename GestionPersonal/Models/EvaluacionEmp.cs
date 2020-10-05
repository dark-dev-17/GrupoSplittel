using GPSInformation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionPersonal.Models
{
    public class EvaluacionEmp
    {
        public List<EvaluacioSeccion> list  { get; set; }
        public int IdEvaluacion { get; set; }
    }
}
