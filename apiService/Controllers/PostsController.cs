using apiService.Data;
using apiService.DTO;
using apiService.Extensions;
using apiService.Helpers;
using apiService.Interfaces;
using apiService.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public PostsController(DataContext context, IMapper mapper, IPhotoService photoService)
        {
            _context = context;
            _mapper = mapper;
            _photoService = photoService;
        }  

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PagedList<PostDTO>>>> Get([FromQuery] PostParams postParams)
        {
            if(!postParams.Type.Equals(0))
            {
                var query = _context.Posts.AsQueryable();
                query = query.Where(t => t.Type == postParams.Type);

                var posts = await PagedList<PostDTO>.CreateAsync(query
                     .AsNoTracking().ProjectTo<PostDTO>(_mapper.ConfigurationProvider),
                     postParams.PageNumber, postParams.PageSize);
                Response.AddPaginationHeader(new PaginatorHeader(posts.CurrentPage, posts.PageSize, posts.TotalCount, posts.TotalPage));

                return Ok(posts);
            } else
            {
                var query = _context.Posts.
                    ProjectTo<PostDTO>(_mapper.ConfigurationProvider)
                    .AsNoTracking();

                var posts = await PagedList<PostDTO>.CreateAsync(query, postParams.PageNumber, postParams.PageSize);
                Response.AddPaginationHeader(new PaginatorHeader(posts.CurrentPage, posts.PageSize, posts.TotalCount, posts.TotalPage));

                return Ok(posts);
            }
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

            var author = await _context.Authors.SingleOrDefaultAsync(x => x.Name.Trim().ToLower() == request.AuthorName.Trim().ToLower());
            if(author != null)
                post.AuthorID = author.Id;
            else post.AuthorID = 1;

            var type = await _context.Typologies.SingleOrDefaultAsync(x => x.Name.Trim().ToLower() == request.Type.Trim().ToLower());
            if (type != null)
                post.Type = type.Id;
            else post.Type = 1;

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return Ok(post);
        }

        [HttpPost("add-photo")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Post>> AddPhoto(IFormFile file, int id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == id);
            if (post == null)
                return NotFound();

            var result = await _photoService.AddPhotoAsync(file);
            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (post.Photos.Count == 0)
                photo.IsMain = true;

            post.Photos.Add(photo);

            int response = await _context.SaveChangesAsync();
            if (response > 0) 
                return NoContent();
            else 
                return BadRequest("Problem adding photo");

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

        private async Task<bool> PostExist(string title)
        {
            return await _context.Posts.AnyAsync(x => x.Title.ToLower() == title.ToLower());
        }
    }
}
