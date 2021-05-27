using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UKAD.Enums;
using UKAD.Interfaces.Service;
using UKAD.Models;

namespace UKAD.Services
{
    public class RequestService : IRequestService
    {
        public string BaseUrl { get; private set; }
        private string Protocol { get; set; }
        private string UrlWithHost { get; set; }
        public RequestService(string baseUrl)
        {
            BaseUrl = baseUrl;
            Protocol = baseUrl.Substring(0, baseUrl.IndexOf("//") + 2);
            UrlWithHost = baseUrl[(baseUrl.IndexOf("//") + 2)..];
        }

        /// <summary>
        /// Send get request on page
        /// </summary>
        public async Task<HttpResponseMessage> SendRequestAsync(string page)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders
                         .UserAgent
                         .Add(new System.Net.Http.Headers.ProductInfoHeaderValue("MyBot", "1.0"));

                    responseMessage = await client.GetAsync(page);
                }
                catch
                {
                    Console.WriteLine(page + " do not avaliable");
                }
            }
            return responseMessage;
        }

        /// <summary>
        /// Parse Http message to Link objects
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Link>> ToLinkList(HttpResponseMessage responseMessage,Link link)
        {
            List<Link> links = new List<Link>();

            var defaultDelimiters = GetDefaultUrlDelimiters(link.LocationUrl);

            if (responseMessage.IsSuccessStatusCode)
            {
                string message = await responseMessage.Content.ReadAsStringAsync();
                var urls = CutAllUrls(message, link.LocationUrl,defaultDelimiters).ToList();
                links = ToAbsoluteUrlList(urls, link.Url, link.LocationUrl).ToList();
            }

            return links;
        }

        /// <summary>
        /// Find all links, 
        /// If we finding in view, it looked on 
        /// If we finding in sitemap it looked on <loc> tag
        /// </summary>
        public IEnumerable<string> CutAllUrls(string message, LocationUrl location,in Dictionary<string,string> urlDelimiters)
        {
            //Url start after "key",Url end before "value"
            List<string> urls = new List<string>();

            foreach (var key in urlDelimiters.Keys)
            {
                int currentPos = 0;
                int indexOfKey = 0;
                while ((currentPos < message.Length) && (indexOfKey != -1))
                {
                    indexOfKey = message.IndexOf(key, currentPos);
                    if (indexOfKey != -1)
                    {
                        var value = urlDelimiters.GetValueOrDefault(key);
                        int indexOfVal = message.IndexOf(value, indexOfKey + key.Length);
                        if (indexOfVal == -1)
                            indexOfVal = message.Length;

                        urls.Add(message[(indexOfKey + key.Length)..indexOfVal]);
                        currentPos = indexOfVal + value.Length;
                    }
                }
            }

            return urls;
        }

        /// <summary>
        /// Default delimiters is 
        /// <loc></loc> for sitemap.xml
        /// href src for view
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public Dictionary<string,string> GetDefaultUrlDelimiters(LocationUrl location)
        {
            Dictionary<string, string> defaultDelimiters = new Dictionary<string, string>();

            if (location == LocationUrl.InSiteMap)
            {
                defaultDelimiters.Add("<loc>", "</loc>");
            }
            else
            {
                defaultDelimiters.Add("href=\"", "\"");
                defaultDelimiters.Add("href='", "'");
                defaultDelimiters.Add("src=\"", "\"");
                defaultDelimiters.Add("src='", "'");
            }
            return defaultDelimiters;
        }
       
        public IEnumerable<Link> ToAbsoluteUrlList(IEnumerable<string> input, string relativelyFrom, LocationUrl location)
        {
            List<Link> ablosuteUrl = new List<Link>();

            foreach (string item in input)
            {
                //absolute path
                if (item.StartsWith("http://") || item.StartsWith("https://"))
                {
                    ablosuteUrl.Add(new Link(item, location));
                    continue;
                }
                //example: item == //example.com/ => add https://www.example.com/
                if (item.StartsWith("//") && !item.Contains(Protocol))
                {
                    ablosuteUrl.Add(new Link(Protocol + item[2..], location));
                    continue;
                }
                
                if (item == "/")
                {
                    ablosuteUrl.Add(new Link(BaseUrl, location));
                    continue;
                }
                //example revativelyFrom == https://www.example.com/something and item == something/text.html => https://www.example.com/something/text.html 
                if (relativelyFrom.EndsWith(item))
                {
                    var absolute = relativelyFrom.Substring(0, relativelyFrom.IndexOf(item)) + item;
                    ablosuteUrl.Add(new Link(absolute, location));
                    continue;
                }
                //example item == /books => add BaseUrl/books
                if (item.StartsWith("/"))
                {
                    ablosuteUrl.Add(new Link(BaseUrl + item, location));
                    continue;
                }
                //example item == ./books/firstbook.html and relativelyFrom == https://www.example.com/library/magazine => https://www.example.com/library/books/firstbook.html
                if (item.StartsWith("./"))
                {
                    string absolute = relativelyFrom;
                    string relative = item;
                    while (relative.StartsWith("./"))
                    {
                        absolute = absolute.Substring(0, relativelyFrom.LastIndexOf("/"));
                        relative = relative[2..];
                    }
                    ablosuteUrl.Add(new Link(absolute + "/" + relative, location));
                    continue;
                }

            }
            return ablosuteUrl;
        }
    }
}
