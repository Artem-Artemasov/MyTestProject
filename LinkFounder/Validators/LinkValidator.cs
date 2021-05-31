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
                if (link.Contains(item))
                {
                    return true;
                }  
            }

            return false;
        }

        public virtual bool IsCorrectLink(string link)
        {
            if (link.IndexOf("https://") != -1 && link.IndexOf("http://") != -1)
            {
                return false;
            }

            if (link.IndexOf(".") == -1)
            {
                return false;
            }

            return true;
        }

        public virtual bool IsInDomain(string link, string domain)
        {
            if (link.Contains(domain) == false)
            {
                return false;
            }
            return true;
        }
    }
}
