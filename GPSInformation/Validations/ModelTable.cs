using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Validations
{
    public sealed class ModelTable : ValidationAttribute
    {
        public string Column { get; set; }

        public ModelTable(string Column)
        {
            this.Column = Column;
        }

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
