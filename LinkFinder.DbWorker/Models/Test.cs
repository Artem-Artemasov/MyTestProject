using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LinkFinder.DbWorker.Models
{
    public class Test
    {
        public int Id { get; set; }
        public string Url { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime TimeCreated { get; set; }
        [JsonIgnore]
        public IEnumerable<Result> Results { get; set; }
    }
}
