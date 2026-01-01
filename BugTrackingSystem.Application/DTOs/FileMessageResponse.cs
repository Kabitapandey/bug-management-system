using BugTrackingSystem.Domain.Entities;

namespace BugTrackingSystem.Application.DTOs
{
    public class FileMessageResponse
    {
        public bool Success { get; set; }
        public string? Msg { get; set; }
        public List<BugAttachment> BugAttachment { get; set; } = new List<BugAttachment>();
    }
}
