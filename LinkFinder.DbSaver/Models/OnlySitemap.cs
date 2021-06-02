using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkFinder.DbSaver.Models
{
    public class OnlySitemap
    {
        public int Id { get; set; }
        [InverseProperty("OnlySitemap")]
        public IEnumerable<Result> Results { get; set; }
    }
}
