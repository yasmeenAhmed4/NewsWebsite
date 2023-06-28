using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsWebsite.BL.DTO;
using NewsWebsite.BL.Manager;
using NewsWebsite.DAL.Data.Models;
using NewsWebsite.DAL.Repos;
using static System.Net.Mime.MediaTypeNames;

namespace NewsWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private INewsManager manager { get; }
       
        public NewsController(INewsManager _manager)
        {
            manager = _manager;
           
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (manager.GetAll().Count() > 0)
            {
                return Ok(manager.GetAll());

            }
            return Ok();
        }


        [HttpGet("{id}")]
        public IActionResult GetByID(int id)
        {
            return Ok(manager.GetById(id));
        }

        [Authorize]
        [HttpPost("AddNews")]
        public IActionResult AddNews(NewsDTO a)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //a.image = AddImage();
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
        [HttpPut("EditNews")]
        public IActionResult UpdateNews(NewsDTO c)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    //author.Name = c.Name;
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
        public IActionResult DeleteNews(int id)
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
