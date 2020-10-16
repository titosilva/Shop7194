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
            if (!ModelState.IsValid || model.role == "admin")
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

            return Ok(new
            {
                user = user,
                token = token
            });
        }

        [HttpGet]
        [Route("")]
        [Authorize(Roles="admin")]
        [ResponseCache(VaryByHeader="User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
        public async Task<ActionResult<List<User>>> GetAll(
            [FromServices] DataContext context
        ){
            try{
                var users = await context.Users.AsNoTracking().ToListAsync();
                return Ok(users);
            }catch{
                return Json(new {message = "Erro ao tentar obter usuarios"});
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles="admin")]
        public async Task<ActionResult<User>>  Put(
            [FromServices] DataContext context,
            int id,
            [FromBody] User model
        ){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != model.userId)
                return NotFound(new { message = "User id does not match"});

            try{
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model;
            }catch{
                return Json(new { message = "Não foi possível salvar novo usuário" });
            }
        }
    }
}