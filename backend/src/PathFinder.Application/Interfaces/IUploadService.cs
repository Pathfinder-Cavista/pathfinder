using CloudinaryDotNet.Actions;
using PathFinder.Application.DTOs;

namespace PathFinder.Application.Interfaces
{
    public interface IUploadService
    {
        Task<bool> DeleteAsync(string publicId, ResourceType type);
        Task<UploadResponse?> UploadImageAsync(string fileName, string originalFileName, Stream stream);
        Task<UploadResponse?> UploadRawAsync(string fileName, string originalFileName, Stream stream);
    }
}
