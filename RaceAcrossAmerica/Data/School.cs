namespace RaceAcrossAmerica.Data
{
    public class School
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        
        public List<ApplicationUser> Users { get; set; } = new();
        public List<Student> Students { get; set; } = new();
        public List<Race> Races { get; set; } = new();
    }
}