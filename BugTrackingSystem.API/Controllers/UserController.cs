using BugTrackingSystem.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> GetDevelopers()
        {
            var response = await _userService.GetDevelopers();
            return Ok(response);
        }
    }
}
