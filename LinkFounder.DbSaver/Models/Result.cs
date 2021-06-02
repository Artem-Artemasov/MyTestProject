using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkFounder.DbSaver.Models
{
    public class Result
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int TimeResponse { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
        [InverseProperty("Results")]
        public IEnumerable<OnlyHtml> OnlyHtml { get; set; }
        [InverseProperty("Results")]
        public IEnumerable<OnlySitemap> OnlySitemap { get; set; }
    }
}
