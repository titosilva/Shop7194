using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Shop.Services;

namespace Shop.Controllers
{
    [Route("users")]
    public class UserController : Controller
    {
        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Post(
            [FromServices] DataContext context,
            [FromBody] User model
        ){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try{
                context.Users.Add(model);
                await context.SaveChangesAsync();
                return model;
            }
            catch(Exception)
            {
                return BadRequest(new {message= "Falha ao tentar criar o usuário"});
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate (
            [FromServices] DataContext context,
            [FromBody] User model
        ){
            var user = await context.Users.AsNoTracking()
                                .Where(x => x.userName == model.userName && x.password == model.password)
                                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            var token = TokenService.GenerateToken(user);

            return new
            {
                user = user,
                token = token
            };
        }
    }
}