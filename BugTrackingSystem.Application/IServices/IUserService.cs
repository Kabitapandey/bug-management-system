using BugTrackingSystem.Application.DTOs;

namespace BugTrackingSystem.Application.IServices
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetDevelopers();
    }
}
