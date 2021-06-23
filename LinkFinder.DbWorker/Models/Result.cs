using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LinkFinder.DbWorker.Models
{
    public class Result
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int TimeResponse { get; set; }
        [JsonIgnore]
        public int TestId { get; set; }
        [JsonIgnore]
        public Test Test { get; set; }
        [DefaultValue(false)]
        public bool InSitemap { get; set; }
        [DefaultValue(false)]
        public bool InHtml { get; set; }
    }
}
