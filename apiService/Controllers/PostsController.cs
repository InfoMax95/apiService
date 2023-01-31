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
        private readonly PostContext _context;
        public PostsController(PostContext context) => _context = context;

        [HttpGet]
        public async Task<IEnumerable<Post>> Get()
            => await _context.Posts.ToListAsync();

        [HttpGet("id")]
        [ProducesResponseType(typeof(Post), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(int id)
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
    }
}
