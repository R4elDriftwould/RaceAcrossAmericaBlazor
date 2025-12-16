using RaceAcrossAmerica.Data;

namespace RaceAcrossAmerica.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Ensure the database is created
            await dbContext.Database.EnsureCreatedAsync();

            // Check if any schools exist
            if (!dbContext.Schools.Any())
            {
                var defaultSchool = new School
                {
                    Name = "Race Across School of Athletes"
                };

                dbContext.Schools.Add(defaultSchool);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
