using AutoMapper;
using LinkFinder.DbWorker;
using LinkFinder.DbWorker.Interfaces;
using LinkFinder.Logic.Validators;
using LinkFinder.WebApi.Logic.Errors;
using LinkFinder.WebApi.Logic.Filters;
using LinkFinder.WebApi.Logic.Request;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkFinder.WebApi.Logic.Response.Services
{
    public class TestService
    {
        private readonly DatabaseWorker _dbWorker;
        private readonly LinkValidator _linkValidator;
        private readonly ICrawlerApp _crawlerApp;
        private readonly ResultFilter _resultFilter;
        private readonly IMapper _mapper;

        public TestService(ICrawlerApp crawlerApp,
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

        public virtual async Task<IEnumerable<TestDto>> GetAllTestsAsync()
        {
            var tests = await _dbWorker.GetTestsAsync();

            tests = tests.OrderByDescending(p => p.TimeCreated);

            return _mapper.Map<IEnumerable<TestDto>>(tests);
        }

        public virtual async Task<TestDto> AddTestAsync(string url)
        {
            if (_linkValidator.IsCorrectLink(url, out string errorMessage) == false)
            {
                throw new InvalidInputUrlException(errorMessage);
            }

            var createdTest = await _crawlerApp.StartWork(url);

            return _mapper.Map<TestDto>(createdTest);
        }

        public virtual async Task<DetailTestDto> GetTestAsync(int testId, GetTestDetailParam param)
        {
            var test = (await _dbWorker.GetTestsAsync())
                                       .First(p => p.Id == testId);

            //Get results sorted it and get needed page
            var testResults = (await _dbWorker.GetResultsAsync(test.Id))
                                              .OrderBy(p => p.TimeResponse)
                                              .AsQueryable();

            testResults = _resultFilter.Filter(testResults, param);

            test.Results = testResults.Pagination(param.Page - 1, param.CountResultsOnPage).ToList();

            return _mapper.Map<DetailTestDto>(test);
        }
    }
}
