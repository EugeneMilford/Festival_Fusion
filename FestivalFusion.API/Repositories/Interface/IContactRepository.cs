using FestivalFusion.API.Modals.Domain;
using FestivalFusion.API.Models.Domain;

namespace FestivalFusion.API.Repositories.Interface
{
    public interface IContactRepository
    {
        Task<Contact> CreateAsync(Contact contact);

        Task<IEnumerable<Contact>> GetAllAsync();

        Task<Contact> DeleteAsync(int id);
    }
}
