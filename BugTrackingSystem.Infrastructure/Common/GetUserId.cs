using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BugTrackingSystem.Infrastructure.Statics
{
    public class GetUserId
    {
        public string GetId(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext.User != null)
            {
                return httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            return null;
        }
    }
}
