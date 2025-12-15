using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RaceAcrossAmerica.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Checkpoint> Checkpoints { get; set; }
        public DbSet<RaceParticipant> RaceParticipants { get; set; }
    }
}
