using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UKAD.Interfaces;

namespace UKAD.Filters
{
    public class LinkFilter
    { 
        /// <summary>
        /// Return true if string do not have a files extension
        /// </summary>
        public virtual bool IsFileLink(string link)
        {
            string[] expansions = new string[]
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
            };

            foreach(var item in expansions)
            {
                if (link.IndexOf(item) != -1) return true ;
            }

            return false;
        }

        public virtual bool IsCorrectLink(string link)
        {
            var regex = new Regex(@"(?<1>(ht)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*))");

            int matchCount = regex.Matches(link).Count;

            if (matchCount != 0 && !link.Contains("@")) return true;

            return false;
        }

        /// <summary>
        /// example ToSingleStyle("https://example.com") = > https://www.example.com/
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public virtual string ToSingleStyle(string link)
        {
            if (link.Contains("www.") == false)
            {
                int httpIndex = link.IndexOf("//") + 2;
                link = link.Insert(httpIndex, "www.");
            }

            if (link.EndsWith("/") == false) link += "/";

            return link;
        }

        public virtual bool IsInDomain(string link,string domain)
        {
            string domainWhOutWWW = "";
            if (domain.Contains("www."))
            {
                domainWhOutWWW = domain.Replace("www.","");
            }
            if (link.Contains(domain) || link.Contains(domainWhOutWWW)) return true;

            return false;
        }
    }
}
