using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UKAD.Enums;
using UKAD.Filters;
using UKAD.Models;

namespace UKAD.Repository
{
    public class LinkRepository
    {
        private List<Link> Links { get; set; }
        private LinkFilter LinkFilter { get; set; }

        public LinkRepository()
        {
            Links = new List<Link>();
            LinkFilter = new LinkFilter();
        }

        /// <summary>
        /// If Link not founded in allLinks, add the Link and return AddState.AddAsNew
        /// If Link exist in allLinks and input Link have different location it 
        ///              setup the link Location.All and return AddAsAllLocation
        /// If input Link exist in allLinks and it equal, function return ExistEquals 
        /// </summary>
        public virtual async Task<AddState> AddAsync(Link link)
        {

            link.Url = LinkFilter.AddWWW(link.Url);

            if (link.Url.EndsWith("/") == false) link.Url = link.Url + "/";

            if (Links.Exists(p => (p.Url == link.Url)) == false)
            {
                lock (Links)
                {
                    Links.Add(link);
                }
                return AddState.AddAsNew;
            }
            else // Links not exist in repo
            {
                Link foundedLink = Links.Find(p => (p.Url == link.Url));

                if (foundedLink.TimeDuration == -1 && link.TimeDuration == -1) // input and founded links not have a time
                        return AddState.ExistWithoutTime;

                if (foundedLink.TimeDuration == -1) // founded links not have a time
                        foundedLink.TimeDuration = link.TimeDuration;

                // input links have a different location with founded
                if (foundedLink.LocationUrl != link.LocationUrl && foundedLink.LocationUrl != LocationUrl.All) 
                {
                    foundedLink.LocationUrl = LocationUrl.All;
                    return AddState.AddAsAllLocation;
                }

                return AddState.ExistNormal;
            }

        }

        public virtual async Task<IEnumerable<Link>> GetAllLinksAsync()
        {
            return Links;
        }

        public virtual async Task<IEnumerable<Link>> GetSiteMapLinksAsync()
        {
            return Links.Where(p => p.LocationUrl == LocationUrl.InSiteMap);
        }

        public virtual async Task<IEnumerable<Link>> GetViewLinksAsync()
        {
            return Links.Where(p => p.LocationUrl == LocationUrl.InView);
        }

        public virtual bool Sort(Func<Link,object> func)
        {
            Links = Links.OrderBy(func).ToList();
            return true;
        }

        public virtual bool IsProcessing(in Link link)
        {
            var url = link.Url;
            var foundedLink = Links.Find(p => p.Url == url);
            if (foundedLink != null && foundedLink.WorkState == WorkState.Processing)
                    return true;

            return false;
        }

        public virtual bool Exist(Link link) 
        {
            if (Links.Exists(p => p.Url == link.Url && p.TimeDuration != -1)) return true;
            return false;
        }

    }
}
