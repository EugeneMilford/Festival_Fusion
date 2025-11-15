using FestivalFusion.API.Modals.Domain;
using FestivalFusion.API.Models.Domain;

namespace FestivalFusion.API.Repositories.Interface
{
    public interface ISponsorRepository
    {
        Task<Sponsor> CreateAsync(Sponsor sponsor);
        Task<IEnumerable<Sponsor>> GetAllAsync();
        Task<Sponsor?> GetById(int id);
        Task<Sponsor?> UpdateAsync(Sponsor sponsor);
        Task<Sponsor> DeleteAsync(int id);
    }
}
