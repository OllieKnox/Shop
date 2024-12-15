using Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Api.Contracts;
using Product.Api.Services;

namespace Product.Api.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Product>>> GetProducts()
        {
            return await productService.GetProducts();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Product>> GetProduct(int id)
        {
            try
            {
                return await productService.GetProduct(id);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize(Policy = "write:products")]
        public async Task<ActionResult<Models.Product>> CreateProduct([FromBody] ProductContract productContract)
        {
            var product = await productService.CreateProduct(productContract);
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "write:products")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductContract productContract)
        {
            try
            {
                await productService.UpdateProduct(id, productContract);
                return NoContent();
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }

        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "write:products")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await productService.DeleteProduct(id);
                return NoContent();
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
