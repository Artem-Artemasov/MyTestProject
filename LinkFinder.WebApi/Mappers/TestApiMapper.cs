using LinkFinder.DbWorker.Models;
using LinkFinder.WebApi.Models;

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
