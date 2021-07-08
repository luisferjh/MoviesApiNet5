using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "peliculas";

        public PeliculasController(ApplicationDbContext _context, IMapper _mapper, IAlmacenadorArchivos _almacenadorArchivos)
        {
            context = _context;
            mapper = _mapper;
            almacenadorArchivos = _almacenadorArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<List<PeliculaDTO>>> Get()
        {
            var peliculas = context.Peliculas.AsQueryable();         
            var dtos = mapper.Map<List<PeliculaDTO>>(peliculas);
            return dtos;
        }

        [HttpGet("{id:int}", Name = "obtenerPelicula")]
        public async Task<ActionResult<PeliculaDTO>> Get(int id)
        {
            var entidad = await context.Peliculas.FirstOrDefaultAsync(f => f.Id == id);

            if (entidad == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<PeliculaDTO>(entidad);
            return dto;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = mapper.Map<Pelicula>(peliculaCreacionDTO);

            return Ok();
            //if (peliculaCreacionDTO.Poster != null)
            //{
            //    using (var memoryStream = new MemoryStream())
            //    {
            //        await peliculaCreacionDTO.Poster.CopyToAsync(memoryStream);
            //        var contenido = memoryStream.ToArray();
            //        var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);
            //        pelicula.Poster = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor,
            //            peliculaCreacionDTO.Poster.ContentType);
            //    }
            //}

            //context.Add(pelicula);
            //await context.SaveChangesAsync();
            //var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);

            //return new CreatedAtRouteResult("obtenerPelicula", new { id = peliculaDTO.Id }, peliculaCreacionDTO);
        }

        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var peliculaDB = await context.Peliculas.FirstOrDefaultAsync(f => f.Id == id);

            if (peliculaDB == null)
            {
                return NotFound();
            }

            peliculaDB = mapper.Map(peliculaCreacionDTO, peliculaDB);
            if (peliculaCreacionDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await peliculaCreacionDTO.Poster.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);
                    peliculaDB.Poster = await almacenadorArchivos.EditarArchivo(contenido, extension, contenedor,
                        peliculaDB.Poster,
                        peliculaCreacionDTO.Poster.ContentType);
                }
            }

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<PeliculaPatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var entidadDB = await context.Peliculas.FirstOrDefaultAsync(f => f.Id == id);

            if (entidadDB == null)
            {
                return NotFound();
            }

            var entidadDTO = mapper.Map<PeliculaPatchDTO>(entidadDB);

            patchDocument.ApplyTo(entidadDTO, ModelState);

            var esValido = TryValidateModel(entidadDTO);
            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(entidadDTO, entidadDB);

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Peliculas.AnyAsync(f => f.Id == id);

            if (!existe)
                return NotFound();

            context.Remove(new Pelicula() { Id = id });
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
