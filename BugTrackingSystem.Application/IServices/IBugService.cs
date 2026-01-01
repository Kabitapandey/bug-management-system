using BugTrackingSystem.Application.DTOs;
using BugTrackingSystem.Domain.Enums;

namespace BugTrackingSystem.Application.IServices
{
    public interface IBugService
    {
        Task<Response> CreateBug(BugDTOs bugDTO);
        Task<List<BugDTOs>> ViewBugs();
        Task<Response> ChangeBugStatus(int bugId, BugStatus status);
        Task<List<BugDTOs>> SearchBugs(string title);
        Task<List<BugDTOs>> ListUnAssignedBugs();
        Task<List<BugDTOs>> ListDevelopersAssignedBugs();
        Task<BugDTOs> GetSingleBug(int id);
    }
}
