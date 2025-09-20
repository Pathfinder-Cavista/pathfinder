namespace PathFinder.Application.Responses
{
    public class NotFoundResponse(string message) 
        : ApiBaseResponse(false, 404)
    {
        public string Message { get; set; } = message;
    }
}