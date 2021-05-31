using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UKAD.Enums;
using UKAD.Models;

namespace UKAD.Interfaces.Repository
{
    public interface ILinkRepository
    {
        Task<AddState> AddAsync(Link inputLink);
        Task<IEnumerable<Link>> GetLinksAsync();
        Task<IEnumerable<Link>> GetLinksAsync(Func<Link, bool> func);
        bool Sort(Func<Link, object> func);
        bool IsProcessing(in Link link);
        bool Exist(Link link);
    }
}
