using BugTrackingSystem.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace BugTrackingSystem.Application.DTOs
{
    public class BugDTOs
    {
        public int BugId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public BugSeverity Severity { get; set; }
        public string ReproductionSteps { get; set; } = null!;
        public List<IFormFile>? Attachments { get; set; }
        public string? FilePath { get; set; }
        public BugStatus Status { get; set; }
        public bool IsAssigned { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? Developer { get; set; }
        public List<string>? ImageFiles { get; set; }
    }
}
