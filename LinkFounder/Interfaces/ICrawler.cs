using LinkFounder.Logic.Models;
using System.Collections.Generic;


namespace LinkFounder.Logic.Interfaces
{
    public interface ICrawler
    {
        IEnumerable<Link> GetLinks(string baseUrl);
    }
}
