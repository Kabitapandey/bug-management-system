using BugTrackingSystem.Application.DTOs;
using BugTrackingSystem.Application.IServices;
using BugTrackingSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace BugTrackingSystem.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }
        public async Task<List<UserDTO>> GetDevelopers()
        {
            var users = await _userManager.GetUsersInRoleAsync("Developer");

            List<UserDTO> userDTO = new List<UserDTO>();
            foreach (var user in users)
            {
                userDTO.Add(new UserDTO
                {
                    FullName = user.FullName,
                    DeveloperID = user.Id
                });

            }

            return userDTO;
        }
    }
}
