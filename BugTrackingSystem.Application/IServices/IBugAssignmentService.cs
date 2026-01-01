using BugTrackingSystem.Application.DTOs;

namespace BugTrackingSystem.Application.IServices
{
    public interface IBugAssignmentService
    {
        Task<Response> AssignBugs(AssignmentDTO assignmentDTO);
        Task<Response> AssignToSelf(AssignmentDTO assignmentDTO);
    }
}
