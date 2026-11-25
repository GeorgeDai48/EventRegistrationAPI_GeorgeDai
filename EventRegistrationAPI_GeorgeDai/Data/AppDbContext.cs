namespace EventRegistrationAPI_GeorgeDai.Data;
using Microsoft.EntityFrameworkCore;
using EventRegistrationAPI_GeorgeDai.Models;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
        public DbSet<Event> Events { get; set; }
        public DbSet<Registration> Registrations { get; set; }
}

