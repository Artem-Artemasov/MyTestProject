using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkFounder.DbSaver.Models
{
    public class Test
    {
        public int Id { get; set; }
        public string Url { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime TimeCreated { get; set; }
        public IEnumerable<Result> Results { get; set; }
    }
}
