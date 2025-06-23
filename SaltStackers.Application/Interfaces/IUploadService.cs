using Microsoft.AspNetCore.Http;

namespace SaltStackers.Application.Interfaces
{
    public interface IUploadService
    {
        string CreatePath(string path, string folder);

        List<string> GetFilesFromDirectory(string path);

        List<string> GetFilesFromDirectory(string path, string folder);

        void UploadedFiles(List<IFormFile> files, string path, string folder);

        bool UploadedFile(IFormFile file, string path, string folder, string? fileName = "");
    }
}
