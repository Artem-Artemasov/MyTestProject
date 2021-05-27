using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKAD.Enums;

namespace UKAD.Models
{
    public class Link
    {
        public string Url { get; set; }
        public LocationUrl LocationUrl { get; set; }
        public int TimeDuration { get; set; }
        public WorkState WorkState { get; set; }

        public Link(string url,LocationUrl location,int timeDuration = -1)
        {
            Url = url;
            LocationUrl = location;
            TimeDuration = timeDuration;
            WorkState = WorkState.Create;
        }

        public Link(string url, LocationUrl location, WorkState workState, int timeDuration):this(url,location,timeDuration)
        {
            WorkState = workState;
        }

        public static Link Clone(Link link)
        {
            return new Link(link.Url, link.LocationUrl, link.WorkState, link.TimeDuration);
        }
    }
}
