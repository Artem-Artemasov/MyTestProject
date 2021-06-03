using LinkFinder.DbWorker.Models;
using LinkFinder.Logic.Models;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Linq;

namespace LinkFinder.DbWorker.Tests
{
    public class DatabaseWorkerTests
    {
        Mock<IRepository<Test>> mockTestRepository;
        Mock<IRepository<Result>> mockResultRepository;
        DatabaseWorker dataSaver;

        [SetUp]
        public void SetUp()
        {
            mockTestRepository = new Mock<IRepository<Test>>();
            mockResultRepository = new Mock<IRepository<Result>>();
            dataSaver = new DatabaseWorker(mockTestRepository.Object, mockResultRepository.Object);
        }

        [Test]
        public void Save_CorrectInputUrl_ShouldAddIt()
        {
            //Act
            dataSaver.SaveAsync("https://www.example.com", new List<Link>(),new List<Link>()).Wait();

            //Assert
            mockTestRepository.Verify(p => p.AddAsync(It.Is<Test>(p => p.Url == "https://www.example.com"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Save_CorrectLinkList_ShouldAddTest()
        {
            //Arrange
            var inputList = new List<Link>() { new Link("https://example.com/books") };

            //Act
            dataSaver.SaveAsync("https://www.example.com", inputList, inputList).Wait();

            //Assert
            mockResultRepository.Verify(p => p.AddRange(It.Is<IEnumerable<Result>>(f => f.Any(result => result.Url == "https://example.com/books"))));
        }

        
        [Test]
        public void Save_LinksInHtmlAndSitemap_ShouldSetAllMatchProperties()
        {
            //Arrange
            var inputList = new List<Link>() { new Link("https://example.com/books") };

            //Act
            dataSaver.SaveAsync("https://www.example.com", inputList, inputList).Wait();

            //Assert
            mockResultRepository.Verify(p => p.AddRange(It.Is<IEnumerable<Result>>(f => f.Any(result => result.InSitemap && result.InHtml))));
        }
    }
}