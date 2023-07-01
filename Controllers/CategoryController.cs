using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using ApiRestaurant.Data;
using ApiRestaurant.Models;
using ApiRestaurant.DTO;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ApiRestaurant.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/category
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.id_categoria == id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        // POST: api/Clientes
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Post([FromBody] CategoryDTO categoryObject)
        {
            _context.Categories.Add(new Category { name=categoryObject.name});

            await _context.SaveChangesAsync();
            var lastCategory= _context.Categories.ToList().Last();

            return CreatedAtAction(nameof(Post),new ListCategoryDTO { id=lastCategory.id_categoria,name=lastCategory.name});
        }

        // PUT: api/Clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CategoryDTO categoryObject)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.id_categoria == id);
            if (category == null)
                return NotFound();

            _context.Entry(category).CurrentValues.SetValues(categoryObject);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.id_categoria == id);
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
