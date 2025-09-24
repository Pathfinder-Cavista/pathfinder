using Hangfire.Server;
using Microsoft.AspNetCore.Http;
using PathFinder.Application.Responses;

namespace PathFinder.Application.Interfaces
{
    public interface IDataloadService
    {
        Task<ApiBaseResponse> RunDataloadAsync(IFormFile file);
    }
}