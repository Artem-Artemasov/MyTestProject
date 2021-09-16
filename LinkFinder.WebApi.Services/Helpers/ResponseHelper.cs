using LinkFinder.WebApi.Logic.Response.Models;

namespace LinkFinder.WebApi.Logic.Helpers
{
    public static class ResponseHelper
    {
        public static ResponseObject CreateResponseObj(object obj)
        {
            return new ResponseObject()
            {
                IsSuccessful = true,
                Content = obj,
            };
        }
    }
}
