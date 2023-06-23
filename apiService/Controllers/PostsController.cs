using apiService.Data;
using apiService.DTO;
using apiService.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        private readonly IMapper _mapper;
        public PostsController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }  

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> Get()
        {
            var posts = await _context.Posts
                .ProjectTo<PostDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(posts);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(Post), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PostDTO>> GetPostById([FromRoute] int id)
        {
            try
            {
                var post = await _context.Posts.Where(x => x.Id == id)
                    .ProjectTo<PostDTO>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync();

                return post == null ? NotFound() : Ok(post);

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }

        }

        [HttpGet]
        [Route("find-by-name/{title}")]
        [ProducesResponseType(typeof(Post), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PostDTO>> GetPostByName([FromRoute] string title)
        {
            try
            {
                var post = await _context.Posts.Where(x => x.Title == title)
                    .ProjectTo<PostDTO>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync();

                return post == null ? NotFound() : Ok(post);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Post>> CreatePost(PostDTO request)
        {
            Post post = new Post();
            post.Title = request.Title;
            post.Description = request.Description; 
            post.Subtitle = request.Subtitle;
            post.Content = request.Content;

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return Ok(post);
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] int id, Post request)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) 
                return NotFound();

            if (id != request.Id) return BadRequest(); 

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
