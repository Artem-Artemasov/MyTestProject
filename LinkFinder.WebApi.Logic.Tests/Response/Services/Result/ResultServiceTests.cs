using NUnit.Framework;
using Moq;
using LinkFinder.WebApi.Logic.Response.Services;
using LinkFinder.DbWorker;
using LinkFinder.WebApi.Logic.Filters;

namespace LinkFinder.WebApi.Logic.Tests.Response.Services.Result
{
    public class ResultServiceTests
    {
        ResultService resultService;
        Mock<DatabaseWorker> mockDbWorker;

        [SetUp]
        public void SetUp()
        {
            Mock<DatabaseWorker> mockDbWorker = new Mock<DatabaseWorker>();
            Mock<ResultFilter> mockFilter = new Mock<ResultFilter>();

            resultService = new ResultService(mockDbWorker.Object, mockFilter.Object) ;
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}
