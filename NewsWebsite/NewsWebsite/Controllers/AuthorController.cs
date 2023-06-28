using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsWebsite.BL.DTO;
using NewsWebsite.BL.Manager;
using NewsWebsite.DAL.Data.Models;
using NewsWebsite.DAL.Repos;

namespace NewsWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private IAuthorManager manager;

        public AuthorController(IAuthorManager manager)
        {
            this.manager = manager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (manager.GetAll().Count() > 0)
            {
                return Ok(manager.GetAll());

            }
            return Ok("");
        }


        [HttpGet("{id}")]
        public IActionResult GetByID(int id)
        {
            return Ok(manager.GetById(id));
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddAuthor(AuthorDTO a)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    manager.Insert(a);
                    manager.Save();
                    return Created("url", a);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPut("EditAuthor")]
        public IActionResult UpdateAuthor(AuthorDTO c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    manager.Update(c);
                    manager.Save();
                    return NoContent();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest();
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public IActionResult DeleteAuthor(int id)
        {
            var c = manager.GetById(id);
            if (c == null)
            {
                return NotFound();
            }
            else
            {
                manager.Delete(id);
                manager.Save();
                return Ok(c);
            }
        }
    }
}
