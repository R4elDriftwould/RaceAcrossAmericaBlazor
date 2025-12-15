using RaceAcrossAmerica.Data;
using Microsoft.EntityFrameworkCore;

namespace RaceAcrossAmerica.Services
{
    public class RaceManagerService
    {
        // Task 1
        // private variable to hold the database context
        // constructor to initialize the database context
        //      [Methods used for Home Page]
        // method to get all races from the database
        // method to Create a new race to the database

        // Task 2
        // method to get all students in student table
        // Add Student to Race
        // Add a Lap to RaceParticipant

        //Task 3
        // method to get race by id
        //  Use method in RaceDetails page for pulling race details

        // Task 4
        // method to Add Checkpoint to race
        // method to Delete Checkpoint from race
        //  Use methods in RaceDetails page for managing checkpoints
        //  methods do not care about RaceId directly, Checkpoints are linked to their own table

        // Task 5
        // method to Create a new student to the database
        // method to Delete a student from the database

        // Task 6 was in the RaceDetails page, implementing the AddStudent to race

        // Task 7
        // Implementing the already created AddLap, used in RaceTracker page
        // method to Remove lap

        private readonly ApplicationDbContext _dbContext;

        // Constructor to initialize the database context
        public RaceManagerService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Get All Races
        public async Task<List<Race>> GetAllRacesAsync()
        {
            // Retrieve Race table from the database
            return await _dbContext.Races
                .Include(r => r.Checkpoints)
                .ToListAsync();
        }

        // Create a Race and add it to the database
        public async Task CreateRaceAsync(Race newRace)
        {
            // Condition Check: Don't allow empty Title for Race
            if (string.IsNullOrWhiteSpace(newRace.Title))
            {
                throw new ArgumentException("Race Title cannot be empty.");
            }

            // using the database context to add the new race
            _dbContext.Add(newRace);
            await _dbContext.SaveChangesAsync();
        }

        // Get list of students
        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _dbContext.Students.ToListAsync();
        }

        // Add Student to Race
        public async Task AddStudentToRaceAsync(int raceId, int studentId)
        {
            /*
             RaceParticipant foundParticipant = null;

            foreach (RaceParticipant rp in _dbContext.RaceParticipants)
            {
                if (rp.RaceId == raceId && rp.StudentId == studentId)
                {
                    foundParticipant = rp;
                    break; // Stop looking, we found it!
                }
            }
            return foundParticipant;
            */

            // Check if Student already exists in race
            bool alreadyInRace = await _dbContext.RaceParticipants
                .AnyAsync(rp => rp.RaceId == raceId && rp.StudentId == studentId);

            if (alreadyInRace)
            {
                return;
            }

            var newParticipant = new RaceParticipant
            {
                RaceId = raceId,
                StudentId = studentId,
                LapsCompleted = 0
            };

            _dbContext.RaceParticipants.Add(newParticipant);
            await _dbContext.SaveChangesAsync();
        }

        // Add a Lap to RaceParticipant
        public async Task AddLapAsync(int raceId, int studentId)
        {
            // Find the RaceParticipant entry
            var participant = await _dbContext.RaceParticipants
                .FirstOrDefaultAsync(rp => rp.RaceId == raceId && rp.StudentId == studentId);

            if(participant != null) 
            {
                participant.LapsCompleted++;
                await _dbContext.SaveChangesAsync();
            }
        }

        // Delete a Lap from a RaceParticipant
        public async Task RemoveLapAsync(int raceId, int studentId)
        {
            // retrieve participant
            var participant = await _dbContext.RaceParticipants
                .FirstOrDefaultAsync(rp => rp.RaceId == raceId && rp.StudentId == studentId);

            // validate participant laps completed is greater than 0
            //      update database
            if(participant != null && participant.LapsCompleted > 0)
            {
                participant.LapsCompleted--;
                await _dbContext.SaveChangesAsync();
            }
        }

        // Get Race by ID
        // include the Checkpoints
        // include the RaceParticipants
        public async Task<Race?> GetRaceByIdAsync(int raceId)
        {

            return await _dbContext.Races
                .Include(r => r.Checkpoints)
                .Include(r => r.RaceParticipants)
                    .ThenInclude(rp => rp.Student)
                .FirstOrDefaultAsync(r => r.Id == raceId);


        }

        // Add Checkpoint to Checkpoints table
        // requires a checkpoint object
        public async Task AddCheckpointAsync(Checkpoint newCheckpoint)
        {
            if (string.IsNullOrEmpty(newCheckpoint.Name))
            {
                throw new ArgumentException("Checkpoint Name cannot be blank.");
            }

            _dbContext.Checkpoints.Add(newCheckpoint);
            await _dbContext.SaveChangesAsync();
        }

        // Delete a Checkpoint from Checkpoint Table
        public async Task DeleteCheckpointAsync(int checkpointId)
        {
            var checkpoint = await _dbContext.Checkpoints.FindAsync(checkpointId);
            if(checkpoint != null)
            {
                _dbContext.Checkpoints.Remove(checkpoint);
                await _dbContext.SaveChangesAsync();
            }
        }

        // Create a Student to Student List
        public async Task CreateStudentAsync(Student newStudent)
        {
            // Validation check
            if(string.IsNullOrEmpty(newStudent.FirstName) || string.IsNullOrEmpty(newStudent.FullName))
            {
                throw new ArgumentException("Student name is required");
            }
            // add to local dbContext
            _dbContext.Students.Add(newStudent);

            // SaveChangeAsync
            await _dbContext.SaveChangesAsync();
        }

        // Delete a student from the Student list
        public async Task DeleteStudentAsync(int studentId)
        {
            var student = await _dbContext.Students.FindAsync(studentId);
            if(student != null)
            {
                _dbContext.Students.Remove(student);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
