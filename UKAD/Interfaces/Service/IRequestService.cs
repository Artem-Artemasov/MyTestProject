using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UKAD.Enums;
using UKAD.Models;

namespace UKAD.Interfaces.Service
{
    public interface IRequestService
    {
        public Task<HttpResponseMessage> SendRequestAsync(string page);
        public Task<IEnumerable<Link>> ConvertLinks(HttpResponseMessage responseMessage, Link link);
    }
}
