using BugTrackingSystem.Domain.Entities;
using BugTrackingSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BugTrackingSystem.Infrastructure
{
    public class BugDbContext : IdentityDbContext<ApplicationUser>
    {
        public BugDbContext(DbContextOptions<BugDbContext> options) : base(options) { }
        public DbSet<Bug> Bugs { get; set; }
        public DbSet<BugAttachment> BugAttachments { get; set; }
        public DbSet<Assignment> BugAssignments { get; set; }
    }
}
