using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkFounder.DbSaver.Models
{
    public class Test
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public IEnumerable<Result> Results { get; set; }
    }
}
