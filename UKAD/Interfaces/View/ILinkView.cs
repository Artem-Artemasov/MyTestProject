using System.Collections.Generic;
using UKAD.Models;
using UKAD.Interfaces.Repository;

namespace UKAD.Interfaces.View
{
    public interface ILinkView
    {
        string ReadUrl();
        bool PrintProcessingMessage();
        bool PrintErrorMessage(string errorMessage);
        bool PrintList(IEnumerable<Link> links);
        bool PrintCounts(ILinkRepository links);
        bool PrintWithTime(IEnumerable<Link> links);
        bool PrintAllInformation(ILinkRepository linkRepository);
    }
}
