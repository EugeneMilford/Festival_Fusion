using FestivalFusion.API.Modals.Domain;

namespace FestivalFusion.API.Repositories.Interface
{
    public interface IFestivalRepository
    {
        Task<Festival> CreateAsync(Festival festival);
        Task<IEnumerable<Festival>> GetAllAsync();
        Task<Festival?> GetById(int id);
        Task<Festival?> UpdateAsync(Festival festival);
        Task<Festival> DeleteAsync(int id);
    }
}
