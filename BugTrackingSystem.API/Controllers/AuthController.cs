using BugTrackingSystem.Application.DTOs;
using BugTrackingSystem.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterDTO registerDTO)
        {
            var response = await _authService.RegisterAsync(registerDTO);
            if (response.StatusCode == 201)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginDTO loginDTO)
        {
            var response = await _authService.LoginAsync(loginDTO);
            if (response.StatusCode == 201)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
