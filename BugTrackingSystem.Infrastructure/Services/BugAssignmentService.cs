using BugTrackingSystem.Application.DTOs;
using BugTrackingSystem.Application.IServices;
using BugTrackingSystem.Domain.Entities;
using BugTrackingSystem.Infrastructure.Statics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BugTrackingSystem.Infrastructure.Services
{
    public class BugAssignmentService : IBugAssignmentService
    {
        private readonly BugDbContext _context;
        public IHttpContextAccessor httpContextAccessor;

        public BugAssignmentService(BugDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response> AssignBugs(AssignmentDTO assignmentDTO)
        {
            Response response = new Response();
            Assignment assignment = new Assignment();

            assignment.DeveloperId = assignmentDTO.DeveloperId;
            assignment.BugId = assignmentDTO.bugId;
            assignment.UserId = new GetUserId().GetId(httpContextAccessor);

            var isAssigned = await _context.BugAssignments.FirstOrDefaultAsync(ba => ba.BugId == assignmentDTO.bugId);

            if (isAssigned != null)
            {
                response.StatusCode = 400;
                response.Msg = "This bug is already assigned to a developer";
                return response;
            }

            _context.Add(assignment);
            var result = await _context.SaveChangesAsync();
            if (result <= 0)
            {
                response.StatusCode = 400;
                response.Msg = "Could not assign a bug; Please try again later";
                return response;
            }

            await _context.SaveChangesAsync();

            var bug = await _context.Bugs.FirstOrDefaultAsync(b => b.BugId == assignmentDTO.bugId);
            bug.IsAssigned = true;

            _context.Bugs.Update(bug);
            await _context.SaveChangesAsync();

            response.StatusCode = 200;
            response.Msg = "Bug assignment successful";
            return response;
        }

        public async Task<Response> AssignToSelf(AssignmentDTO assignmentDTO)
        {
            Response response = new Response();
            var bug = await _context.Bugs.FirstOrDefaultAsync(b => b.BugId == assignmentDTO.bugId);

            Assignment assignment = new Assignment();
            assignment.UserId = bug.UserId;
            assignment.DeveloperId = new GetUserId().GetId(httpContextAccessor);
            assignment.BugId = assignmentDTO.bugId;


            _context.BugAssignments.Add(assignment);
            int result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                response.StatusCode = 200;
                response.Msg = "Bug assigned successfully";
            }
            else
            {
                response.StatusCode = 400;
                response.Msg = "Could not assign a bug";
            }

            bug.IsAssigned = true;
            _context.Bugs.Update(bug);
            await _context.SaveChangesAsync();
            return response;
        }
    }
}
