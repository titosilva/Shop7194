using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers{
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Product>>> Get ([FromServices] DataContext context)
        {
            var products = await context.Products.Include(x => x.category).AsNoTracking().ToListAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> GetById(int id, [FromServices] DataContext context)
        {
            var product = await context.Products.Include(x => x.category).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return Ok(product);
        }

        [HttpGet]
        [Route("categories/{id:int}")]
        public async Task<ActionResult<List<Product>>> GetByCategory(int id, [FromServices] DataContext context)
        {
            var products = await context.Products.Include(x => x.category).AsNoTracking().Where(x => x.category.Id == id).ToListAsync();
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post ([FromBody] Product model, [FromServices] DataContext context)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            else
            {
                context.Products.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
        }
    }
}
