using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class ActorPatchDTO
    {
        [StringLength(20)]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}
