using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKAD.Enums;
using UKAD.Models;

namespace UKAD.Interfaces
{
    public interface ILinkRepository
    {
        public Task<AddState> AddAsync(Link link);
        public Task<IEnumerable<AddState>> AddRangeAsync(IEnumerable<Link> links);
        public Task<IEnumerable<Link>> GetSiteMapLinksAsync();
        public Task<IEnumerable<Link>> GetViewLinksAsync();
        public Task<IEnumerable<Link>> GetAllLinksAsync();
        public Link GetLastLink();
        public bool Sort(Func<Link, object> func);
        public bool IsProcessing(string url);
        public bool Exist(Link link);
    }
}
