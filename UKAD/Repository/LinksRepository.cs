using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKAD.Enums;
using UKAD.Filters;
using UKAD.Interfaces;
using UKAD.Models;

namespace UKAD.Repository
{
    class LinkRepository : ILinkRepository
    {
        private List<Link> Links { get; set; }
        private LinkFilter LinkFilter { get; set; }

        public LinkRepository()
        {
            this.Links = new List<Link>();
            this.LinkFilter = new LinkFilter();
        }

        /// <summary>
        /// If Link not founded in allLinks, add the Link and return AddState.AddAsNew
        /// If Link exist in allLinks and input Link have different location it 
        ///              setup the link Location.All and return AddAsAllLocation
        /// If input Link exist in allLinks and it equal, function return ExistEquals 
        /// </summary>
        public async Task<AddState> AddAsync(Link link)
        {

            link.Url = LinkFilter.WWWConvert(link.Url);

            if (link.Url.EndsWith("/") == false) link.Url = link.Url + "/";

            if (Links.Exists(p => (p.Url == link.Url)) == false)
            {
                lock (this.Links)
                {
                    this.Links.Add(link);
                }
                return AddState.AddAsNew;
            }
            else
            {
                Link foundedLink = this.Links.Find(p => (p.Url == link.Url));

                if (foundedLink.TimeDuration == -1 && link.TimeDuration == -1)
                {
                    return AddState.ExistWithoutTime;
                }

                if (foundedLink.TimeDuration == -1)
                {
                    foundedLink.TimeDuration = link.TimeDuration;
                }

                if (foundedLink.LocationUrl != link.LocationUrl && foundedLink.LocationUrl != LocationUrl.All)
                {
                    foundedLink.LocationUrl = LocationUrl.All;
                    return AddState.AddAsAllLocation;
                }

                return AddState.ExistNormal;
            }

        }
        public async Task<IEnumerable<AddState>> AddRangeAsync(IEnumerable<Link> links)
        {
            List<AddState> addStates = new List<AddState>();

            lock (this.Links)
            {
                foreach(var link in links)
                {
                    addStates.Add(AddAsync(link).Result);
                }
            }

            return addStates;
        }

        public async Task<IEnumerable<Link>> GetAllLinksAsync()
        {
            return this.Links;
        }
        public async Task<IEnumerable<Link>> GetSiteMapLinksAsync()
        {
            return this.Links.Where(p => p.LocationUrl == LocationUrl.InSiteMap);
        }
        public async Task<IEnumerable<Link>> GetViewLinksAsync()
        {
            return this.Links.Where(p => p.LocationUrl == LocationUrl.InView);
        }
        public Link GetLastLink()
        {
            return Links.LastOrDefault();
        }
        public bool Sort(Func<Link,object> func)
        {
            this.Links = this.Links.OrderBy(func).ToList();
            return true;
        }
        public bool IsProcessing(string url)
        {
            var obj = Links.Find(p => p.Url == url);
            if (obj != null)
            {
                if (obj.WorkState == WorkState.Processing)
                {
                    return true;
                }
            }

            return false;
        }
        public bool Exist(Link link) 
        {
            if (Links.Exists(p => p.Url == link.Url && p.TimeDuration != -1)) return true;
            return false;
        }

    }
}
