namespace PathFinder.Application.Responses
{
    public class ConflictResponse(string message) 
        : ApiBaseResponse(false, 409)
    {
        public string Message { get; set; } = message;
    }
}
