using System.Collections.Generic;
using UKAD.Logic.Models;
using UKAD.Logic.Interfaces.Repository;

namespace UKAD.Logic.Interfaces.View
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
