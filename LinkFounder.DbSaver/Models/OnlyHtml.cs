using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkFounder.DbSaver.Models
{
    public class OnlyHtml
    {
        public int Id { get; set; }
        [InverseProperty("OnlyHtml")]
        public IEnumerable<Result> Results { get; set; }
    }
}
