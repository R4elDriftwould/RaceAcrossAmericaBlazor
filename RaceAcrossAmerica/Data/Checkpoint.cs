namespace RaceAcrossAmerica.Data
{
    public class Checkpoint
    {
        // ID of the checkpoint
        // Distance from start in miles
        // Name of the checkpoint
        // Description of the checkpoint
        // Map Position X
        // Map Position Y
        // RaceId the checkpoint belongs to

        public int Id { get; set; }
        public int DistanceFromStart { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public int MapPositionX { get; set; }
        public int MapPositionY { get; set; }
        public int RaceId { get; set; }
        public Race? Race { get; set; }
    }
}
