using LinkFinder.DbWorker;
using LinkFinder.DbWorker.Models;
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

        public virtual async Task<IEnumerable<ApiTest>> GetAllTestsAsync()
        {
            var tests = await _dbWorker.GetTestsAsync();

            return tests.Select(p => new ApiTest()
            {
                Id = p.Id,
                Url = p.Url,
                TimeCreated = p.TimeCreated,
            });
        }

        public virtual async Task<string> AddTestAsync(string url)
        {
            if (_linkValidator.IsCorrectLink(url, out string errorMessage) == false)
            {
                return errorMessage;
            }

            await _crawlerApp.StartWork(url);

            return null;
        }

        public virtual async Task<ApiDetailTest> GetTestAsync(int testId, TestDetailParam param)
        {
            var test = (await _dbWorker.GetTestsAsync()).FirstOrDefault(p => p.Id == testId);
            
            //Get tests sorted it and get needed page
            var testResults = (await _dbWorker.GetResultsAsync(test.Id)).OrderBy(p => p.TimeResponse).AsQueryable();
            
            //When need not all results
            if (param.InHtml || param.InSitemap)
            {
                testResults = testResults.Where(p => p.InSitemap == param.InSitemap)
                           .Where(p => p.InHtml == param.InHtml);
            }
            else
            {
                testResults =  testResults.Skip((param.Page - 1) * param.CountResultsOnPage)
                                          .Take(param.CountResultsOnPage);
            }

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
