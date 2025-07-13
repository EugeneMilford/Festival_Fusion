using FestivalFusion.API.Modals.Domain;

namespace FestivalFusion.API.Repositories.Interface
{
    public interface IFestivalRepository
    {
        Task<Festival> CreateAsync(Festival festival);
    }
}
