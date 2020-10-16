using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Shop.Models;
using Shop.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Shop.Controllers{
    [Route("v1/categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        [ResponseCache(VaryByHeader="User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
        // Use this response cache to invert (using AddResponseCaching to Startup.cs will cause all endpoints to have caching, and
        // if that is used, the line below can be used to remove the cache from a specific endpoint).
        // [ResponseCache(Duration=0, Location=ResponseCacheLocation.None, NoStore=true)]
        public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
        {
            // As no tracking: desliga o retorno de proxy do DataContext (apenas retorna uma categoria, sem outras informações do EF)
            var categories = await context.Categories.AsNoTracking().ToListAsync();
            return Ok(categories);
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Category>> GetById(int id, [FromServices] DataContext context)
        {
            var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return Ok(category);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles="admin")]
        public async Task<ActionResult<Category>> Post([FromBody]Category model, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            try
            {
                context.Categories.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            } catch {
                return BadRequest(new { message = "Error while trying to save model in database" });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles="admin")]
        public async Task<ActionResult<Category>> Put(int id, [FromBody]Category model, [FromServices] DataContext context)
        {
            if(model.Id != id)
                return NotFound(new { message = "Category not found" });
            
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry<Category>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            } catch {
                return BadRequest(new { message = "Error while trying to save changes on database" });
            }
        }
        
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles="admin")]
        public async Task<ActionResult> Delete(int id, [FromServices] DataContext context)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if(category==null)
                return NotFound(new { message = "Could not find category "+id.ToString()});

            try{
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return Ok(new { message = "Category deleted succesfully" });
            }catch{
                return BadRequest("Failed to remove category from database");
            }

        }

    }
}