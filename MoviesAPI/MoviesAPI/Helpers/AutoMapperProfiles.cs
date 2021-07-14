using AutoMapper;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();

            CreateMap<SalaDeCine, SalaDeCineDTO>()
                .ForMember(dest => dest.Latitud, x => x.MapFrom(y => y.Ubicacion.Y))
                .ForMember(dest => dest.Longitud, x => x.MapFrom(y => y.Ubicacion.X));    

            CreateMap<SalaDeCineDTO, SalaDeCine>()
                .ForMember(dest => dest.Ubicacion, x => x.MapFrom(y =>
                geometryFactory.CreatePoint(new Coordinate(y.Longitud, y.Latitud))));

            CreateMap<SalaDeCineCreacionDTO, SalaDeCine>()
                  .ForMember(dest => dest.Ubicacion, x => x.MapFrom(y =>
                geometryFactory.CreatePoint(new Coordinate(y.Longitud, y.Latitud)))); ;

            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>()
                .ForMember(x => x.Foto, options => options.Ignore());
            CreateMap<ActorPatchDTO, Actor>().ReverseMap();
            
            CreateMap<PeliculaDTO, Pelicula>().ReverseMap();
            CreateMap<PeliculaCreacionDTO, Pelicula>()
              .ForMember(x => x.Poster, options => options.Ignore())
              .ForMember(x => x.PeliculasGeneros, options => options.MapFrom(MapPeliculasGeneros))
              .ForMember(x => x.PeliculasActores, options => options.MapFrom(MapPeliculasActores));
            CreateMap<PeliculaPatchDTO, Pelicula>().ReverseMap();

            CreateMap<Pelicula, PeliculaDetalleDTO>()
                .ForMember(dest => dest.Generos, options => options.MapFrom(MapPeliculasGeneros))
                .ForMember(dest => dest.Actores, options => options.MapFrom(MapPeliculasActores));

        }

        private List<GeneroDTO> MapPeliculasGeneros(Pelicula pelicula, PeliculaDetalleDTO peliculaDetalleDTO) 
        {
            List<GeneroDTO> resultado = new List<GeneroDTO>();

            if (pelicula.PeliculasGeneros == null)            
                return resultado;
            
            foreach (PeliculasGeneros generoPelicula in pelicula.PeliculasGeneros)            
                resultado.Add(new GeneroDTO() { Id = generoPelicula.GeneroId, Nombre = generoPelicula.Genero.Nombre});
            
            return resultado;
        }

        private List<ActorPeliculaDetalleDTO> MapPeliculasActores(Pelicula pelicula, PeliculaDetalleDTO peliculaDetalleDTO)
        {
            List<ActorPeliculaDetalleDTO> resultado = new List<ActorPeliculaDetalleDTO>();

            if (pelicula.PeliculasActores == null)
                return resultado;

            foreach (PeliculasActores actorPelicula in pelicula.PeliculasActores)
                resultado.Add(new ActorPeliculaDetalleDTO() {  
                    ActorId = actorPelicula.ActorId,  
                    Personaje = actorPelicula.Personaje,
                     NombrePersona  =  actorPelicula.Actor.Nombre
                     });

            return resultado;
        }

        private List<PeliculasGeneros> MapPeliculasGeneros(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula) 
        {
            List<PeliculasGeneros> resultado = new List<PeliculasGeneros>();
            if (peliculaCreacionDTO.GenerosIds == null || peliculaCreacionDTO.GenerosIds.Count() <= 0)
            {
                return resultado;
            }

            foreach (int id in peliculaCreacionDTO.GenerosIds)
            {
                resultado.Add(new PeliculasGeneros { GeneroId = id});
            }

            return resultado;
        }

        private List<PeliculasActores> MapPeliculasActores(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            List<PeliculasActores> resultado = new List<PeliculasActores>();
            if (peliculaCreacionDTO.Actores == null || peliculaCreacionDTO.Actores.Count() <= 0)
            {
                return resultado;
            }

            foreach (ActorPeliculasCreacionDTO actor in peliculaCreacionDTO.Actores)
            {
                resultado.Add(new PeliculasActores { ActorId = actor.ActorId, Personaje = actor.Personaje });
            }

            return resultado;
        }
    }
}
