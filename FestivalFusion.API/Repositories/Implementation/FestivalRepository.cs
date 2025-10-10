using FestivalFusion.API.Data;
using FestivalFusion.API.Modals.Domain;
using FestivalFusion.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace FestivalFusion.API.Repositories.Implementation
{
    public class FestivalRepository : IFestivalRepository
    {
        private readonly FestivalContext dbContext;

        public FestivalRepository(FestivalContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Festival> CreateAsync(Festival festival)
        {
            await dbContext.Festivals.AddAsync(festival);
            await dbContext.SaveChangesAsync();

            return festival;
        }

        public async Task<IEnumerable<Festival>> GetAllAsync()
        {
            return await dbContext.Festivals.ToListAsync();
        }

        public async Task<Festival?> GetById(int id)
        {
            return await dbContext.Festivals.FirstOrDefaultAsync(x => x.FestivalId == id);
        }

        public async Task<Festival?> UpdateAsync(Festival festival)
        {
            var existingCategory = await dbContext.Festivals.FirstOrDefaultAsync(x => x.FestivalId == festival.FestivalId);

            if (existingCategory != null)
            {
                dbContext.Entry(existingCategory).CurrentValues.SetValues(festival);
                await dbContext.SaveChangesAsync();
                return festival;
            }

            return null;
        }

        public async Task<Festival?> DeleteAsync(int id)
        {
            var existingFestival = await dbContext.Festivals.FirstOrDefaultAsync(x => x.FestivalId == id);

            if (existingFestival is null)
            {
                return null;
            }

            dbContext.Festivals.Remove(existingFestival);
            await dbContext.SaveChangesAsync();
            return existingFestival;
        }
    }
}
