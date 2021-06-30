using System.Collections.Generic;

namespace LinkFinder.Logic.Services
{
    public class LinkParser
    {
        public virtual IEnumerable<string> Parse(string message)
        {
            List<string> urls = new List<string>();
            var urlTags = GetDefaultUrlTags();

            foreach (var currentTags in urlTags)
            {
                int currentPos = 0;
                while (currentPos < message.Length) //find urls in all message between currentTags
                {
                    int indexOfStartTag = message.IndexOf(currentTags.Key, currentPos);

                    if (indexOfStartTag < 0)
                    {//not found start tag
                        break;
                    }

                    int indexOfEndTag = message.IndexOf(currentTags.Value, indexOfStartTag + currentTags.Key.Length);
                    if (indexOfEndTag == -1) //not found end tag
                    {
                        indexOfEndTag = message.IndexOf(" ", indexOfStartTag);
                    }

                    urls.Add(message[(indexOfStartTag + currentTags.Key.Length)..indexOfEndTag]);
                    currentPos = indexOfEndTag + currentTags.Value.Length;
                }
            }
            return urls;
        }

        public static Dictionary<string, string> GetDefaultUrlTags()
        {
            return new Dictionary<string, string>
            {
                { "<loc>", "</loc>" },
                { "href=\"", "\"" },
                { "href='", "'" },
                { "src=\"", "\"" },
                { "src='", "'" }
            };
        }
    }
}
