using EventsManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventsManagementApp.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Organizer> Organizers { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Registration>()
                .HasKey(r => new { r.EventId, r.ParticipantId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
