using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKAD.Models;
using UKAD.Repository;

namespace UKAD.Interfaces
{
    public interface ILinkView
    {
        string ReadUrl();
        bool PrintProcessingMessage();
        bool PrintErrorMessage(string errorMessage);
        bool PrintList(IEnumerable<Link> links);
        bool PrintCounts(LinkRepository links);
        bool PrintWithTime(IEnumerable<Link> links);
        bool PrintAllInformation(LinkRepository linkRepository);
    }
}
