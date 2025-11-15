using FestivalFusion.API.Data;
using FestivalFusion.API.Modals.Domain;
using FestivalFusion.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace FestivalFusion.API.Repositories.Implementation
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly FestivalContext dbContext;

        public ArtistRepository(FestivalContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Artist> CreateAsync(Artist artist)
        {
            await dbContext.Artists.AddAsync(artist);
            await dbContext.SaveChangesAsync();

            return artist;
        }

        public async Task<IEnumerable<Artist>> GetAllAsync()
        {
            return await dbContext.Artists.ToListAsync();
        }

        public async Task<Artist?> GetById(int id)
        {
            return await dbContext.Artists.FirstOrDefaultAsync(x => x.ArtistId == id);
        }

        public async Task<Artist?> UpdateAsync(Artist artist)
        {
            var existingArtist = await dbContext.Artists.FirstOrDefaultAsync(x => x.ArtistId == artist.ArtistId);

            if (existingArtist != null)
            {
                dbContext.Entry(existingArtist).CurrentValues.SetValues(artist);
                await dbContext.SaveChangesAsync();
                return artist;
            }

            return null;
        }

        public async Task<Artist> DeleteAsync(int id)
        {
            var existingArtist = await dbContext.Artists.FirstOrDefaultAsync(x => x.ArtistId == id);

            if (existingArtist is null)
            {
                return null;
            }

            dbContext.Artists.Remove(existingArtist);
            await dbContext.SaveChangesAsync();
            return existingArtist;
        }
    }
}
