using FestivalFusion.API.Data;
using FestivalFusion.API.Modals.Domain;
using FestivalFusion.API.Models.Domain;
using FestivalFusion.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace FestivalFusion.API.Repositories.Implementation
{
    public class SponsorRepository : ISponsorRepository
    {
        private readonly FestivalContext dbContext;

        public SponsorRepository(FestivalContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Sponsor> CreateAsync(Sponsor sponsor)
        {
            await dbContext.Sponsors.AddAsync(sponsor);
            await dbContext.SaveChangesAsync();

            return sponsor;
        }

        public async Task<IEnumerable<Sponsor>> GetAllAsync()
        {
            return await dbContext.Sponsors.ToListAsync();
        }

        public async Task<Sponsor?> GetById(int id)
        {
            return await dbContext.Sponsors.FirstOrDefaultAsync(x => x.SponsorId == id);
        }

        public async Task<Sponsor?> UpdateAsync(Sponsor sponsor)
        {
            var existingSponsor = await dbContext.Sponsors.FirstOrDefaultAsync(x => x.SponsorId == sponsor.SponsorId);

            if (existingSponsor != null)
            {
                dbContext.Entry(existingSponsor).CurrentValues.SetValues(sponsor);
                await dbContext.SaveChangesAsync();
                return sponsor;
            }

            return null;
        }

        public async Task<Sponsor> DeleteAsync(int id)
        {
            var existingSponsor = await dbContext.Sponsors.FirstOrDefaultAsync(x => x.SponsorId == id);

            if (existingSponsor is null)
            {
                return null;
            }

            dbContext.Sponsors.Remove(existingSponsor);
            await dbContext.SaveChangesAsync();
            return existingSponsor;
        }
    }
}
