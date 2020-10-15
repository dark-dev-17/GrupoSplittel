using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(Name = "InformacionMedica", IsMappedByLabels = false, IsStoreProcedure = false)]
    public class Empleado
    {
        [ColumnDB(Name = "IdPersona", IsMapped = true, IsKey = true)]
        public int IdEmpleado { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Por favor selecciona al empleado")]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }

        [Display(Name = "No.Nomina")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:000000}", ApplyFormatInEditMode = true)]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public int NumeroNomina { get; set; }

        [Display(Name = "Nomina")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor selecciona una opción")]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public int TipoNomina { get; set; }

        [Display(Name = "Sociedad")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor selecciona una opción")]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public int IdSociedad { get; set; }

        [Display(Name = "Departamento")]
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Por favor selecciona una opción")]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public int IdDepartamento { get; set; }

        [Display(Name = "Puesto")]
        [Required]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public int IdPuesto { get; set; }

        [Display(Name = "Fe.Ingreso")]
        [Required]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public DateTime Ingreso { get; set; }

        [Display(Name = "Fe.Baja")]
        [Required]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public DateTime Egreso { get; set; }

        [Display(Name = "Correo")]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public string Email { get; set; }

        [Display(Name = "Ext.")]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public string Extension { get; set; }

        [Display(Name = "Salario")]
        [Required]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public double Salario { set; get; }

        [Display(Name = "Estatus")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor selecciona una opción")]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public int IdEstatus { get; set; }

        [Display(Name = "No.Nomina")]
        [ColumnDB(Name = "Nombre", IsMapped = false, IsKey = false)]
        public string NominaReal { 
            get {
                string letra = "";
                if(TipoNomina == 14)
                {
                    //semanal
                    letra = "S";
                }
                else if (TipoNomina == 15)
                {
                    //quincenal
                    letra = "Q";
                }
                else
                {
                    //mensual
                    letra = "Q";
                }
                return letra + string.Format("{0:000000}", NumeroNomina); 
            } 
        }

        [Display(Name = "Nomina")]
        [ColumnDB(Name = "Nombre", IsMapped = false, IsKey = false)]
        public string NominaRealDescripcion
        {
            get
            {
                if (TipoNomina == 14)
                {
                    //semanal
                    return "Semanal";
                }
                else if (TipoNomina == 15)
                {
                    //quincenal
                    return "Quincenal";
                }
                else
                {
                    //mensual
                    return "Mensual";
                }
            }
        }

        [Display(Name = "Tarjeta Acceso")]
        [ColumnDB(Name = "TarjetaAcceso", IsMapped = true, IsKey = false)]
        public string TarjetaAcceso { get; set; }

        [Display(Name = "Placas coche")]
        [ColumnDB(Name = "Placas", IsMapped = true, IsKey = false)]
        public string Placas { get; set; }

        [ColumnDB(Name = "Creado", IsMapped = true, IsKey = false)]
        public DateTime Creado { get; set; }


        [ColumnDB(Name = "Actualizado", IsMapped = true, IsKey = false)]
        public DateTime Actualizado { get; set; }

    }
}
