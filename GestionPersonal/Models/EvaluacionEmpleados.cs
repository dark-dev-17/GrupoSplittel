using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionPersonal.Models
{
    public class EvaluacionEmple
    {
        [Required]
        public int IdEvaluacion { get; set; }
        [Required]
        public List<int> Empleados { get; set; }
    }
}
