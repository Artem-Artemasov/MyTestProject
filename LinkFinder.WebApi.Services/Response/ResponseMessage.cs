namespace LinkFinder.WebApi.Services.Response
{
    public class ResponseMessage
    {
        public bool IsSuccessful { get; set; } = true;
        public object Content { get; set; }
    }
}
