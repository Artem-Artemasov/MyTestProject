using Microsoft.AspNetCore.Mvc;

namespace LinkFinder.WebApi.Logic.Request
{
    public class CreateTestParam
    {
        /// <summary>
        /// Link that will be crawled with program
        /// </summary>
        public string Url { get; set; }
    }
}
