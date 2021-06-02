using System.Diagnostics;
using System.Net.Http;
using LinkFinder.Logic.Models;

namespace LinkFinder.Logic.Services
{
    public class RequestService
    {
        public virtual HttpResponseMessage SendRequest(string page, out int timeOfResponse)
        {
            using HttpClient client = new HttpClient();
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                client.DefaultRequestHeaders
                     .UserAgent
                     .Add(new System.Net.Http.Headers.ProductInfoHeaderValue("MyBot", "1.0"));

                var startTime = Stopwatch.StartNew();
                responseMessage = client.GetAsync(page).Result;
                startTime.Stop();
                timeOfResponse = (int)startTime.ElapsedMilliseconds;

                return responseMessage;
            }
            catch
            {
                timeOfResponse = 0;
                return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.RequestTimeout };
            }
        }

        public virtual string DownloadPage(Link link)
        {
            var responseMessage = SendRequest(link.Url, out int timeOfResponse);

            link.TimeResponse = timeOfResponse;

            if (responseMessage.IsSuccessStatusCode == false)
            {
                return "";
            }

            return responseMessage.Content.ReadAsStringAsync().Result;
        }
    }
}
