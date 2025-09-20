namespace PathFinder.Application.Responses
{
    public class OkResponse<TData>(TData data, string message = "Success") 
        : ApiBaseResponse(true, 200)
    {
        public TData Data { get; set; } = data;
        public string Message { get; set; } = message;
    }
}
