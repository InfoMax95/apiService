using apiService.Entities;
using apiService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace apiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly BlogApiContext _context;
        public PostsController(BlogApiContext context) => _context = context;

        [HttpGet]
        public async Task<IEnumerable<Post>> Get()
            => await _context.Posts.ToListAsync();

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(Post), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var post = await _context.Posts.FindAsync(id);
            return post == null ? NotFound() : Ok(post);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new {id = post.Id}, post);
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] int id, Post post)
        {
            if (id != post.Id) return BadRequest(); 

            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var postToDelete = await _context.Posts.FindAsync(id);
            if (postToDelete == null) return NotFound();    

            _context.Posts.Remove(postToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
