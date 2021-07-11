using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class FiltroPeliculasDTO
    {
        public int Pagina { get; set; }
        public int CantidadRegistroPorPagina { get; set; }
        public PaginacionDTO Paginacion
        {
            get { return new PaginacionDTO() { Pagina = Pagina, CantidadRegistrosPorPagina = CantidadRegistroPorPagina }; }
        }

        public string Titulo { get; set; }
        public int GeneroId { get; set; }
        public bool EnCines { get; set; }
        public bool ProximosEstrenos { get; set; }
        public string CampoOrdernar { get; set; }
        public bool OrdenAscendente { get; set; } = true;
    }
}
