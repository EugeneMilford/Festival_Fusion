using FestivalFusion.API.Modals.Domain;
using FestivalFusion.API.Models.Domain;

namespace FestivalFusion.API.Repositories.Interface
{
    public interface IBlogRepository
    {
        Task<Blog> CreateAsync(Blog blog);
        Task<IEnumerable<Blog>> GetAllAsync();
        Task<Blog?> GetById(int id);
        Task<Blog?> UpdateAsync(Blog blog);
        Task<Blog> DeleteAsync(int id);
    }
}
