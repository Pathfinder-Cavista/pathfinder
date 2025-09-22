using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using PathFinder.Application.DTOs;
using PathFinder.Application.Interfaces;
using PathFinder.Application.Settings;
using System.Net;

namespace PathFinder.Application.Features
{
    public class UploadService : IUploadService
    {
        private readonly Cloudinary _cloud;
        public UploadService(IOptions<CloudinarySettings> options)
        {
            var settings = options.Value ??
                throw new ArgumentNullException(nameof(options));

            _cloud = new Cloudinary(new Account
            {
                ApiKey = settings.ApiKey,
                ApiSecret = settings.Secret,
                Cloud = settings.CloudName
            });
        }

        public async Task<UploadResponse?> UploadImageAsync(string fileName, string originalFileName, Stream stream)
        {
            var imageUploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, stream),
                PublicId = $"images/{fileName}",
                UniqueFilename = false,
                UseFilename = false,
                Transformation = new Transformation()
            };

            var response = await _cloud.UploadAsync(imageUploadParams);
            return response.StatusCode == HttpStatusCode.OK ?
                new UploadResponse(response.PublicId, response.SecureUrl.ToString()) : null;
        }

        public async Task<UploadResponse?> UploadRawAsync(string fileName, string originalFileName, Stream stream)
        {
            var uploadParam = new RawUploadParams
            {
                File = new FileDescription(originalFileName, stream),
                PublicId = $"documents/{fileName}",
                UseFilename = false,
                UniqueFilename = false,
                AccessMode = "public"
            };

            var response = await _cloud.UploadAsync(uploadParam);
            return response != null && response.StatusCode == HttpStatusCode.OK ?
                new UploadResponse(response.PublicId, response.SecureUrl.ToString()) : null;
        }

        public async Task<bool> DeleteAsync(string publicId, ResourceType type)
        {
            if (string.IsNullOrWhiteSpace(publicId))
            {
                return false;
            }

            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = type
            };

            var deletionResult = await _cloud.DestroyAsync(deleteParams);
            return deletionResult.StatusCode == HttpStatusCode.OK && deletionResult.Result.ToLower() == "ok";
        }
    }
}
