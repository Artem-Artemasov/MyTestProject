using System;
using System.Collections.Generic;

namespace LinkFinder.WebApi.Services.Response
{
    public class DetailTestDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public DateTime TimeCreated { get; set; }
        public IEnumerable<ResultDto> Results { get; set; }
    }
}
