using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkFinder.WebApi.Models
{
    public class ApiDetailTest
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public DateTime TimeCreated { get; set; }
        public IEnumerable<ApiResult> Results {get;set;}
    }
}
