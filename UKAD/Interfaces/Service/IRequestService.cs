using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UKAD.Enums;
using UKAD.Models;

namespace UKAD.Interfaces.Service
{
    public interface IRequestService
    {
         Task<HttpResponseMessage> SendRequestAsync(string page);
         Task<IEnumerable<Link>> ToLinkList(HttpResponseMessage responseMessage, Link link);
    }
}
