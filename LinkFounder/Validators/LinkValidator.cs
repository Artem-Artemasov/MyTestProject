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
            int indexProtocol = link.IndexOf("https://");
            string protocol = "https://";

            if (indexProtocol == -1)
            {
                indexProtocol = link.IndexOf("http://");
                protocol = "http://";
            }
            
            if ( indexProtocol == -1 || link.Length <= protocol.Length) // not have protocol or link == protocol
            {
                return false;
            }

            if (link.IndexOf(".") == -1) // not have .
            {
                return false;
            }
            
            if (link.Length <= indexProtocol + protocol.Length + 1 || Char.IsLetter(link[indexProtocol + protocol.Length + 1]) == false)
            {
                return false;
            }

            int indexOfwww = link.IndexOf("www.");
            if (link.Length <=  indexOfwww+ 4)
            {
                return false;
            }

            if (indexOfwww != -1 && link.IndexOf('.', indexOfwww + 4) == -1)
            {
                return false;
            }

            return true;
        }

        public virtual bool IsInCurrentSite(string link, string baseUrl)
        {
            return link.Contains(baseUrl);
        }
    }
}
