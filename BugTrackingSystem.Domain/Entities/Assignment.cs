namespace BugTrackingSystem.Domain.Entities
{
    public class Assignment
    {
        public int AssignmentId { get; set; }
        public string UserId { get; set; } = null!;
        public int BugId { get; set; }
        public string DeveloperId { get; set; } = null!;
    }
}
