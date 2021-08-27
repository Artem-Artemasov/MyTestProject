
namespace LinkFinder.Logic.Models
{
    public class Link
    {
        public string Url { get; set; }
        public int TimeResponse { get; set; }

        public Link(string url,int timeDuration = -1)
        {
            Url = url;
            TimeResponse = timeDuration;
        }
    }
}
