using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LinkFinder.Logic.Validators
{
    public class LinkValidator
    {
        /// <summary>
        /// Return true if string do not have a files extension
        /// </summary>
        public virtual bool IsFileLink(string link)
        {
            string[] extensions = new string[]
            {
                ".css",
                ".json",
                ".js",
                ".exe",
                ".jpeg",
                ".sql",
                ".png",
                ".jpg",
                ".svg",
                ".ttf",
                ".woff",
                ".woff2",
                ".ico",
                "@"
            };

           return extensions.Any(p => link.Contains(p));
        }

        public virtual bool IsCorrectLink(string link,out string errorMessage)
        {
            if (String.IsNullOrEmpty(link))
            {
                errorMessage = "String is empty";
                return false;
            }

            int indexProtocol = link.IndexOf("https://");
            string protocol = "https://";

            if (indexProtocol == -1)
            {
                indexProtocol = link.IndexOf("http://");
                protocol = "http://";
            }
            
            if ( indexProtocol == -1 || link.Length <= protocol.Length) // not have protocol or link == protocol
            {
                errorMessage = "Url haven't protocol or url is a only protocol";
                return false;
            }

            if (link.Contains(".") == false) // not have .
            {
                errorMessage = "Url haven't a domain";
                return false;
            }
            
            int indexOfwww = link.IndexOf("www.");
            if (link.Length <=  indexOfwww+ 4)
            {
                errorMessage = "So short url";
                return false;
            }

            if (indexOfwww != -1 && link.IndexOf('.', indexOfwww + 4) == -1) // for example: link == https://www. 
            {
                errorMessage = "Url not contains a domain name";
                return false;
            }

            errorMessage = "";
            return true;
        }

        public virtual bool IsInCurrentSite(string link, string baseUrl)
        {
            return link.Contains(baseUrl);
        }
    }
}
