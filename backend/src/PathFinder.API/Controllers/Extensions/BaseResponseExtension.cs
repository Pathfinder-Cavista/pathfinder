using PathFinder.Application.Responses;

namespace PathFinder.API.Controllers.Extensions
{
    public static class BaseResponseExtension
    {
        public static OkResponse<TData> GetResult<TData>(this ApiBaseResponse response)
        {
            return (OkResponse<TData>)response;
        }
    }
}
