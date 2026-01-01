namespace BugTrackingSystem.Domain.Entities
{
    public class BugAttachment
    {
        public int BugAttachmentId { get; set; }
        public int BugId { get; set; }
        public Bug Bug { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public long FileSize { get; set; }
    }
}
