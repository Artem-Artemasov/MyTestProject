using System.Collections.Generic;

namespace LinkFounder.DbSaver.Models
{
    public class Test
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public IEnumerable<Result> Results { get; set; }
    }
}
