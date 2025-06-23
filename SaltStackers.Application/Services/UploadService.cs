using SaltStackers.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace SaltStackers.Application.Services
{
    public class UploadService : IUploadService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UploadService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string CreatePath(string path, string folder)
        {
            return Path.Combine(_webHostEnvironment.WebRootPath, "uploads", path, folder);
        }

        public List<string> GetFilesFromDirectory(string path)
        {
            var result = new List<string>();
            if (Directory.Exists(path))
            {
                var filePaths = Directory.GetFiles(path);

                foreach (string filePath in filePaths)
                {
                    result.Add(Path.GetFileName(filePath));
                }
            }
                
            return result;
        }

        public List<string> GetFilesFromDirectory(string path, string folder)
        {
            var filePath = CreatePath(path, folder);
            return GetFilesFromDirectory(filePath);
        }

        public void UploadedFiles(List<IFormFile> files, string path, string folder)
        {
            if (files != null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", path, folder);
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var file in files)
                {
                    if (file != null)
                    {
                        var filePath = Path.Combine(uploadsFolder, file.FileName);
                        using var fileStream = new FileStream(filePath, FileMode.Create);
                        file.CopyTo(fileStream);
                    }
                }
            }
        }

        public bool UploadedFile(IFormFile file, string path, string folder, string? fileName = "")
        {
            if (file != null)
            {
                try
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", path, folder);
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    if (string.IsNullOrEmpty(fileName))
                    {
                        fileName = file.FileName;
                    }
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(fileStream);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }
    }
}
