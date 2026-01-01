using Microsoft.AspNetCore.Identity;

namespace BugTrackingSystem.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = null!;
    }
}
