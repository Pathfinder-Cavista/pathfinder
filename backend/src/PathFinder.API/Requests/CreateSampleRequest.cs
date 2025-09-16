namespace PathFinder.API.Requests
{
    public class CreateSampleRequest
    {
        public string Name { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
    }
}