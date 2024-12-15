using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.Api.Contexts;
using Product.Api.Contracts;
using Product.Api.Converters;

namespace Product.Api.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class ProductController(ProductContext productContext, IProductConverter productConverter) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Product>>> GetProducts()
        {
            return await productContext.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Product>> GetProduct(int id)
        {
            var product = await productContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        [Authorize(Policy = "write:products")]
        public async Task<ActionResult<Models.Product>> PostProduct([FromBody] ProductContract productContract)
        {
            var product = productConverter.ToModel(productContract);

            productContext.Products.Add(product);
            await productContext.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "write:products")]
        public async Task<IActionResult> PutProduct(int id, [FromBody] ProductContract productContract)
        {
            var product = productConverter.ToModel(productContract);
            product.Id = id;

            productContext.Entry(product).State = EntityState.Modified;

            try
            {
                await productContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "write:products")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await productContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            productContext.Products.Remove(product);
            await productContext.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return productContext.Products.Any(e => e.Id == id);
        }
    }
}
