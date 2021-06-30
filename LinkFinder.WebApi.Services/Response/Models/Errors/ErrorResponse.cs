namespace LinkFinder.WebApi.Logic.Response.Models.Errors
{
    public class ErrorResponse
    {
        public string Name { get; set; } = "Error";
        public string ErrorMessage { get; set; } = "";

        public ErrorResponse(string name, string error)
        {
            Name = name;
            ErrorMessage = error;
        }
    }
}
