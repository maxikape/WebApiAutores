using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext contex;

        public AutoresController(ApplicationDbContext contex)
        {
            this.contex = contex;
        }


        [HttpGet]
        public async Task<ActionResult<List<Autor>>> Get()
        {
            return await contex.Autores.Include(x => x.Libros).ToListAsync();
        }

        [HttpGet("primero")]
        public async Task<ActionResult<Autor>> PrimerAutor()
        {
            return await contex.Autores.FirstOrDefaultAsync();
        }


        [HttpGet("{id:int}/{Param2=persona}")]
        public async Task<ActionResult<Autor>> Get(int id, string persona)
        {

                var autor = await contex.Autores.FirstOrDefaultAsync(x => x.Id == id);

                if (autor == null)
                {
                    return NotFound();
                }

                return autor;
            
        }



        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
            contex.Add(autor);
            await contex .SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Autor autor, int id) 
        {
            if (autor.Id != id)
            {
                return BadRequest("el id del Autor No coincide con el ID de la URL");
            }

            contex.Update(autor);
            await contex.SaveChangesAsync();
            return Ok();


        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await contex.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            contex.Remove(new Autor {Id = id });
            await contex.SaveChangesAsync();
            return Ok();


        }



    }
}
