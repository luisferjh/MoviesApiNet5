using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api/generos")]
    public class GenerosController : CustomBaseController
    {
        public GenerosController(ApplicationDbContext context, IMapper mapper)
            :base(context, mapper)
        {           
        }

        [HttpGet]
        public async Task<ActionResult<List<GeneroDTO>>> Get()
        {
            //var entidades = await _context.Generos.ToListAsync();
            //var dtos = _mapper.Map<List<GeneroDTO>>(entidades);
            //return dtos;
            return await Get<Genero, GeneroDTO>();
        }

        [HttpGet("{id:int}", Name = "obtenerGenero")]
        public async Task<ActionResult<GeneroDTO>> Get(int id)
        {
            return await Get<Genero, GeneroDTO>(id);
        }
        //[HttpGet("{id:int}", Name = "obtenerGenero")]
        //public async Task<ActionResult<GeneroDTO>> Get(int id)
        //{
        //    var entidad = await _context.Generos.FirstOrDefaultAsync(f => f.Id == id);

        //    if (entidad == null)
        //    {
        //        return NotFound();
        //    }

        //    var dto = _mapper.Map<GeneroDTO>(entidad);
        //    return dto;
        //}

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            return await Post<GeneroCreacionDTO, Genero, GeneroDTO>(generoCreacionDTO, "obtenerGenero");
        }

        //[HttpPost]
        //public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO)
        //{
        //    var entidad = _mapper.Map<Genero>(generoCreacionDTO);
        //    _context.Add(entidad);
        //    await _context.SaveChangesAsync();
        //    var generoDTO = _mapper.Map<GeneroDTO>(entidad);

        //    return new CreatedAtRouteResult("obtenerGenero", new { id = generoDTO.Id }, generoDTO);

        //}

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            return await Put<GeneroCreacionDTO, Genero>(id, generoCreacionDTO);
        }

        //[HttpPut("{id}")]
        //public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        //{
        //    var entidad = _mapper.Map<Genero>(generoCreacionDTO);
        //    entidad.Id = id;
        //    _context.Entry(entidad).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();
        //    return NoContent();
        //}


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Genero>(id);
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete(int id)
        //{
        //    var existe = await _context.Generos.AnyAsync(f => f.Id == id);

        //    if (!existe)
        //        return NotFound();

        //    _context.Remove(new Genero() { Id = id });
        //    await _context.SaveChangesAsync();
            
        //    return NoContent();
        //}
    }
}
