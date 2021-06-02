using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkFinder.DbSaver.Models;
using System.Data;
using LinkFinder.Logic.Models;

namespace LinkFinder.DbSaver
{
    //using for save test and results
    public class DbWorker
    {
        private readonly IRepository<Test> _testsRepository;
        private readonly IRepository<Result> _resultRepository;
        public DbWorker(IRepository<Test> testRepository, IRepository<Result> resultRepository)
        {
            _testsRepository = testRepository;
            _resultRepository = resultRepository;
        }

        public async Task Save(string url, IEnumerable<Link> links)
        {
            var test = new Test() { Url = url };

            await _testsRepository.AddAsync(test);
            await _testsRepository.SaveChangesAsync();

            var results = links.Select(p => new Result() { Url = p.Url, TimeResponse = p.TimeResponse, TestId = test.Id });

            _resultRepository.AddRange(results);
            await _resultRepository.SaveChangesAsync();
        }
    }
}
