using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Validaciones
{
    public class TipoArchivoValidacion:ValidationAttribute
    {
        private readonly string[] tiposValidos;
        public TipoArchivoValidacion(string[] _tiposValidos)
        {
            tiposValidos = _tiposValidos;
        }

        public TipoArchivoValidacion(GrupoTipoArchivo grupoTipoArchivo)
        {
            if (grupoTipoArchivo == GrupoTipoArchivo.Imagen)
            {
                tiposValidos = new string[] { "image/jpeg", "image/png", "image/gif" };
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            if (!tiposValidos.Contains(formFile.ContentType))
            {
                return new ValidationResult($"El tipo de archivo debe ser uno de los siguientes: {string.Join(", ", tiposValidos)}");
            }
            return ValidationResult.Success;
        }
    }
}
