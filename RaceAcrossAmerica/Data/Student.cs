namespace RaceAcrossAmerica.Data
{
    public class Student
    {
        // ID of the student
        // Name of the student
        // Group the student belongs to
        
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set;  }
        public string Group { get; set; } = "Unassigned";

        public string FullName => $"{FirstName} {LastName}";
    }
}
