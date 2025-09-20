namespace PathFinder.Application.Responses
{
    public class ForbiddenResponse(string message) : ApiBaseResponse(false, 403)
    {
        public string Message { get; set; } = message;
    }
}
