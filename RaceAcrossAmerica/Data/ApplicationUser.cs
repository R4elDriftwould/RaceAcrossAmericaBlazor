using Microsoft.AspNetCore.Identity;

namespace RaceAcrossAmerica.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        // Linking user to a specific school
        public int? SchoolId { get; set; }
        public School? School { get; set; }
    }

}
