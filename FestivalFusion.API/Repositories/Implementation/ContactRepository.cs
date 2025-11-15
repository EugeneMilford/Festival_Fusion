using FestivalFusion.API.Data;
using FestivalFusion.API.Modals.Domain;
using FestivalFusion.API.Models.Domain;
using FestivalFusion.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace FestivalFusion.API.Repositories.Implementation
{
    public class ContactRepository : IContactRepository
    {
        private readonly FestivalContext dbContext;

        public ContactRepository(FestivalContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Contact> CreateAsync(Contact contact)
        {
            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();
            return contact;
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            return await dbContext.Contacts.ToListAsync();
        }

        public async Task<Contact> DeleteAsync(int id)
        {
            var existingContact = await dbContext.Contacts.FirstOrDefaultAsync(x => x.ContactId == id);

            if (existingContact is null)
            {
                return null;
            }

            dbContext.Contacts.Remove(existingContact);
            await dbContext.SaveChangesAsync();
            return existingContact;
        }
    }
}
