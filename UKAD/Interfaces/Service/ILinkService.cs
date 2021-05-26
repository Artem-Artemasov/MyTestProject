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
         void SetUpBaseUrl(string baseUrl);
         Task<bool> AddLinksFromViewAsync(Link page);
         Task<bool> AddLinksFromSitemapAsync();
         Task AnalyzeSiteForUrlAsync();
    }
}
