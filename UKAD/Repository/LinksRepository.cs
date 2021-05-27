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

        public LinkRepository()
        {
            Links = new List<Link>();
        }

        /// <summary>
        /// If Link not founded in allLinks, add the Link and return AddState.AddAsNew
        /// If Link exist in allLinks and input Link have different location it 
        ///              setup the link Location.All and return AddAsAllLocation
        /// If input Link exist in allLinks and it equal, function return ExistEquals 
        /// </summary>
        public virtual async Task<AddState> AddAsync(Link inputLink)
        {

            var link = Link.Clone(inputLink);

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

        public virtual async Task<IEnumerable<Link>> GetLinksAsync()
        {
            return Links.ToList();
        }

        /// <summary>
        /// Not use the GetLinksAsync() for support forwards communication with storage
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<Link>> GetLinksAsync(Func<Link,bool> func)
        {
            return Links.Where(func).ToList();
        }

/*        public virtual async Task<IEnumerable<Link>> GetSiteMapLinksAsync()
        {
            return Links.Where(p => p.LocationUrl == LocationUrl.InSiteMap).ToList();
        }

        public virtual async Task<IEnumerable<Link>> GetViewLinksAsync()
        {
            return Links.Where(p => p.LocationUrl == LocationUrl.InView).ToList();
        }*/

        public virtual bool Sort(Func<Link,object> func)
        {
            Links = Links.OrderBy(func).ToList();
            return true;
        }

        public virtual bool IsProcessing(in Link link)
        {
            var url = link.Url;
            var foundedLink = Links.Find(p => p.Url == url);
            if (null != foundedLink && foundedLink.WorkState == WorkState.Processing)
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
