using BugTrackingSystem.Domain.Enums;

namespace BugTrackingSystem.Domain.Entities
{
    public class Bug
    {
        public int BugId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public BugSeverity Severity { get; set; }
        public string ReproductionSteps { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<BugAttachment> Attachments { get; set; } = new List<BugAttachment>();
        public string UserId { get; set; } = null!;
        public BugStatus Status { get; set; }
        public bool IsAssigned { get; set; }
        public Assignment? BugAssignment { get; set; }
    }
}
