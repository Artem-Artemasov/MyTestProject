using LinkFounder.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkFounder.Logic.Interfaces
{
    public interface ICrawler
    {
        public IEnumerable<Link> GetLinks(string baseUrl);
    }
}
