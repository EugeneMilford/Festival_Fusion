using FestivalFusion.API.Modals.Domain;
using FestivalFusion.API.Models.Domain;
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
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<Contact> Contacts { get; set; }
    }
}
