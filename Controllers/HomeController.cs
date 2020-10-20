using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [Route("home")]
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<dynamic>> Get([FromServices] DataContext context)
        {
            var admin = new User { userId = 1, userName = "tito", password = "batata", role = "admin"};
            var user = new User { userId = 2, userName = "isabelle", password = "batata", role = "user"};

            var category = new Category { Id = 1, Title = "Casamento" };
            var product = new Product { Id = 1, category = category, name = "aliancas", price = 999 }; 

            context.Users.Add(admin);
            context.Users.Add(user);
            context.Categories.Add(category);
            context.Products.Add(product);

            await context.SaveChangesAsync();

            return Ok(new
            {
                message = "Daddos configurados",
                data = new {
                    admin = admin,
                    user = user,
                    category = category,
                    product = product
                }
            });
        }
    }
}