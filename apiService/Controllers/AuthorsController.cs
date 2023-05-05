using apiService.Data;
using apiService.DTO;
using apiService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        public AuthorsController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            var authors = await _context.Authors.ToListAsync();
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor([FromRoute] int id)
        {
            var author = await _context.Authors.FindAsync(id);
            return Ok(author);
        }

        [HttpPost("InsertAuthor")]
        public async Task<ActionResult<Author>> InsertAuthor(AuthorDTO request)
        {
            Author author = new Author();
            author.Name = request.Name.ToLower();

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return Ok(author);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Author>> UpdateAuthor([FromRoute] int id, Author author)
        {
            if (id != author.Id)
            {
                return BadRequest();
            }

            _context.Authors.Entry(author).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(author);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAuthor([FromRoute] int id)
        {
            var authorToDelete = await _context.Authors.FindAsync(id);
            if(authorToDelete == null) 
                return BadRequest();

            _context.Authors.Remove(authorToDelete);
            await _context.SaveChangesAsync();

            return Ok(authorToDelete.Name);
        }
        private async Task<bool> UserExist(string name)
        {
            return await _context.Authors.AnyAsync(x => x.Name == name.ToLower());
        }
    }
}
