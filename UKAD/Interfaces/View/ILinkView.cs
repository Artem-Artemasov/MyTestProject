using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKAD.Models;

namespace UKAD.Interfaces
{
    public interface ILinkView
    {
        public bool Processing();
        public bool PrintList(IEnumerable<Link> links);
        public bool PrintCounts(ILinkRepository links);
        public bool PrintWithTime(IEnumerable<Link> links);
    }
}
