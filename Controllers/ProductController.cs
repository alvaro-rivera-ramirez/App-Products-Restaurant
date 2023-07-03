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
        public async Task<IActionResult> CreateProduct([FromForm]CreateProductDTO productObject)
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
                name=productObject.name,
                description = productObject.description,
                price = productObject.price,
                available = productObject.available,
                categoryId = productObject.categoryId,
                name_file = uniqueFileName
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            return Ok("Producto creado exitosamente.");
            // if (string.IsNullOrEmpty(productObject.description))
            // {
            //     return BadRequest("El campo description es requerido.");
            // }

            // Subir el archivo
            //var fileName = await WriteFile(imageFile, cancellationToken);

            // Verificar si se pudo subir el archivo correctamente
            // if (string.IsNullOrEmpty(fileName))
            // {
            //     return BadRequest("No se pudo subir el archivo.");
            // }

            // Asociar la ruta del archivo al objeto de producto
            // productObject.name_file = fileName;

            // Resto del código para crear el producto y guardarlo en la base de datos
            // var newProduct = new Product
            // {
            //     name = productObject.name,
            //     description = productObject.description,
            //     price = productObject.price,
            //     available = productObject.available,
            //     categoryId = productObject.categoryId,
            //     name_file = Path.Combine(Directory.GetCurrentDirectory(), "img"), // Asociar la ruta del archivo al objeto de producto
            // };

            // _context.Products.Add(newProduct);
            // _context.SaveChanges();

            // var createdProduct = new ProductDTO
            // {
            //     id_producto = newProduct.id_producto,
            //     name = newProduct.name,
            // };

            // return CreatedAtAction(nameof(CreateProduct), createdProduct);
        }

        private async Task<string> WriteFile(IFormFile file, CancellationToken cancellationToken)
        {
            string filename = "";
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                filename = DateTime.Now.Ticks.ToString() + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "img");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "img", filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir al subir el archivo
            }
            return filename;
        }

        // PUT: api/Clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProductDTO productObject)
        {
            var product = await _context.Products.FirstOrDefaultAsync(c => c.id_producto== id);
            if (product == null)
                return NotFound();

            _context.Entry(product).CurrentValues.SetValues(productObject);
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
            var product = await _context.Products.FirstOrDefaultAsync(c => c.id_producto == id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
