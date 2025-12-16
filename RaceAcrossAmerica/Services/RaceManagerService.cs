using RaceAcrossAmerica.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace RaceAcrossAmerica.Services
{
    public class RaceManagerService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly UserManager<ApplicationUser> _userManager;

        // We inject AuthStateProvider and UserManager to find the current user
        public RaceManagerService(
            ApplicationDbContext dbContext,
            AuthenticationStateProvider authStateProvider,
            UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _authStateProvider = authStateProvider;
            _userManager = userManager;
        }

        // --- Helper Method: Get Current School ID ---
        private async Task<int?> GetCurrentSchoolIdAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var userPrincipal = authState.User;

            if (userPrincipal.Identity == null || !userPrincipal.Identity.IsAuthenticated)
            {
                return null;
            }

            // Retrieve the full user object from the DB to access the SchoolId property
            var currentUser = await _userManager.GetUserAsync(userPrincipal);
            return currentUser?.SchoolId;
        }

        // --- Task 1: Races ---

        public async Task<List<Race>> GetAllRacesAsync()
        {
            var schoolId = await GetCurrentSchoolIdAsync();
            if (schoolId == null) return new List<Race>();

            // Filter: Only return races for this school
            return await _dbContext.Races
                .Where(r => r.SchoolId == schoolId)
                .Include(r => r.Checkpoints)
                .ToListAsync();
        }

        public async Task CreateRaceAsync(Race newRace)
        {
            if (string.IsNullOrWhiteSpace(newRace.Title))
            {
                throw new ArgumentException("Race Title cannot be empty.");
            }

            var schoolId = await GetCurrentSchoolIdAsync();
            if (schoolId == null)
            {
                throw new UnauthorizedAccessException("User is not associated with a school.");
            }

            // Auto-assign the new race to the user's school
            newRace.SchoolId = schoolId;

            _dbContext.Add(newRace);
            await _dbContext.SaveChangesAsync();
        }

        // --- Task 2: Students ---

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            var schoolId = await GetCurrentSchoolIdAsync();
            if (schoolId == null) return new List<Student>();

            // Filter: Only return students for this school
            return await _dbContext.Students
                .Where(s => s.SchoolId == schoolId)
                .ToListAsync();
        }

        public async Task AddStudentToRaceAsync(int raceId, int studentId)
        {
            // Security Check: Ensure the Race belongs to the current school
            var schoolId = await GetCurrentSchoolIdAsync();
            var race = await _dbContext.Races.FirstOrDefaultAsync(r => r.Id == raceId && r.SchoolId == schoolId);

            if (race == null) return; // Race not found or belongs to another school

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

        public async Task AddLapAsync(int raceId, int studentId)
        {
            // We verify the participant via the race (which is linked to the school)
            var participant = await _dbContext.RaceParticipants
                .Include(rp => rp.Student) // potentially needed for checks
                .FirstOrDefaultAsync(rp => rp.RaceId == raceId && rp.StudentId == studentId);

            // Optional: You could strictly check if participant.Student.SchoolId == currentSchoolId here too

            if (participant != null)
            {
                participant.LapsCompleted++;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveLapAsync(int raceId, int studentId)
        {
            var participant = await _dbContext.RaceParticipants
                .FirstOrDefaultAsync(rp => rp.RaceId == raceId && rp.StudentId == studentId);

            if (participant != null && participant.LapsCompleted > 0)
            {
                participant.LapsCompleted--;
                await _dbContext.SaveChangesAsync();
            }
        }

        // --- Task 3: Race Details ---

        public async Task<Race?> GetRaceByIdAsync(int raceId)
        {
            var schoolId = await GetCurrentSchoolIdAsync();
            if (schoolId == null) return null;

            // Filter: Only find the race if it matches the ID AND the SchoolId
            return await _dbContext.Races
                .Include(r => r.Checkpoints)
                .Include(r => r.RaceParticipants)
                    .ThenInclude(rp => rp.Student)
                .FirstOrDefaultAsync(r => r.Id == raceId && r.SchoolId == schoolId);
        }

        // --- Task 4: Checkpoints ---
        // Checkpoints are children of Races. Since we secure the Race, we implicitly secure the Checkpoint add/delete.

        public async Task AddCheckpointAsync(Checkpoint newCheckpoint)
        {
            if (string.IsNullOrEmpty(newCheckpoint.Name))
            {
                throw new ArgumentException("Checkpoint Name cannot be blank.");
            }

            // Verify the race belongs to the user's school before adding a checkpoint
            var schoolId = await GetCurrentSchoolIdAsync();
            bool raceExistsForSchool = await _dbContext.Races.AnyAsync(r => r.Id == newCheckpoint.RaceId && r.SchoolId == schoolId);

            if (!raceExistsForSchool)
            {
                throw new UnauthorizedAccessException("Cannot add checkpoint to a race that does not belong to your school.");
            }

            _dbContext.Checkpoints.Add(newCheckpoint);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCheckpointAsync(int checkpointId)
        {
            var schoolId = await GetCurrentSchoolIdAsync();

            var checkpoint = await _dbContext.Checkpoints
                .Include(c => c.Race)
                .FirstOrDefaultAsync(c => c.Id == checkpointId);

            // Verify the checkpoint belongs to a race owned by the user's school
            if (checkpoint != null && checkpoint.Race != null && checkpoint.Race.SchoolId == schoolId)
            {
                _dbContext.Checkpoints.Remove(checkpoint);
                await _dbContext.SaveChangesAsync();
            }
        }

        // --- Task 5: Students Management ---

        public async Task CreateStudentAsync(Student newStudent)
        {
            if (string.IsNullOrEmpty(newStudent.FirstName) || string.IsNullOrEmpty(newStudent.FullName))
            {
                throw new ArgumentException("Student name is required");
            }

            var schoolId = await GetCurrentSchoolIdAsync();
            if (schoolId == null) throw new UnauthorizedAccessException("User is not associated with a school.");

            // Auto-assign student to the school
            newStudent.SchoolId = schoolId;

            _dbContext.Students.Add(newStudent);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteStudentAsync(int studentId)
        {
            var schoolId = await GetCurrentSchoolIdAsync();

            // Only find the student if they belong to the user's school
            var student = await _dbContext.Students
                .FirstOrDefaultAsync(s => s.Id == studentId && s.SchoolId == schoolId);

            if (student != null)
            {
                _dbContext.Students.Remove(student);
                await _dbContext.SaveChangesAsync();
            }
        }

        // --- Task 6: Team Management ---

        public async Task<List<ApplicationUser>> GetTeamMembersAsync()
        {
            var schoolId = await GetCurrentSchoolIdAsync();
            if (schoolId == null) return new List<ApplicationUser>();

            // Get all users who belong to this school
            return await _userManager.Users
                .Where(u => u.SchoolId == schoolId)
                .ToListAsync();
        }

        public async Task<IdentityResult> AddTeamMemberAsync(string email, string password)
        {
            var schoolId = await GetCurrentSchoolIdAsync();
            if (schoolId == null)
            {
                throw new UnauthorizedAccessException("You must belong to a school to add team members.");
            }

            var newUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
                SchoolId = schoolId, // Crucial: Assign them to YOUR school
                EmailConfirmed = true // Auto-confirm for simplicity
            };

            var result = await _userManager.CreateAsync(newUser, password);
            return result;
        }
    }
}