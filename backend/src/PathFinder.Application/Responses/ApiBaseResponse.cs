namespace PathFinder.Application.Responses
{
    public abstract class ApiBaseResponse
    {
        public bool Success { get; set; }
        public int Status { get; set; }

        protected ApiBaseResponse(bool success, int statusCode)
        {
            Success = success;
            Status = statusCode;
        }
    }
}
