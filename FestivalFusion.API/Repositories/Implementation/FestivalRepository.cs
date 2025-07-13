using FestivalFusion.API.Data;
using FestivalFusion.API.Modals.Domain;
using FestivalFusion.API.Repositories.Interface;

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
    }
}
