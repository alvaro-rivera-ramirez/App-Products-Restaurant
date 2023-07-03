using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using ApiRestaurant.Data;
using ApiRestaurant.Models;
using ApiRestaurant.DTO;
using System.IO;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ApiRestaurant.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public ProductController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: api/product
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        // GET: api/Producto/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(c => c.id_producto == id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // POST: api/Producto
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDTO productObject)
        {
            if (productObject == null)
            {
                return BadRequest("El objeto productObject es nulo.");
            }
            // Guardar la imagen en el servidor
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "img");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = $"{Guid.NewGuid()}_{productObject.name_file.FileName}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await productObject.name_file.CopyToAsync(fileStream);
            }

            // Guardar los datos en la base de datos
            var product = new Product
            {
                name = productObject.name,
                description = productObject.description,
                price = productObject.price,
                available = productObject.available,
                categoryId = productObject.categoryId,
                name_file = uniqueFileName
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            return Ok("Producto creado exitosamente.");
        }
    }
}
