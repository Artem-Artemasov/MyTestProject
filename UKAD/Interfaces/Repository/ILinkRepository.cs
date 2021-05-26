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
         Task<AddState> AddAsync(Link link);
         Task<IEnumerable<AddState>> AddRangeAsync(IEnumerable<Link> links);
         Task<IEnumerable<Link>> GetSiteMapLinksAsync();
         Task<IEnumerable<Link>> GetViewLinksAsync();
         Task<IEnumerable<Link>> GetAllLinksAsync();
         Link GetLastLink();
         bool Sort(Func<Link, object> func);
         bool IsProcessing(in Link link);
         bool Exist(Link link);
    }
}
