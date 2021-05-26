using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UKAD.Enums;
using UKAD.Filters;
using UKAD.Interfaces;
using UKAD.Models;

namespace UKAD.Services
{
    public class LinkService:ILinkService
    {
        public string BaseUrl { get; private set; }
        public ILinkRepository LinkRepository { get; set; }
        public LinkFilter LinkFilter { get; private set; }
        public RequestService RequestService { get; private set; }
        private string Host { get; set; }
        private string UrlWithHost { get; set; }
        private int MaxTask { get; set; }
        public LinkService(ILinkRepository linkRepository)
        {
            this.LinkRepository = linkRepository;
            this.LinkFilter = new LinkFilter();
            MaxTask = 10;

        }
        public void SetBaseUrl(string baseUrl)
        {
            this.RequestService = new RequestService(baseUrl);
            this.BaseUrl = baseUrl;
            this.Host = baseUrl.Substring(0, baseUrl.IndexOf("//") + 2);
            this.UrlWithHost = baseUrl[(baseUrl.IndexOf("//") + 2)..];
        }

        /// <summary>
        /// If baseUrl is not setup, this method do nothing 
        /// Find links in sitemap and html code, add links in repo.
        /// </summary>
        public async Task FindAllLinksAsync()
        {
            if (this.BaseUrl.Length == 0) return;

            Task[] factory = new Task[MaxTask];

            for (int i = 0; i <MaxTask; i++)
            {
                /*Thread.Sleep(50);*/
                factory[i] = Task.Factory.StartNew(() => 
                this.FindLinksInViewAsync(new Link(this.BaseUrl, Enums.LocationUrl.InView))).Result;
            }

            Task.WaitAll(factory);

            await this.FindLinksInSitemapAsync();
        }

        /// <summary>
        /// Find all links in html code started with page
        /// </summary>
        public async Task<bool> FindLinksInViewAsync(Link page)
        {
            if (this.BaseUrl.Length == 0) return false;
            if (LinkRepository.Exist(page)) return true;
            if (LinkRepository.IsProcessing(page.Url) && page.Url != BaseUrl) return true;

            page.WorkState = WorkState.Processing;
            var responseMessage = await TimeRequest(page);
           /* Console.WriteLine(page.Url);*/
            lock (LinkRepository)
            {
                LinkRepository.AddAsync(page).Wait();
            }
            var pageUrlList = (await RequestService.ConvertLinks(responseMessage, page)).ToList();
            pageUrlList.RemoveAll(p =>!LinkFilter.IsInDomain(p.Url, this.BaseUrl) || LinkFilter.IsFileLink(p.Url));

            foreach (var link in pageUrlList)
            {
                AddState result;

                lock (this.LinkRepository)  
                {
                    result = this.LinkRepository.AddAsync(link).Result;
                }

                if (result != AddState.ExistNormal)
                {
                    await FindLinksInViewAsync(link);
                }

            }
            page.WorkState = WorkState.Complete;
            return true;
        }

        /// <summary>
        /// Send request to site/sitemap.xml, find links and add it to repo
        /// </summary>
        public async Task<bool> FindLinksInSitemapAsync()
        {
            if (this.BaseUrl.Length == 0) return false;

            var sitemap = new Link(this.FindSitemapUrl(), LocationUrl.InSiteMap);

            var responseMessage = await RequestService.SendRequestAsync(sitemap.Url);

            var sitemapList = await RequestService.ConvertLinks(responseMessage,sitemap);

            foreach (var link in sitemapList)
            {
                if (this.LinkFilter.IsInDomain(link.Url, this.BaseUrl) == false) continue;
                var result = await LinkRepository.AddAsync(link);

                if (result == AddState.AddAsNew)
                {
                    await TimeRequest(link);
                }
            }
            return true;
        }

        protected async Task<HttpResponseMessage> TimeRequest(Link link)
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
        protected virtual string FindSitemapUrl()
        {
            return this.BaseUrl + "/sitemap.xml";
        }

    }
}
