using System.Collections.Generic;
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
    }
}
