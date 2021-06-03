using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkFinder.DbWorker.Models
{
    public class Result
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int TimeResponse { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
        [DefaultValue(false)]
        public bool InSitemap { get; set; }
        [DefaultValue(false)]
        public bool InHtml { get; set; }
    }
}
