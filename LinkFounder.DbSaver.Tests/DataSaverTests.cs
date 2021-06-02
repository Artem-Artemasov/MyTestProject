using LinkFounder.DbSaver.Models;
using LinkFounder.Logic.Models;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Linq;

namespace LinkFounder.DbSaver.Tests
{
    public class DataSaverTests
    {
        Mock<IRepository<Test>> mockTestRepository;
        Mock<IRepository<Result>> mockResultRepository;
        DataSaver dataSaver;

        [SetUp]
        public void SetUp()
        {
            mockTestRepository = new Mock<IRepository<Test>>();
            mockResultRepository = new Mock<IRepository<Result>>();
            dataSaver = new DataSaver(mockTestRepository.Object, mockResultRepository.Object);
        }

        [Test]
        public void Save_CorrectInputUrl_ShouldAddIt()
        {
            //Act
            dataSaver.Save("https://www.example.com", new List<Link>()).Wait();

            //Assert
            mockTestRepository.Verify(p => p.AddAsync(It.Is<Test>(p => p.Url == "https://www.example.com"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Save_CorrectLinkList_ShouldAddTest()
        {
            //Act
            dataSaver.Save("https://www.example.com", new List<Link>() { new Link("https://example.com/books")}).Wait();

            //Assert
            mockResultRepository.Verify(p => p.AddRange(It.Is<IEnumerable<Result>>(f => f.Any(result => result.Url == "https://example.com/books"))));
        }
    }
}