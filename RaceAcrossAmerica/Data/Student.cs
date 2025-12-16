namespace RaceAcrossAmerica.Data
{
    public class Student
    {
        // ID of the student
        public int Id { get; set; }
        public int? SchoolId { get; set; } // Foreign Key
        public School? School { get; set; } // Navigation property

        // Name of the student
        // Group the student belongs to

        public required string FirstName { get; set; }
        public required string LastName { get; set;  }
        public string Group { get; set; } = "Unassigned";

        public string FullName => $"{FirstName} {LastName}";
    }
}
