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
        public async Task<IActionResult> GetProducts()
        {
            var products = await productService.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var products = await productService.GetProduct(id);
                return Ok(products);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize(Policy = "write:products")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductContract productContract)
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
