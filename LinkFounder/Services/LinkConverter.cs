using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkFounder.Logic.Services
{
    public class LinkConverter
    {
        public virtual IEnumerable<string> RelativeToAbsolute(IEnumerable<string> input, string relativelyFrom)
        {
            List<string> ablosuteUrl = new List<string>();

            if (relativelyFrom.EndsWith('/') == false)
            {
                relativelyFrom += '/';
            }

            var Protocol = relativelyFrom.Substring(0, relativelyFrom.IndexOf("//") + 2);

            var baseUrl = CutBaseUrl(relativelyFrom);

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
                    ablosuteUrl.Add(baseUrl);
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
                    ablosuteUrl.Add(baseUrl + item[1..]);
                    continue;
                }
                //example item == ./books/firstbook.html and relativelyFrom == https://www.example.com/library/magazine => https://www.example.com/library/books/firstbook.html
                if (item.StartsWith("./"))
                {
                    string absolute = relativelyFrom;
                    string relative = item;

                    absolute = absolute[0..(absolute.LastIndexOf("/"))];

                    while (relative.StartsWith("./"))
                    {
                        relative = relative[2..];
                        absolute = absolute[0..(absolute.LastIndexOf("/"))]; 
                    }


                    ablosuteUrl.Add(absolute + '/' + relative);
                    continue;
                }

            }
            return ablosuteUrl;
        }

        private string CutBaseUrl(string relatively)
        {
            if (relatively == "")
            {
                return "";
            }

            var Protocol = relatively.Substring(0, relatively.IndexOf("//") + 2);

            int indexEndDomain = relatively.IndexOf('/', relatively.IndexOf(Protocol) + Protocol.Length);

            return relatively[0..indexEndDomain] + '/';
        }
    }
}
