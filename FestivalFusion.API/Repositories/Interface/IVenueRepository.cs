using FestivalFusion.API.Modals.Domain;

namespace FestivalFusion.API.Repositories.Interface
{
    public interface IVenueRepository
    {
        Task<Venue> CreateAsync(Venue venue);
        Task<IEnumerable<Venue>> GetAllAsync();
        Task<Venue?> GetById(int id);
        Task<Venue?> UpdateAsync(Venue venue);
        Task<Venue> DeleteAsync(int id);
    }
}
