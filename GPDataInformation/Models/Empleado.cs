using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;

namespace GPDataInformation.Models
{
    public class Empleado 
    {
        public int IdEmpleado { get; set; }
        [Required]
        public int IdPersona { get; set; }
        [Required]
        public int NumeroNomina { get; set; }
        [Required]
        public int TipoNomina { get; set; }
        [Required]
        public int IdSociedad { get; set; }
        [Required]
        public int IdDepartamento { get; set; }
        [Required]
        public int IdPuesto { get; set; }
        [Required]
        public DateTime Ingreso { get; set; }
        [Required]
        public DateTime Egreso { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Extension { get; set; }
        [Required]
        public double Salario { set; get; }
        [Required]
        public int IdEstatus { get; set; }



        public void SetConnection(DBConnection dBConnection)
        {

        }
    }

}
