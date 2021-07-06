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
    [Route("api/actores")]
    public class ActoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "actores";

        public ActoresController(ApplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            this._context = context;
            this._mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get()
        {
            var actores = await _context.Actores.ToListAsync();
            var dtos = _mapper.Map<List<ActorDTO>>(actores);
            return dtos;
        }

        [HttpGet("{id:int}", Name = "obtenerActor")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            var entidad = await _context.Actores.FirstOrDefaultAsync(f => f.Id == id);

            if (entidad == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<ActorDTO>(entidad);
            return dto;
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actor = _mapper.Map<Actor>(actorCreacionDTO);

            // if (actorCreacionDTO.Foto != null)
            // {
            //     using (var memoryStream = new MemoryStream())
            //     {
            //         await actorCreacionDTO.Foto.CopyToAsync(memoryStream);
            //         var contenido = memoryStream.ToArray();
            //         var extension = Path.GetExtension(actorCreacionDTO.Foto.FileName);
            //         actor.Foto = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor,                     
            //             actorCreacionDTO.Foto.ContentType);
            //     }
            // }

            // _context.Add(actor);
            //await _context.SaveChangesAsync();
            //var actorDTO = _mapper.Map<ActorDTO>(actor);

            // return new CreatedAtRouteResult("obtenerActor", new { id = actorDTO.Id }, actorCreacionDTO);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actorDB = await _context.Actores.FirstOrDefaultAsync(f => f.Id == id);

            if (actorDB == null)
            {
                return NotFound();
            }

            actorDB = _mapper.Map(actorCreacionDTO, actorDB);
            if (actorCreacionDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreacionDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreacionDTO.Foto.FileName);
                    actorDB.Foto = await almacenadorArchivos.EditarArchivo(contenido, extension, contenedor,
                        actorDB.Foto,
                        actorCreacionDTO.Foto.ContentType);
                }
            }
            //var entidad = _mapper.Map<Actor>(actorCreacionDTO);
            //entidad.Id = id;
            //_context.Entry(entidad).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ActorPatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var entidadDB = await _context.Actores.FirstOrDefaultAsync(f => f.Id == id);

            if (entidadDB == null)
            {
                return NotFound();
            }

            var entidadDTO = _mapper.Map<ActorPatchDTO>(entidadDB);

            patchDocument.ApplyTo(entidadDTO, ModelState);

            var esValido = TryValidateModel(entidadDTO);
            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(entidadDTO, entidadDB);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await _context.Actores.AnyAsync(f => f.Id == id);

            if (!existe)
                return NotFound();

            _context.Remove(new Actor() { Id = id });
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
