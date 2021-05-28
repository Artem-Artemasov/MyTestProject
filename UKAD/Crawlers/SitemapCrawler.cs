using System.Collections.Generic;
using UKAD.Logic.Enums;
using UKAD.Logic.Models;
using UKAD.Logic.Services;

namespace UKAD.Logic.Crawlers
{
    public class SitemapCrawler
    {
        public virtual IEnumerable<Link> SitemapLinkFindAsync(string baseUrl,LinkService linkService)
        {
            var sitemapLink = new Link(GetSitemapUrl(baseUrl));
            var responseMessage = RequestService.SendRequest(sitemapLink.Url, out int timeOfResponse);
            sitemapLink.TimeResponse = timeOfResponse;
            var foundedLinkList =  linkService.ResponseToLinkList(responseMessage, sitemapLink, LinkService.GetDefaultUrlTags(LocationUrl.InSiteMap));

            foreach (var link in foundedLinkList)
            {
                RequestService.SendRequest(link.Url, out int timeResponse);
                link.TimeResponse = timeResponse;
            }

            return foundedLinkList;
        }
        public virtual string GetSitemapUrl(string baseUrl)
        {
            return baseUrl + "/sitemap.xml";
        }
    }
}
