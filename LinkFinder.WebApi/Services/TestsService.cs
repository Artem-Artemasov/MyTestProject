using LinkFinder.DbWorker;
using LinkFinder.Logic.Validators;
using LinkFinder.WebApi.Filters;
using LinkFinder.WebApi.Mappers;
using LinkFinder.WebApi.Models;
using LinkFinder.WebApi.RoutingParams;
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
        private readonly ResultsFilter _resultsFilter;

        public TestsService(CrawlerApp crawlerApp, LinkValidator linkValidator, DatabaseWorker dbWorker, ResultsFilter resultsFilter)
        {
            _crawlerApp = crawlerApp;
            _linkValidator = linkValidator;
            _dbWorker = dbWorker;
            _resultsFilter = resultsFilter;
        }

        public virtual async Task<IEnumerable<ApiTest>> GetAllTestsAsync()
        {
            var tests = await _dbWorker.GetTestsAsync();

            tests = tests.OrderByDescending(p => p.TimeCreated);

            return tests.Select(p => TestApiMapper.Map(p));
        }

        public virtual async Task<ApiTest> AddTestAsync(string url)
        {
            if (_linkValidator.IsCorrectLink(url, out string errorMessage) == false)
            {
                throw new System.Exception(errorMessage);
            }

            var createdTest = await _crawlerApp.StartWork(url);

            return TestApiMapper.Map(createdTest);
        }

        public virtual async Task<ApiDetailTest> GetTestAsync(int testId, TestDetailParam param)
        {
            var test = (await _dbWorker.GetTestsAsync())
                                       .FirstOrDefault(p => p.Id == testId);

            //Get results sorted it and get needed page
            var testResults = (await _dbWorker.GetResultsAsync(test.Id))
                                              .OrderBy(p => p.TimeResponse)
                                              .AsQueryable();

            testResults = _resultsFilter.Filter(testResults, param);

            testResults = testResults.Skip((param.Page - 1) * param.CountResultsOnPage)
                                          .Take(param.CountResultsOnPage);

            return new ApiDetailTest()
            {
                Id = test.Id,
                Url = test.Url,
                TimeCreated = test.TimeCreated,
                Results = testResults.Select(p => ResultsService.MapApiResult(p)),
            };
        }
    }
}
