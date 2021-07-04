using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Validaciones
{
    public class PesoImagenValidacion:ValidationAttribute
    {
        private readonly int pesoMaximoEnMegabytes;
        public PesoImagenValidacion(int PesoMaximoEnMegabytes)
        {
            pesoMaximoEnMegabytes = PesoMaximoEnMegabytes;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            if (formFile == null)
            {
                return ValidationResult.Success;
            }

            if (formFile.Length > pesoMaximoEnMegabytes * 1024 * 1024)
            {
                return new ValidationResult($"El peso maximo del archivo no debe ser mayor a {pesoMaximoEnMegabytes} mb");
            }

            return ValidationResult.Success;
        }
    }
}
