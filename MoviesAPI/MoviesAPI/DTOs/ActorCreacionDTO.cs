using Microsoft.AspNetCore.Http;
using MoviesAPI.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class ActorCreacionDTO
    {     
        [Required]
        [StringLength(20)]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        [PesoImagenValidacion(PesoMaximoEnMegabytes:4)]
        [TipoArchivoValidacion(grupoTipoArchivo: GrupoTipoArchivo.Imagen )]
        public IFormFile Foto { get; set; }
    }
}
