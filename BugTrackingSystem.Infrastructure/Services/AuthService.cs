using BugTrackingSystem.Application.DTOs;
using BugTrackingSystem.Application.IServices;
using BugTrackingSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BugTrackingSystem.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._configuration = configuration;
        }

        public async Task<Response> LoginAsync(LoginDTO loginDTO)
        {
            Response response = new Response();
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                response.StatusCode = 400;
                response.Msg = "Invalid credentials provided";

                return response;
            }

            response.StatusCode = 200;
            response.Msg = await GenerateJwtToken(user);
            return response;
        }

        public async Task<Response> RegisterAsync(RegisterDTO registerDTO)
        {
            Response response = new Response();
            var userExists = await _userManager.FindByEmailAsync(registerDTO.Email);

            if (userExists != null)
            {
                response.StatusCode = 400;
                response.Msg = "User already exists";
                return response;
            }

            var user = new ApplicationUser { UserName = registerDTO.Email, Email = registerDTO.Email, FullName = registerDTO.FullName };
            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
            {
                response.StatusCode = 400;
                response.Msg = string.Join(",", result.Errors.Select(e => e.Description));
                return response;
            }

            //role addition
            if (!await _roleManager.RoleExistsAsync(registerDTO.Role))
            {
                await _roleManager.CreateAsync(new IdentityRole(registerDTO.Role));
            }

            await _userManager.AddToRoleAsync(user, registerDTO.Role);

            response.StatusCode = 200;
            response.Msg = await GenerateJwtToken(user);
            return response;
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key1 = _configuration["JwtSettings:secret"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:secret"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(7),
                    signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
