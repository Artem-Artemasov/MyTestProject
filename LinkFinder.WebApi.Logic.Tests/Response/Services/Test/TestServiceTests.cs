using AutoMapper;
using LinkFinder.DbWorker;
using LinkFinder.DbWorker.Interfaces;
using LinkFinder.DbWorker.Models;
using LinkFinder.Logic.Validators;
using LinkFinder.WebApi.Logic.Errors;
using LinkFinder.WebApi.Logic.Filters;
using LinkFinder.WebApi.Logic.Response.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkFinder.WebApi.Logic.Tests.Response.Services
{
    public class TestServiceTests
    {
        Mock<ResultFilter> mockResultFilter = new Mock<ResultFilter>();
        Mock<LinkValidator> mockLinkValidator = new Mock<LinkValidator>();
        Mock<ICrawlerApp> mockCrawledApp = new Mock<ICrawlerApp>();
        Mock<IRepository<Test>> mockTestRepository = new Mock<IRepository<Test>>();
        Mock<IRepository<Result>> mockResultRepository = new Mock<IRepository<Result>>();
        Mock<DatabaseWorker> mockDbWorker;
        Mock<IMapper> mockMapper = new Mock<IMapper>();
        TestService testService;

        [SetUp]
        public void SetUp()
        {
            mockDbWorker = new Mock<DatabaseWorker>(mockTestRepository.Object, mockResultRepository.Object);
            testService = new TestService(mockCrawledApp.Object,
                                          mockLinkValidator.Object,
                                          mockDbWorker.Object,
                                          mockResultFilter.Object,
                                          mockMapper.Object);
        }

        [Test]
        public void AddTestAsync_InvalidURL_ShouldThrowException()
        {
            //Arrange
            string errorMessage;
            mockLinkValidator.Setup(p => p.IsCorrectLink(It.IsAny<string>(), out errorMessage))
                             .Returns(false);

            //Act
            var result = Assert.ThrowsAsync<InvalidInputUrlException>(() => testService.AddTestAsync("example.com"));

            //Assert
            Assert.IsNotNull(result);
        }
    }
}
