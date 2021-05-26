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
    public class RequestService:IRequestService
    {
        public string BaseUrl { get; private set; }
        private string Host { get; set; }
        private string UrlWithHost { get; set; }
        public RequestService(string baseUrl)
        {
            this.BaseUrl = baseUrl;
            this.Host = baseUrl.Substring(0, baseUrl.IndexOf("//") + 2);
            this.UrlWithHost = baseUrl[(baseUrl.IndexOf("//") + 2)..];
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
        public virtual async Task<IEnumerable<Link>> ConvertLinks(HttpResponseMessage responseMessage,Link link)
        {
            List<Link> links = new List<Link>();

            if (responseMessage.IsSuccessStatusCode)
            {
                string message = await responseMessage.Content.ReadAsStringAsync();
                var urls = ParseMessageToUrl(message, link.LocationUrl).ToList();
                links = ConvertRelateToAbsolute(urls, link.Url, link.LocationUrl).ToList();
            }

            return links;
        }
        /// <summary>
        /// Find all links, 
        /// If we finding in view, it looked on href and src attributes
        /// If we finding in sitemap it looked on <loc> tag
        /// </summary>
        public virtual IEnumerable<string> ParseMessageToUrl(string message, LocationUrl location)
        {
            Regex regex;
            List<string> urls = new List<string>();

            if (location == LocationUrl.InSiteMap)
            {
                regex = new Regex(@"(?<1>(ht)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*))"
                                , RegexOptions.Multiline | RegexOptions.ExplicitCapture);
            }
            else
            {
                regex = new Regex(@"(href|src)\s*=\s*([""'](?<1>([^""']*))[""'])",
                    RegexOptions.Multiline | RegexOptions.ExplicitCapture);
            }


            MatchCollection matches = regex.Matches(message);

            if (matches.Count > 0)
            {
                urls.AddRange(matches.Select(p => p.Groups[1].Value));

                /*foreach (Match match in matches)
                {
                    urls.Add(match.Groups[1].Value);
                }*/
            }

            return urls;
        }

        /// <summary>
        /// Convert a revalite url to absolute with help baseUrl
        /// </summary>
        /// P.s Метод сильно громозкий, но пока не появилось идей как уменьшить
        /// <returns></returns>
        public virtual IEnumerable<Link> ConvertRelateToAbsolute(IEnumerable<string> input, string baseUrl, LocationUrl location)
        {
            List<Link> ablosuteUrl = new List<Link>();

            foreach (string item in input)
            {
                if (item.StartsWith("http://") || item.StartsWith("https://"))
                {
                    ablosuteUrl.Add(new Link(item, location));
                    continue;
                }

                if (item.StartsWith("//") && !item.Contains(this.Host))
                {
                    ablosuteUrl.Add(new Link(this.Host + item[2..], location));
                    continue;
                }

                if (item == "/")
                {
                    ablosuteUrl.Add(new Link(this.BaseUrl, location));
                    continue;
                }


                if (baseUrl.EndsWith(item))
                {
                    string absolute;
                    absolute = baseUrl.Substring(0, baseUrl.IndexOf(item)) + item;
                    ablosuteUrl.Add(new Link(absolute, location));
                    continue;
                }

                if (item.StartsWith("/"))
                {
                    ablosuteUrl.Add(new Link(this.BaseUrl + item, location));
                    continue;
                }

                if (item.StartsWith("./"))
                {
                    string absolute = baseUrl;
                    string relative = item;
                    while (relative.StartsWith("./"))
                    {
                        absolute = absolute.Substring(0, baseUrl.LastIndexOf("/"));
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
