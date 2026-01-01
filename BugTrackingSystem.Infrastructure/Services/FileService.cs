using BugTrackingSystem.Application.DTOs;
using BugTrackingSystem.Application.IServices;
using BugTrackingSystem.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BugTrackingSystem.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly string _webRootPath;

        public FileService(string webRootPath)
        {
            this._webRootPath = webRootPath;
        }
        public async Task<FileMessageResponse> SaveFile(List<IFormFile> file, int bugId)
        {
            FileMessageResponse fileMsgResponse = new FileMessageResponse();
            fileMsgResponse.Success = true;

            //handling uploding attachments
            if (file != null && file.Any())
            {
                var uploadPath = Path.Combine(_webRootPath, "uploads", "bugs", bugId.ToString());

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".txt", ".log" };
                List<string> attachments = new List<string>();
                fileMsgResponse.BugAttachment = new List<BugAttachment>();

                foreach (var fileAttachment in file)
                {
                    var extension = Path.GetExtension(fileAttachment.FileName).ToLower();
                    if (!allowedExtensions.Contains(extension))
                    {
                        fileMsgResponse.Success = false;
                        fileMsgResponse.Msg = "Invalid file type; Only jpg, jpeg, png, txt and log type are accepted";
                    }

                    else if (fileAttachment.Length > 5 * 1024 * 1024)
                    {
                        fileMsgResponse.Success = false;
                        fileMsgResponse.Msg = "File must be less than 5MB";
                    }

                    else
                    {
                        var attachmentName = $"{Guid.NewGuid()}{extension}";
                        var fullPath = Path.Combine(uploadPath, attachmentName);

                        using var stream = new FileStream(fullPath, FileMode.Create);
                        await fileAttachment.CopyToAsync(stream);

                        fileMsgResponse.BugAttachment.Add(new BugAttachment
                        {
                            FileName = fileAttachment.FileName,
                            FilePath = $"uploads/bugs/{bugId}/{attachmentName}",
                            ContentType = fileAttachment.ContentType,
                            FileSize = fileAttachment.Length
                        });
                    }
                }
            }

            return fileMsgResponse;
        }

    }
}
