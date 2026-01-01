using BugTrackingSystem.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace BugTrackingSystem.Application.IServices
{
    public interface IFileService
    {
        Task<FileMessageResponse> SaveFile(List<IFormFile> file, int bugId);
    }
}
