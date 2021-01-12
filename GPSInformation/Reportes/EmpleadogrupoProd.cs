using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Reportes
{
    public class EmpleadogrupoProd
    {
        public int IdPersona { get; set; }
        [Display(Name = "Nomina")]
        public string NumeroNomina { get; set; }
        [Display(Name = "Nombre")]
        public string NombreCompleto { get; set; }
        [Display(Name = "Puesto")]
        public string Puesto { get; set; }
        [Display(Name = "Fe.Ingreso")]
        public DateTime Ingreso { get; set; }
        [Display(Name = "TurnoActual")]
        public string Turno { get; set; }
        [Display(Name = "Activo")]
        public bool Active { get; set; }
    }
}
