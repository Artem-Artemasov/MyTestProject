using LinkFinder.DbWorker;
using LinkFinder.Logic.Validators;
using LinkFinder.WebApi.Models;
using LinkFinder.WebApi.RoutingParams;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkFinder.WebApi.Services
{
    public class TestsService
    {
        private readonly DatabaseWorker _dbWorker;
        private readonly LinkValidator _linkValidator;
        private readonly CrawlerApp _crawlerApp;
        private readonly ResultsService _resultsService;

        public TestsService(CrawlerApp crawlerApp, LinkValidator linkValidator, DatabaseWorker dbWorker, ResultsService resultsService)
        {
            _crawlerApp = crawlerApp;
            _linkValidator = linkValidator;
            _dbWorker = dbWorker;
            _resultsService = resultsService;
        }

        public virtual async Task<IEnumerable<ApiTest>> GetAllTests()
        {
            var tests = await _dbWorker.GetTestsAsync();

            return tests.Select(p => new ApiTest()
            {
                Id = p.Id,
                Url = p.Url,
                TimeCreated = p.TimeCreated,
            });
        }

        public virtual async Task<string> AddTest(string url)
        {
            if (_linkValidator.IsCorrectLink(url, out string errorMessage) == false)
            {
                return errorMessage;
            }

            await _crawlerApp.StartWork(url);

            return null;
        }

        public virtual async Task<ApiDetailTest> GetTest(int id, TestDetailParam param)
        {
            var test = (await _dbWorker.GetTestsAsync())
                                    .Include(p => p.Results)
                                    .FirstOrDefault(p => p.Id == id);

            return new ApiDetailTest()
            {
                Id = test.Id,
                Url = test.Url,
                TimeCreated = test.TimeCreated,
                Results = test.Results.Select(p => ResultsService.MapApiResult(p)),
            };
        }

    }
}
