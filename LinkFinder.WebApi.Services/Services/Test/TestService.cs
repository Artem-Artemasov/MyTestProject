using AutoMapper;
using LinkFinder.DbWorker;
using LinkFinder.Logic.Validators;
using LinkFinder.WebApi.Services.Filters;
using LinkFinder.WebApi.Services.Request;
using LinkFinder.WebApi.Services.Response;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkFinder.WebApi.Services
{
    public class TestService
    {
        private readonly DatabaseWorker _dbWorker;
        private readonly LinkValidator _linkValidator;
        private readonly CrawlerApp _crawlerApp;
        private readonly ResultFilter _resultFilter;
        private readonly IMapper _mapper;

        public TestService(CrawlerApp crawlerApp,
            LinkValidator linkValidator,
            DatabaseWorker dbWorker,
            ResultFilter resultsFilter,
            IMapper mapper)
        {
            _crawlerApp = crawlerApp;
            _linkValidator = linkValidator;
            _dbWorker = dbWorker;
            _resultFilter = resultsFilter;
            _mapper = mapper;
        }

        public virtual async Task<ResponseMessage> GetAllTestsAsync()
        {
            var tests = await _dbWorker.GetTestsAsync();

            tests = tests.OrderByDescending(p => p.TimeCreated);

            return new ResponseMessage()
            {
                Content = _mapper.Map<IEnumerable<TestDto>>(tests)
            };
        }

        public virtual async Task<ResponseMessage> AddTestAsync(string url)
        {
            if (_linkValidator.IsCorrectLink(url, out string errorMessage) == false)
            {
                return new ResponseMessage()
                {
                    IsSuccessful = false,
                    Content = errorMessage,
                };
            }

            var createdTest = await _crawlerApp.StartWork(url);

            return new ResponseMessage()
            {
                Content = _mapper.Map<TestDto>(createdTest)
            };
        }

        public virtual async Task<ResponseMessage> GetTestAsync(int testId, GetTestDetailParam param)
        {
            var test = (await _dbWorker.GetTestsAsync())
                                       .FirstOrDefault(p => p.Id == testId);

            //Get results sorted it and get needed page
            var testResults = (await _dbWorker.GetResultsAsync(test.Id))
                                              .OrderBy(p => p.TimeResponse)
                                              .AsQueryable();

            testResults = _resultFilter.Filter(testResults, param);

            test.Results = testResults.Pagination(param.Page - 1, param.CountResultsOnPage).ToList();

            var detailTest = _mapper.Map<DetailTestDto>(test);

            return new ResponseMessage
            {
                Content = detailTest,
            };
        }
    }
}
