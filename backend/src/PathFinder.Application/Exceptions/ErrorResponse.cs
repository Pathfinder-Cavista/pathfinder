namespace PathFinder.Application.Exceptions
{
    public class ErrorResponse
    {
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Success => false;
    }
}
