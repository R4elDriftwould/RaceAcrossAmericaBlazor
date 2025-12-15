namespace RaceAcrossAmerica.Data
{
    public class RaceParticipant
    {
        // ID of the RaceParticipant
        // StudentId of the participant
        // Student Object of the participant
        // RaceId the participant is in
        // LapsCompleted by the participant

        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student? Student { get; set; }
        public int LapsCompleted { get; set; }
        public int RaceId { get; set; }
        public int CalculatedDistance(int milesPerLap)
        {
            return LapsCompleted * milesPerLap;
        }
    }
}
