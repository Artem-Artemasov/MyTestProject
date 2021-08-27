namespace LinkFinder.WebApi.Logic.Response.Models
{
    public class ResponseObject
    {
        public bool IsSuccessful { get; set; } = true;
        public object Content { get; set; }
        public string Errors { get; set; }
    }
}
