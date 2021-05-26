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
        public Link(string Url,LocationUrl location,int timeDuration = -1)
        {
            this.Url = Url;
            this.LocationUrl = location;
            this.TimeDuration = timeDuration;
            this.WorkState = WorkState.Create;
        }
    }
}
