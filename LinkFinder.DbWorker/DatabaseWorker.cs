using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkFinder.DbWorker.Models;
using System.Data;
using LinkFinder.Logic.Models;
using LinkFinder.Logic;

namespace LinkFinder.DbWorker
{
    //using for save test and results
    public class DatabaseWorker
    {
        private readonly IRepository<Test> _testsRepository;
        private readonly IRepository<Result> _resultRepository;

        public DatabaseWorker(IRepository<Test> testRepository, IRepository<Result> resultRepository)
        {
            _testsRepository = testRepository;
            _resultRepository = resultRepository;
        }

        public virtual async Task<Test> SaveTestAsync(string url, IEnumerable<Link> htmlLinks, IEnumerable<Link> sitemapLinks)
        {
            var test = new Test() { Url = url };
            await _testsRepository.AddAsync(test);
            await _testsRepository.SaveChangesAsync();

            var allLinks = htmlLinks.Except(sitemapLinks, (x, y) => x.Url == y.Url)
                                           .Concat(sitemapLinks);

            //Create list of result object
            var allResults = allLinks.Select(p => new Result()
            {
                Url = p.Url,
                TimeResponse = p.TimeResponse,
                Test = test,
                InHtml = htmlLinks.Any(s => s.Url == p.Url),
                InSitemap = sitemapLinks.Any(s => s.Url == p.Url)
            });

            _resultRepository.AddRange(allResults);
            await _resultRepository.SaveChangesAsync();

            return test;
        }

        public virtual async Task<IQueryable<Test>> GetTestsAsync()
        {
            return _testsRepository.GetAll();
        }

        public virtual async Task<IQueryable<Result>> GetResultsAsync(int testId)
        {
            return _resultRepository.GetAll().Where(p => p.TestId == testId);
        }
    }
}
