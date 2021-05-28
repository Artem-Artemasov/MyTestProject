using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using UKAD.Logic.Enums;
using UKAD.Logic.Filters;
using UKAD.Logic.Models;

namespace UKAD.Logic.Services
{
    public class ViewCrawler
    {
        public virtual IEnumerable<Link> GetViewLinks(LinkService linkService,LinkFilter linkFilter)
        {
            var storage = new List<Link>();
            var correctUrl = linkFilter.ToSingleStyle(linkService.BaseUrl);
            storage = CrawInDomain(new Link(correctUrl), storage, linkService,linkFilter).ToList();

            return storage;
        }

        private IEnumerable<Link> CrawInDomain(in Link currentPage, List<Link> existingLinks, in LinkService linkService, in LinkFilter linkFilter)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            if (currentPage.TimeResponse < 0)
            {
                responseMessage = RequestService.SendRequest(currentPage.Url, out int timeOfResponse);
                currentPage.TimeResponse = timeOfResponse;

                if (currentPage.IsParsed)
                    return existingLinks;
            }

            currentPage.IsParsed = true;

            var foundedLinks = linkService.ResponseToLinkList(responseMessage, currentPage, LinkService.GetDefaultUrlTags(LocationUrl.InView))
                                          .ToList();

            foundedLinks = ValidateLink(foundedLinks, linkService.BaseUrl, linkFilter)
                           .Where(p => existingLinks.FirstOrDefault(s => s.Url == p.Url) == null)
                           .ToList();

            existingLinks.AddRange(foundedLinks);

            foreach(var link in foundedLinks)
            {
                link.Url = linkFilter.ToSingleStyle(link.Url);
                CrawInDomain(link, existingLinks, linkService, linkFilter);
            }

            return existingLinks;
        }

        private IEnumerable<Link> ValidateLink(in List<Link> links, string domain, LinkFilter linkFilter)
        {
            var validLinks = new List<Link>();
            foreach (var link in links)
            {
                if (validLinks.FirstOrDefault(p => p.Url == link.Url) != null)
                    continue;

                if (linkFilter.IsFileLink(link.Url))
                    continue;

                if (linkFilter.IsInDomain(link.Url, domain) == false)
                    continue;

                validLinks.Add(link);
            }
            return validLinks;
        }
    }
}
