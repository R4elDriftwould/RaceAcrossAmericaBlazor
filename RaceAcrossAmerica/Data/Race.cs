namespace RaceAcrossAmerica.Data
{
    public class Race
    {
        // ID of the Race
        // School Id
        // School Object
        // Title for Race
        // Total Distance for Race
        // IsActive flag
        // List of Checkpoints associated

        public int Id { get; set; }

        public int? SchoolId { get; set; } // Foreign Key
        public School? School { get; set; } // Navigation property

        public required string Title { get; set; }
        public int TotalDistanceMiles { get; set; }
        public bool IsActive { get; set; }
        public List<Checkpoint> Checkpoints { get; set; } = new();
        public List<RaceParticipant> RaceParticipants { get; set; } = new();

    }
}
