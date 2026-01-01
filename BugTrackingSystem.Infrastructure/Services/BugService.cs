using BugTrackingSystem.Application.DTOs;
using BugTrackingSystem.Application.IServices;
using BugTrackingSystem.Application.Mappers;
using BugTrackingSystem.Domain.Entities;
using BugTrackingSystem.Domain.Enums;
using BugTrackingSystem.Infrastructure.Identity;
using BugTrackingSystem.Infrastructure.Statics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BugTrackingSystem.Infrastructure.Services
{
    public class BugService : IBugService
    {
        private readonly BugDbContext _context;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public BugService(BugDbContext context, IFileService fileService, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _fileService = fileService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<Response> CreateBug(BugDTOs bugDTO)
        {
            Response bugResponse = new Response();
            var bug = new BugMapper().ConvertToEntity(bugDTO);
            bug.UserId = new GetUserId().GetId(_httpContextAccessor);
            bug.Status = BugStatus.Open;
            bug.IsAssigned = false;

            var fileResponse = await _fileService.SaveFile(bugDTO.Attachments, bug.BugId);


            if (!fileResponse.Success)
            {
                bugResponse.StatusCode = 400;
                bugResponse.Msg = fileResponse.Msg;

                return bugResponse;
            }

            //adding bugs
            _context.Bugs.Add(bug);
            await _context.SaveChangesAsync();

            //adding attachments
            foreach (var attachment in fileResponse.BugAttachment)
            {
                attachment.BugId = bug.BugId;
                _context.BugAttachments.Add(attachment);
            }
            await _context.SaveChangesAsync();


            bugResponse.StatusCode = 200;
            bugResponse.Msg = "Bug inserted successfully";

            return bugResponse;
        }

        public async Task<List<BugDTOs>> ViewBugs()
        {
            var userId = new GetUserId().GetId(_httpContextAccessor);
            var bugs = await _context.Bugs
                .Include(ba => ba.Attachments)
                .Where(b => b.UserId == userId)
                .ToListAsync();

            return GetBugDTOs(bugs);
        }


        public async Task<Response> ChangeBugStatus(int bugId, BugStatus status)
        {
            Response response = new Response();

            var bug = await _context.Bugs.FirstOrDefaultAsync(b => b.BugId == bugId);

            if (bug == null)
            {
                response.StatusCode = 404;
                response.Msg = "Bug Not Found";
            }
            bug.Status = status;

            _context.Bugs.Update(bug);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                response.StatusCode = 200;
                response.Msg = "Bug updated successfully";
            }
            else
            {
                response.StatusCode = 400;
                response.Msg = "Bad request";
            }
            return response;
        }

        public async Task<List<BugDTOs>> SearchBugs(string title)
        {
            var bugs = await _context.Bugs.Where(b => b.Title.Contains(title)).ToListAsync();
            return GetBugDTOs(bugs);
        }

        public async Task<List<BugDTOs>> ListUnAssignedBugs()
        {
            var userId = new GetUserId().GetId(_httpContextAccessor);
            var bugs = await _context.Bugs.Where(b => !b.IsAssigned).ToListAsync();
            return GetBugDTOs(bugs);
        }

        public List<BugDTOs> GetBugDTOs(List<Bug> bugs)
        {
            List<BugDTOs> bugDTOs = new List<BugDTOs>();

            foreach (var bug in bugs)
            {
                bugDTOs.Add(new BugMapper().ConvertToDTO(bug));
            }

            return bugDTOs;
        }

        public async Task<List<BugDTOs>> ListDevelopersAssignedBugs()
        {
            var userId = new GetUserId().GetId(_httpContextAccessor);
            var bugs = await _context.Bugs.Where(b => b.BugAssignment.DeveloperId == userId).ToListAsync();

            return GetBugDTOs(bugs);
        }

        public async Task<BugDTOs> GetSingleBug(int id)
        {
            var bug = await _context.Bugs
                .Include(a => a.Attachments)
                .Include(ba => ba.BugAssignment)
                .FirstOrDefaultAsync(b => b.BugId == id);
            var bugDTO = new BugMapper().ConvertToDTO(bug);
            bugDTO.ImageFiles = new List<string>();

            if (bug.Attachments != null)
            {
                foreach (var bugs in bug.Attachments)
                {
                    bugDTO.ImageFiles.Add(bugs.FilePath);
                }
            }

            if (bug.BugAssignment != null)
            {
                var assignedDeveloper = await _userManager.FindByIdAsync(bug.BugAssignment.DeveloperId);
                bugDTO.Developer = assignedDeveloper.FullName;

            }
            return bugDTO;
        }
    }
}
