using apiService.Data;
using apiService.DTO;
using apiService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypesController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        public TypesController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Type>>> GetTypes()
        {
            var types = await _context.Typologies.ToListAsync();
            return Ok(types);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Type>> GetType([FromRoute] int id)
        {
            var type = await _context.Typologies.FindAsync(id);
            return Ok(type);
        }

        [HttpPost("InsertType")]
        public async Task<ActionResult<Type>> InsertType(TypologyDTO request)
        {
            Typology _type = new Typology();
            _type.Name = request.Name.ToLower();
            _type.Description = request.Description;

            _context.Typologies.Add(_type);
            await _context.SaveChangesAsync();

            return Ok(_type);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Type>> UpdateType([FromRoute] int id, Typology _type)
        {
            if (id != _type.Id)
            {
                return BadRequest();
            }

            _context.Typologies.Entry(_type).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(_type);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteType([FromRoute] int id)
        {
            var typeToDelete = await _context.Typologies.FindAsync(id);
            if (typeToDelete == null)
                return BadRequest();

            _context.Typologies.Remove(typeToDelete);
            await _context.SaveChangesAsync();

            return Ok(typeToDelete.Name);
        }
        private async Task<bool> UserExist(string name)
        {
            return await _context.Authors.AnyAsync(x => x.Name == name.ToLower());
        }
    }
}
