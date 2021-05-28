using System.Linq;
using System.Net.Http;
using UKAD.Logic.Enums;
using UKAD.Logic.Models;
using System.Collections.Generic;

namespace UKAD.Logic.Services
{
    public class LinkService
    {
        public readonly string BaseUrl;

        public LinkService(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public IEnumerable<string> CutUrlBetweenTags(string message, in Dictionary<string, string> urlTags)
        {
            List<string> urls = new List<string>();
  
            foreach(var currentTags in urlTags) 
            {
                int currentPos = 0;
                while (currentPos < message.Length) //find urls in all message between currentTags
                {
                    int indexOfStartTag = message.IndexOf(currentTags.Key, currentPos);

                    if (indexOfStartTag < 0) //not found start tag
                            break;

                    int indexOfEndTag = message.IndexOf(currentTags.Value, indexOfStartTag + currentTags.Key.Length);
                    if (indexOfEndTag == -1) //not found end tag
                        indexOfEndTag = message.Length - 1;

                    urls.Add(message[(indexOfStartTag + currentTags.Key.Length)..indexOfEndTag]);
                    currentPos = indexOfEndTag + currentTags.Value.Length;
                }
            }
            return urls;
        }

        public IEnumerable<Link> ResponseToLinkList(HttpResponseMessage responseMessage, Link relativelyFrom, Dictionary<string, string> linkMarkers)
        { 
            if (responseMessage.IsSuccessStatusCode)
            {
                string message = responseMessage.Content.ReadAsStringAsync().Result;
                var urls = CutUrlBetweenTags(message, linkMarkers).ToList();
                var links = ToAbsoluteUrlList(urls, relativelyFrom.Url).ToList();

                return links.Select(p => new Link(p));
            }

            return new List<Link>(); // responseMessage not sucessStatusCode
        }

        /// <summary>
        /// Default delimiters is 
        /// <loc></loc> for sitemap.xml
        /// href src for view
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDefaultUrlTags(LocationUrl location)
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

        public IEnumerable<string> ToAbsoluteUrlList(IEnumerable<string> input, string relativelyFrom)
        {
            List<string> ablosuteUrl = new List<string>();
            var Protocol = relativelyFrom.Substring(0, relativelyFrom.IndexOf("//") + 2);
            foreach (string item in input)
            {
                //absolute path
                if (item.StartsWith("http://") || item.StartsWith("https://"))
                {
                    ablosuteUrl.Add(item);
                    continue;
                }
                //example: item == //example.com/ => add https://www.example.com/
                if (item.StartsWith("//") && !item.Contains(Protocol))
                {
                    ablosuteUrl.Add(Protocol + item[2..]);
                    continue;
                }

                if (item == "/")
                {
                    ablosuteUrl.Add(BaseUrl);
                    continue;
                }
                //example revativelyFrom == https://www.example.com/something/ and item == /something => https://www.example.com/something
                if (relativelyFrom.EndsWith(item))
                {
                    var absolute = relativelyFrom.Substring(0, relativelyFrom.IndexOf(item)) + item;
                    ablosuteUrl.Add(absolute);
                    continue;
                }
                //example item == /books => add BaseUrl/books
                if (item.StartsWith("/"))
                {
                    ablosuteUrl.Add(BaseUrl + item[1..]);
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
                    ablosuteUrl.Add(absolute + relative);
                    continue;
                }

            }
            return ablosuteUrl;
        }
    }
}
