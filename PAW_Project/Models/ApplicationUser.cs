using Microsoft.AspNetCore.Identity;

namespace PAW_Project.Models
{
    public class ApplicationUser : IdentityUser
    {
        // This adds the profile picture column to the database
        public byte[]? ProfilePicture { get; set; }
    }
}