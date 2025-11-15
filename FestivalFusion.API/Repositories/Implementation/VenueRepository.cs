using FestivalFusion.API.Data;
using FestivalFusion.API.Modals.Domain;
using FestivalFusion.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace FestivalFusion.API.Repositories.Implementation
{
    public class VenueRepository : IVenueRepository
    {
        private readonly FestivalContext dbContext;

        public VenueRepository(FestivalContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Venue> CreateAsync(Venue venue)
        {
            await dbContext.Venues.AddAsync(venue);
            await dbContext.SaveChangesAsync();

            return venue;
        }

        public async Task<IEnumerable<Venue>> GetAllAsync()
        {
            return await dbContext.Venues.ToListAsync();
        }

        public async Task<Venue?> GetById(int id)
        {
            return await dbContext.Venues.FirstOrDefaultAsync(x => x.VenueId == id);
        }

        public async Task<Venue?> UpdateAsync(Venue venue)
        {
            var existingVenue = await dbContext.Venues.FirstOrDefaultAsync(x => x.VenueId == venue.VenueId);

            if (existingVenue != null)
            {
                dbContext.Entry(existingVenue).CurrentValues.SetValues(venue);
                await dbContext.SaveChangesAsync();
                return venue;
            }

            return null;
        }

        public async Task<Venue> DeleteAsync(int id)
        {
            var existingVenue = await dbContext.Venues.FirstOrDefaultAsync(x => x.VenueId == id);

            if (existingVenue is null)
            {
                return null;
            }

            dbContext.Venues.Remove(existingVenue);
            await dbContext.SaveChangesAsync();
            return existingVenue;
        }
    }
}
