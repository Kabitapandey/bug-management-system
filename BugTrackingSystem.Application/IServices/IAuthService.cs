using BugTrackingSystem.Application.DTOs;

namespace BugTrackingSystem.Application.IServices
{
    public interface IAuthService
    {
        Task<Response> RegisterAsync(RegisterDTO registerDTO);
        Task<Response> LoginAsync(LoginDTO loginDTO);
    }
}
