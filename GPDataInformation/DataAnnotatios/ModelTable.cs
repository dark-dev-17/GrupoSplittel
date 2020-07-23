using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace GPDataInformation.DataAnnotatios
{
    public sealed class ModelTable : ValidationAttribute
    {
        public string Column { get; set; }        
        protected override ValidationResult IsValid(object country, ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Column) || string.IsNullOrWhiteSpace(Column))
            {
                return new ValidationResult("Por favor ingresa el nombre de la columna");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
