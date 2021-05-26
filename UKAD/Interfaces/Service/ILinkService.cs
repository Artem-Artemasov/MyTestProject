using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UKAD.Enums;
using UKAD.Models;

namespace UKAD.Interfaces
{
    public interface ILinkService
    {
        public ILinkRepository LinkRepository { get; set; }
        public void SetBaseUrl(string baseUrl);
        public Task<bool> FindLinksInViewAsync(Link page);
        public Task<bool> FindLinksInSitemapAsync();
        public Task FindAllLinksAsync();
    }
}
