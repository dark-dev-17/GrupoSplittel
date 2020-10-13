using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(Name = "Puesto", IsMappedByLabels = false, IsStoreProcedure = true)]
    public class Puesto
    {
        [ColumnDB(Name = "IdDepartamento", IsMapped = true, IsKey = true)]
        public int IdPuesto { get; set; }
        [Required]
        [ColumnDB(Name = "DPU", IsMapped = true, IsKey = false)]
        public string DPU { get; set; }
        [Required]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public string Nombre { get; set; }
        [Required]
        [ColumnDB(Name = "DescripcionPuesto", IsMapped = true, IsKey = false)]
        public string DescripcionPuesto { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor selecciona una opción")]
        [ColumnDB(Name = "IdDepartamento", IsMapped = true, IsKey = false)]
        public int IdDepartamento { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:00}", ApplyFormatInEditMode = false)]
        [ColumnDB(Name = "SalarioMin", IsMapped = true, IsKey = false)]
        public double SalarioMin { set; get; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:00}", ApplyFormatInEditMode = false)]
        [ColumnDB(Name = "SalarioMax", IsMapped = true, IsKey = false)]
        public double SalarioMax { set; get; }
        [Required]
        [ColumnDB(Name = "HoraEntrada", IsMapped = true, IsKey = false)]
        public TimeSpan HoraEntrada { get; set; }
        [Required]
        [ColumnDB(Name = "HoraSalida", IsMapped = true, IsKey = false)]
        public TimeSpan HoraSalida { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor selecciona una opción")]
        [ColumnDB(Name = "IdUbicacion", IsMapped = true, IsKey = false)]
        public int IdUbicacion { get; set; }
        [ColumnDB(Name = "IdPuestoParent", IsMapped = true, IsKey = false)]
        public int IdPuestoParent { get; set; }
        [ColumnDB(Name = "RequisicionPersonal", IsMapped = true, IsKey = false)]
        public int RequisicionPersonal { get; set; }
        [ColumnDB(Name = "NumeroDPU", IsMapped = true, IsKey = false)]
        public int NumeroDPU { get; set; }


        [ColumnDB(Name = "Departamento", IsMapped = false, IsKey = false)]
        public Departamento Departamento { get;  set; }
        
        [ColumnDB(Name = "Ubicacion", IsMapped = false, IsKey = false)]
        public CatalogoOpcionesValores Ubicacion { get;  set; }
        
        [ColumnDB(Name = "Puesto", IsMapped = false, IsKey = false)]
        public Puesto PuestoParent { get;  set; }
    }
}
