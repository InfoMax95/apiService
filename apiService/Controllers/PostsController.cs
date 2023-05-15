using apiService.Data;
using apiService.DTO;
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
        private readonly DataContext _context;
        public PostsController(DataContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> Get()
        {
            //var posts = await _context.Posts.ToListAsync();
            var posts = (from c in _context.Posts
                         join d in _context.Authors
                         on c.AuthorID equals d.Id
                         join b in _context.Typologies
                         on c.Type equals b.Id
                         //orderby c.Guid descending
                         select new PostToView { Posts = c, Author = d, Typology = b });
            return Ok(posts);
        }

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
        public async Task<ActionResult<Post>> Create(PostDTO request)
        {
            Post post = new Post();
            post.Title = request.Title;
            post.Description = request.Description; 
            post.Subtitle = request.Subtitle;
            post.Content = request.Content;
            post.Type = request.Type;
            post.AuthorID = request.AuthorID;

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return Ok(post);
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
