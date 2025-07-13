using FestivalFusion.API.Modals.Domain;
using Microsoft.EntityFrameworkCore;

namespace FestivalFusion.API.Data
{
    public class FestivalContext : DbContext
    {
        public FestivalContext(DbContextOptions<FestivalContext> options) : base(options)
        {
        }

        public DbSet<Festival> Festivals { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    }
}
