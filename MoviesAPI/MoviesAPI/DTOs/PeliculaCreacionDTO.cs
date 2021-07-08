using Microsoft.AspNetCore.Http;
using MoviesAPI.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class PeliculaCreacionDTO
    {
        public int Id { get; set; }
        [Required]
        public string Titulo { get; set; }
        public bool EnCines { get; set; }
        public DateTime FechaEstreno { get; set; }
        [PesoImagenValidacion(PesoMaximoEnMegabytes: 4)]
        [TipoArchivoValidacion(GrupoTipoArchivo.Imagen)]
        public IFormFile Poster { get; set; }

        public List<int> GenerosIds { get; set; }
    }
}
