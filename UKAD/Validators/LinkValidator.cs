using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LinkFounder.Logic.Validators
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
            };

            foreach (var item in extensions)
            {
                if (link.IndexOf(item) != -1)
                {
                    return true;
                }  
            }

            return false;
        }

        public virtual bool IsCorrectLink(string link)
        {
            if (link.Contains("https://") == false || link.Contains("http://") == false)
            {
                return false;
            }

            if (link.Contains(".") == false)
            {
                return false;
            }

            return true;
        }

        public virtual bool IsInDomain(string link, string domain)
        {
            if (link.Contains(domain) == false)
                return false;

            return true;
        }
    }
}
