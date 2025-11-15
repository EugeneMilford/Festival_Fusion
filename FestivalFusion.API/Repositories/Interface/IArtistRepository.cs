using FestivalFusion.API.Modals.Domain;

namespace FestivalFusion.API.Repositories.Interface
{
    public interface IArtistRepository
    {
        Task<Artist> CreateAsync(Artist artist);
        Task<IEnumerable<Artist>> GetAllAsync();
        Task<Artist?> GetById(int id);
        Task<Artist?> UpdateAsync(Artist artist);
        Task<Artist> DeleteAsync(int id);
    }
}
