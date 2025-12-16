using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RaceAcrossAmerica.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<School> Schools { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Checkpoint> Checkpoints { get; set; }
        public DbSet<RaceParticipant> RaceParticipants { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Define the relationships explicitly to ensure Cascade Delete works or is restricted as preferred

            // A School has many Users
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.School)
                .WithMany(s => s.Users)
                .HasForeignKey(u => u.SchoolId)
                .OnDelete(DeleteBehavior.Restrict); // Don't delete school if users exist

            // A School has many Students
            builder.Entity<Student>()
                .HasOne(s => s.School)
                .WithMany(school => school.Students)
                .HasForeignKey(s => s.SchoolId)
                .OnDelete(DeleteBehavior.Cascade); // Delete students if school is deleted

            // A School has many Races
            builder.Entity<Race>()
                .HasOne(r => r.School)
                .WithMany(school => school.Races)
                .HasForeignKey(r => r.SchoolId)
                .OnDelete(DeleteBehavior.Cascade); // Delete races if school is deleted

            builder.Entity<RaceParticipant>()
                .HasOne(rp => rp.Student)
                .WithMany() // Student entity doesn't have a list of participants
                .HasForeignKey(rp => rp.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
