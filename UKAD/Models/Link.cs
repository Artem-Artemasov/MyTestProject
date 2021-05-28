namespace UKAD.Logic.Models
{
    public class Link
    {
        public string Url { get; set; }
        public int TimeResponse { get; set; }
        public bool IsParsed { get; set; }

        public Link(string url,int timeDuration = -1,bool isParsed = false)
        {
            Url = url;
            TimeResponse = timeDuration;
            IsParsed = isParsed;
        }

        public static Link Clone(Link link)
        {
            return new Link(link.Url, link.TimeResponse,link.IsParsed);
        }

    }
}
