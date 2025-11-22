using FestivalFusion.API.Models.Domain;
using FestivalFusion.API.Models.DTO;
using FestivalFusion.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FestivalFusion.API.Controllers
{
    // https://localhost:7461/api/blog
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogRepository blogRepository;

        public BlogsController(IBlogRepository blogRepository)
        {
            this.blogRepository = blogRepository;
        }

        // POST: /api/blog
        [HttpPost]
        [Authorize(Roles = "Writer,Editor,Moderator")]
        public async Task<IActionResult> AddBlog(AddBlogRequestDto request)
        {
            // Map DTO to Domain Model
            var blog = new Blog
            {
                Title = request.Title,
                Content = request.Content,
                Category = request.Category,
                FeaturedImageUrl = request.FeaturedImageUrl,
                Author = request.Author,
                PublishedDate = request.PublishedDate,
                UpdatedDate = request.UpdatedDate,
                IsPublished = request.IsPublished,
                IsFeatured = request.IsFeatured
            };

            // Add and save changes
            blog = await blogRepository.CreateAsync(blog);

            // Domain model to DTO
            var response = new BlogDto
            {
                BlogId = blog.BlogId,
                Title = blog.Title,
                Content = blog.Content,
                Category = blog.Category,
                FeaturedImageUrl = blog.FeaturedImageUrl,
                Author = blog.Author,
                PublishedDate = blog.PublishedDate,
                UpdatedDate = blog.UpdatedDate,
                IsPublished = blog.IsPublished,
                IsFeatured = blog.IsFeatured
            };

            return CreatedAtAction(nameof(GetBlogById), new { id = blog.BlogId }, response);
        }

        // GET: /api/blog
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllBlogs()
        {
            var blogs = await blogRepository.GetAllAsync();

            // Convert Blog domain model to DTO
            var response = new List<BlogDto>();

            foreach (var blog in blogs)
            {
                response.Add(new BlogDto
                {
                    BlogId = blog.BlogId,
                    Title = blog.Title,
                    Content = blog.Content,
                    Category = blog.Category,
                    FeaturedImageUrl = blog.FeaturedImageUrl,
                    Author = blog.Author,
                    PublishedDate = blog.PublishedDate,
                    UpdatedDate = blog.UpdatedDate,
                    IsPublished = blog.IsPublished,
                    IsFeatured = blog.IsFeatured
                });
            }

            return Ok(response);
        }

        // GET: /api/blog/{id}
        [HttpGet]
        [Authorize]
        [Route("{id:int}")]
        public async Task<IActionResult> GetBlogById([FromRoute] int id)
        {
            var existingBlog = await blogRepository.GetById(id);

            if (existingBlog is null)
            {
                return NotFound();
            }

            // Convert to DTO
            var response = new BlogDto
            {
                BlogId = existingBlog.BlogId,
                Title = existingBlog.Title,
                Content = existingBlog.Content,
                Category = existingBlog.Category,
                FeaturedImageUrl = existingBlog.FeaturedImageUrl,
                Author = existingBlog.Author,
                PublishedDate = existingBlog.PublishedDate,
                UpdatedDate = existingBlog.UpdatedDate,
                IsPublished = existingBlog.IsPublished,
                IsFeatured = existingBlog.IsFeatured
            };

            return Ok(response);
        }

        // PUT: /api/blog/{id}
        [HttpPut]
        [Authorize(Roles = "Editor,Moderator")]
        [Route("{id:int}")]
        public async Task<IActionResult> EditBlog([FromRoute] int id, UpdateBlogRequestDto request)
        {
            // Convert DTO to Domain Model
            var blog = new Blog
            {
                BlogId = id,
                Title = request.Title,
                Content = request.Content,
                Category = request.Category,
                FeaturedImageUrl = request.FeaturedImageUrl,
                Author = request.Author,
                PublishedDate = request.PublishedDate,
                UpdatedDate = request.UpdatedDate,
                IsPublished = request.IsPublished,
                IsFeatured = request.IsFeatured
            };

            blog = await blogRepository.UpdateAsync(blog);

            if (blog == null)
            {
                return NotFound();
            }

            // Convert Domain model to DTO
            var response = new BlogDto
            {
                BlogId = id,
                Title = blog.Title,
                Content = blog.Content,
                Category = blog.Category,
                FeaturedImageUrl = blog.FeaturedImageUrl,
                Author = blog.Author,
                PublishedDate = blog.PublishedDate,
                UpdatedDate = blog.UpdatedDate,
                IsPublished = blog.IsPublished,
                IsFeatured = blog.IsFeatured
            };

            return Ok(response);
        }

        // DELETE: /api/blog/{id}
        [HttpDelete]
        [Authorize(Roles = "Editor")]
        [Route("{id:int}")]
        public async Task<IActionResult> RemoveBlog([FromRoute] int id)
        {
            var blog = await blogRepository.DeleteAsync(id);

            if (blog is null)
            {
                return NotFound();
            }

            // Convert Domain model to DTO
            var response = new BlogDto
            {
                BlogId = blog.BlogId,
                Title = blog.Title,
                Content = blog.Content,
                Category = blog.Category,
                FeaturedImageUrl = blog.FeaturedImageUrl,
                Author = blog.Author,
                PublishedDate = blog.PublishedDate,
                UpdatedDate = blog.UpdatedDate,
                IsPublished = blog.IsPublished,
                IsFeatured = blog.IsFeatured
            };

            return Ok(response);
        }
    }
}
