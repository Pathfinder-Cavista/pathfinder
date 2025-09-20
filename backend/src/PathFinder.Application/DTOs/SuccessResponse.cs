namespace PathFinder.Application.DTOs
{
    public record SuccessResponse
    {
        public int Status => 200;
        public string Message { get; set; } = string.Empty;
        public bool Success => true;

        public SuccessResponse(string message)
        {
            Message = message;
        }
    }
}
