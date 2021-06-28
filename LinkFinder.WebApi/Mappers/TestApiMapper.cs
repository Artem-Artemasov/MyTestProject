using LinkFinder.DbWorker.Models;
using LinkFinder.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkFinder.WebApi.Mappers
{
    public static class TestApiMapper
    {
        public static ApiTest Map(Test test)
        {
            return new ApiTest()
            {
                Id = test.Id,
                Url = test.Url,
                TimeCreated = test.TimeCreated,
            };
        }

    }
}
