namespace PathFinder.Application.Responses
{
    public class BadRequestResponse(string message) 
        : ApiBaseResponse(false, 400)
    {
        public string Message { get; set; } = message;
    }
}
