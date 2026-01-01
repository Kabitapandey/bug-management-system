using BugTrackingSystem.Application.DTOs;
using BugTrackingSystem.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugAssignmentController : ControllerBase
    {
        private readonly IBugAssignmentService _bugAssignmentService;

        public BugAssignmentController(IBugAssignmentService bugAssignmentService)
        {
            this._bugAssignmentService = bugAssignmentService;
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AssignBugs(AssignmentDTO assignmentDTO)
        {
            var response = await _bugAssignmentService.AssignBugs(assignmentDTO);

            if (response.StatusCode == 400)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("self")]
        [Authorize(Roles = "Developer")]
        public async Task<IActionResult> AssignToSelf(AssignmentDTO assignmentDTO)
        {
            var response = await _bugAssignmentService.AssignToSelf(assignmentDTO);
            if (response.StatusCode == 400)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
