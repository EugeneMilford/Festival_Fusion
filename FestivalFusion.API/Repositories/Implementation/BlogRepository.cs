using FestivalFusion.API.Data;
using FestivalFusion.API.Modals.Domain;
using FestivalFusion.API.Models.Domain;
using FestivalFusion.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace FestivalFusion.API.Repositories.Implementation
{
    public class BlogRepository : IBlogRepository
    {
        private readonly FestivalContext dbContext;

        public BlogRepository(FestivalContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Blog> CreateAsync(Blog blog)
        {
            await dbContext.Blogs.AddAsync(blog);
            await dbContext.SaveChangesAsync();

            return blog;
        }

        public async Task<IEnumerable<Blog>> GetAllAsync()
        {
            return await dbContext.Blogs.ToListAsync();
        }

        public async Task<Blog?> GetById(int id)
        {
            return await dbContext.Blogs.FirstOrDefaultAsync(x => x.BlogId == id);
        }

        public async Task<Blog?> UpdateAsync(Blog blog)
        {
            var existingBlogs = await dbContext.Blogs.FirstOrDefaultAsync(x => x.BlogId == blog.BlogId);

            if (existingBlogs != null)
            {
                dbContext.Entry(existingBlogs).CurrentValues.SetValues(blog);
                await dbContext.SaveChangesAsync();
                return blog;
            }

            return null;
        }

        public async Task<Blog> DeleteAsync(int id)
        {
            var existingBlogs = await dbContext.Blogs.FirstOrDefaultAsync(x => x.BlogId == id);

            if (existingBlogs is null)
            {
                return null;
            }

            dbContext.Blogs.Remove(existingBlogs);
            await dbContext.SaveChangesAsync();
            return existingBlogs;
        }
    }
}
