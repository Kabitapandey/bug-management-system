using BugTrackingSystem.Application.DTOs;
using BugTrackingSystem.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BugController : ControllerBase
    {
        private readonly IBugService _bugService;

        public BugController(IBugService bugService)
        {
            this._bugService = bugService;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> createBugs([FromForm] BugDTOs bugDTOs)
        {
            var response = await _bugService.CreateBug(bugDTOs);
            if (response.StatusCode == 400)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ViewBugs()
        {
            var response = await _bugService.ViewBugs();
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusDTO updateStatusDTO)
        {
            var response = await _bugService.ChangeBugStatus(id, updateStatusDTO.Status);

            if (response.StatusCode == 404)
            {
                return NotFound(response);
            }
            else if (response.StatusCode == 400)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchBugs([FromQuery] string title)
        {
            var response = await _bugService.SearchBugs(title);
            return Ok(response);
        }

        [HttpGet("unassigned")]
        public async Task<IActionResult> GetUnAssignedBygs()
        {
            var response = await _bugService.ListUnAssignedBugs();
            return Ok(response);
        }

        [HttpGet("developers")]
        [Authorize(Roles = "Developer")]
        public async Task<IActionResult> GetDevelopersAssignedBugs()
        {
            var response = await _bugService.ListDevelopersAssignedBugs();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleBug(int id)
        {
            var response = await _bugService.GetSingleBug(id);
            return Ok(response);
        }
    }
}
