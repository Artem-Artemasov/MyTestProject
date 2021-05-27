using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UKAD.Enums;
using UKAD.Filters;
using UKAD.Interfaces;
using UKAD.Models;
using UKAD.Repository;

namespace UKAD.Services
{
    public class LinkService:ILinkService
    {
        private const int MaxTaskConst = 1;
        public string BaseUrl { get; private set; }
        public LinkRepository LinkRepository { get; set; }
        public LinkFilter LinkFilter { get; private set; }
        public RequestService RequestService { get; private set; }
        private string Host { get; set; }
        private string UrlWithHost { get; set; }

        public LinkService(LinkRepository linkRepository)
        {
            LinkRepository = linkRepository;
            LinkFilter = new LinkFilter();
        }
        public void SetUpBaseUrl(string baseUrl)
        {
            RequestService = new RequestService(baseUrl);
            BaseUrl = baseUrl;
            Host = baseUrl.Substring(0, baseUrl.IndexOf("//") + 2);
            UrlWithHost = baseUrl[(baseUrl.IndexOf("//") + 2)..];
        }

        /// <summary>
        /// If baseUrl is not setup, this method do nothing 
        /// Find links in sitemap and html code, add links in repository.
        /// It needed more time for work
        /// </summary>
        public async Task AnalyzeSiteForUrlAsync()
        {
            if (BaseUrl.Length == 0) return;

            Task[] factory = new Task[MaxTaskConst];

            for (int i = 0; i < MaxTaskConst; i++)
            {
                Thread.Sleep(50);
                factory[i] = Task.Factory.StartNew(() =>
                AddLinksFromViewAsync(new Link(BaseUrl, Enums.LocationUrl.InView))).Result;
            }

            Task.WaitAll(factory);

            await AddLinksFromSitemapAsync();
        }

        /// <summary>
        /// Find all links in html code started with page
        /// </summary>
        public async Task<bool> AddLinksFromViewAsync(Link currentPage)
        {
            if (BaseUrl.Length == 0) 
                    return false;
            if (LinkRepository.Exist(currentPage)) 
                    return true;
            if (LinkRepository.IsProcessing(currentPage) && currentPage.Url != BaseUrl) 
                    return true;

            currentPage.WorkState = WorkState.Processing;

            var responseMessage = await GetResponseMessage(currentPage);
            lock (LinkRepository)
            {

                currentPage.Url = LinkFilter.ToSingleStyle(currentPage.Url);
                LinkRepository.AddAsync(currentPage).Wait();
            }

            var pageUrlList = (await RequestService.ToLinkList(responseMessage, currentPage)).ToList();

            pageUrlList.RemoveAll(p => LinkFilter.IsInDomain(p.Url, this.BaseUrl) == false);
            pageUrlList.RemoveAll(p => LinkFilter.IsFileLink(p.Url));

            foreach (var link in pageUrlList)
            {
                AddState addState;

                link.Url = LinkFilter.ToSingleStyle(link.Url);
                lock (LinkRepository)  
                        addState = LinkRepository.AddAsync(link).Result;

                if (addState != AddState.ExistNormal)
                {
                    await AddLinksFromViewAsync(link);
                }
            }

            currentPage.WorkState = WorkState.Complete;
            return true;
        }

        /// <summary>
        /// Send request to sitemap url, find links and add it to repository
        /// </summary>
        public async Task<bool> AddLinksFromSitemapAsync()
        {
            if (BaseUrl.Length == 0) return false;

            var sitemapLink = new Link(GetSitemapUrl(), LocationUrl.InSiteMap);
            var responseMessage = await GetResponseMessage(sitemapLink);
            var linkList = await RequestService.ToLinkList(responseMessage, sitemapLink);

            foreach (var link in linkList)
            {
                if (LinkFilter.IsInDomain(link.Url, BaseUrl) == false) 
                        continue;

                link.Url = LinkFilter.ToSingleStyle(link.Url);

                AddState addState;
                lock (LinkRepository)
                    addState = LinkRepository.AddAsync(link).Result;

                if (addState == AddState.AddAsNew)
                {//needed for setup time request
                    await GetResponseMessage(link);
                }
            }

            return true;
        }

        /// <summary>
        /// Return responseMessage on request and setup time input variable
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> GetResponseMessage(Link link)
        {
            var startTime = Stopwatch.StartNew();
            var responseMessage = await RequestService.SendRequestAsync(link.Url);
            startTime.Stop();
            link.TimeDuration = (int)startTime.ElapsedMilliseconds;
            return responseMessage;
        }

        /// <summary>
        /// Find sitemap.xml file in this domain
        /// </summary>
        private string GetSitemapUrl()
        {
            return BaseUrl + "/sitemap.xml";
        }

    }
}
