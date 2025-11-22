using FestivalFusion.API.Controllers;
using FestivalFusion.API.Modals.Domain;
using FestivalFusion.API.Models.Domain;
using FestivalFusion.API.Models.DTO;
using FestivalFusion.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FestivalFusion.Tests.Controllers
{
    public class BlogsControllerTests
    {
        private readonly Mock<IBlogRepository> blogRepoMock;
        private readonly BlogsController controller;

        public BlogsControllerTests()
        {
            blogRepoMock = new Mock<IBlogRepository>();
            controller = new BlogsController(blogRepoMock.Object);
        }

        [Fact]
        public async Task AddBlog_ReturnsCreatedWithDto()
        {
            var request = new AddBlogRequestDto
            {
                Title = "Test Blog",
                Content = "Content",
                Category = "General",
                FeaturedImageUrl = "img",
                Author = "Author",
                PublishedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                IsPublished = true,
                IsFeatured = false
            };

            blogRepoMock
                .Setup(r => r.CreateAsync(It.IsAny<Blog>()))
                .ReturnsAsync((Blog b) =>
                {
                    b.BlogId = 1;
                    return b;
                });

            var result = await controller.AddBlog(request);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var dto = Assert.IsType<BlogDto>(created.Value);
            Assert.Equal(1, dto.BlogId);
            Assert.Equal(request.Title, dto.Title);
            Assert.Equal(request.Author, dto.Author);
        }

        [Fact]
        public async Task GetAllBlogs_ReturnsOkWithList()
        {
            var blogs = new List<Blog>
            {
                new Blog { BlogId = 1, Title = "B1", Content = "c1", Category = "cat1", FeaturedImageUrl = "i1", Author = "a1", PublishedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, IsPublished = true, IsFeatured = false },
                new Blog { BlogId = 2, Title = "B2", Content = "c2", Category = "cat2", FeaturedImageUrl = "i2", Author = "a2", PublishedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, IsPublished = false, IsFeatured = true }
            };

            blogRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(blogs);

            var result = await controller.GetAllBlogs();

            var ok = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsAssignableFrom<IEnumerable<BlogDto>>(ok.Value);
            Assert.NotEmpty(dtos);
        }

        [Fact]
        public async Task GetBlogById_Found_ReturnsOk()
        {
            var blog = new Blog
            {
                BlogId = 10,
                Title = "FoundBlog",
                Content = "c",
                Category = "cat",
                FeaturedImageUrl = "img",
                Author = "a",
                PublishedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                IsPublished = true,
                IsFeatured = false
            };

            blogRepoMock.Setup(r => r.GetById(10)).ReturnsAsync(blog);

            var result = await controller.GetBlogById(10);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<BlogDto>(ok.Value);
            Assert.Equal(blog.BlogId, dto.BlogId);
            Assert.Equal(blog.Title, dto.Title);
        }

        [Fact]
        public async Task GetBlogById_NotFound_ReturnsNotFound()
        {
            blogRepoMock.Setup(r => r.GetById(999)).ReturnsAsync((Blog?)null);

            var result = await controller.GetBlogById(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditBlog_Success_ReturnsOk()
        {
            var updateRequest = new UpdateBlogRequestDto
            {
                Title = "Updated",
                Content = "upd",
                Category = "upd",
                FeaturedImageUrl = "img-upd",
                Author = "author-upd",
                PublishedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                IsPublished = true,
                IsFeatured = true
            };

            var updatedBlog = new Blog
            {
                BlogId = 5,
                Title = updateRequest.Title,
                Content = updateRequest.Content,
                Category = updateRequest.Category,
                FeaturedImageUrl = updateRequest.FeaturedImageUrl,
                Author = updateRequest.Author,
                PublishedDate = updateRequest.PublishedDate,
                UpdatedDate = updateRequest.UpdatedDate,
                IsPublished = updateRequest.IsPublished,
                IsFeatured = updateRequest.IsFeatured
            };

            blogRepoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Blog>()))
                .ReturnsAsync(updatedBlog);

            var result = await controller.EditBlog(5, updateRequest);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<BlogDto>(ok.Value);
            Assert.Equal(5, dto.BlogId);
            Assert.Equal(updateRequest.Title, dto.Title);
        }

        [Fact]
        public async Task EditBlog_NotFound_ReturnsNotFound()
        {
            blogRepoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Blog>()))
                .ReturnsAsync((Blog?)null);

            var updateRequest = new UpdateBlogRequestDto
            {
                Title = "X",
                Content = "x",
                Category = "x",
                FeaturedImageUrl = "x",
                Author = "x",
                PublishedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                IsPublished = false,
                IsFeatured = false
            };

            var result = await controller.EditBlog(1234, updateRequest);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task RemoveBlog_Success_ReturnsOk()
        {
            var blog = new Blog
            {
                BlogId = 7,
                Title = "ToDelete",
                Content = "c",
                Category = "cat",
                FeaturedImageUrl = "img",
                Author = "a",
                PublishedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                IsPublished = false,
                IsFeatured = false
            };

            blogRepoMock.Setup(r => r.DeleteAsync(7)).ReturnsAsync(blog);

            var result = await controller.RemoveBlog(7);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<BlogDto>(ok.Value);
            Assert.Equal(7, dto.BlogId);
        }

        [Fact]
        public async Task RemoveBlog_NotFound_ReturnsNotFound()
        {
            blogRepoMock.Setup(r => r.DeleteAsync(42)).ReturnsAsync((Blog?)null);

            var result = await controller.RemoveBlog(42);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
