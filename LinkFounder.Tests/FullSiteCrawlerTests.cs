using LinkFounder.Logic.Crawlers;
using LinkFounder.Logic.Interfaces;
using LinkFounder.Logic.Models;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace LinkFounder.Logic.Tests
{
    public class FullSiteCrawlerTests
    {
        FullSiteCrawler fullSiteCrawler;
        Mock<ICrawler> mockHtmlCrawler;
        Mock<ICrawler> mockSitemapCrawler;

        [SetUp]
        public void SetUp()
        {
            mockHtmlCrawler = new Mock<ICrawler>();
            mockSitemapCrawler = new Mock<ICrawler>();
            fullSiteCrawler = new FullSiteCrawler(mockHtmlCrawler.Object, mockSitemapCrawler.Object);
        }

        [Test]
        public void GetLinks_EmptyBaseUrl_EmptyList()
        {
            //Arrange
            mockHtmlCrawler.Setup(p => p.GetLinks(It.IsAny<string>()))
                           .Returns(new List<Link> { });
            mockSitemapCrawler.Setup(p => p.GetLinks(It.IsAny<string>()))
                            .Returns(new List<Link> { });

            //Act
            var result = fullSiteCrawler.GetLinks("");

            //Asserrt
            Assert.AreEqual(result.Count(), 0);
        }

        [Test]
        public void GetLinks_OverlappedLists_ShouldCorrectConcatIt()
        {
            //Arrange
            mockHtmlCrawler.Setup(p => p.GetLinks(It.IsAny<string>()))
                           .Returns(new List<Link> { new Link("https://example.com/",0) });
            mockSitemapCrawler.Setup(p => p.GetLinks(It.IsAny<string>()))
                            .Returns(new List<Link> { new Link("https://example.com/"),
                                                      new Link("https://example.com/books")});
            //Act
            var result = fullSiteCrawler.GetLinks("https://example.com").ToList();

            //Arrange
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.Exists(p => p.Url == "https://example.com/"));
                Assert.IsTrue(result.Exists(p => p.Url == "https://example.com/books"));
            });
        }

        [Test]
        public void GetLinks_TwiceEnterOneSite_ShouldReturnBufferList()
        {
            //Arrange
            mockHtmlCrawler.SetupSequence(p => p.GetLinks(It.IsAny<string>()))
                           .Returns(new List<Link> { new Link("https://example.com/", 0) })
                           .Returns(new List<Link>());

            mockSitemapCrawler.SetupSequence(p => p.GetLinks(It.IsAny<string>()))
                            .Returns(new List<Link> { new Link("https://example.com/"),
                                                      new Link("https://example.com/books")})
                            .Returns(new List<Link>());
            //Act
            var result = fullSiteCrawler.GetLinks("https://example.com").ToList();

            //Arrange
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.Exists(p => p.Url == "https://example.com/"));
                Assert.IsTrue(result.Exists(p => p.Url == "https://example.com/books"));
            });
        }

        [Test]
        public void GetHtmlLinks_CorrectLink_ShouldReturnList()
        {
            //Arrange
            mockHtmlCrawler.Setup(p => p.GetLinks(It.IsAny<string>()))
                           .Returns(new List<Link> { new Link("https://example.com/", 0) });

            //Act
            var result = fullSiteCrawler.GetHtmlLinks("https://example.com").ToList();

            //Arrange
            Assert.IsTrue(result.Exists(p => p.Url == "https://example.com/"));
        }

        [Test]
        public void GeSitemapLinks_CorrectLink_ShouldReturnList()
        {
            //Arrange
            mockSitemapCrawler.Setup(p => p.GetLinks(It.IsAny<string>()))
                           .Returns(new List<Link> { new Link("https://example.com/", 0) });

            //Act
            var result = fullSiteCrawler.GetSitemapLinks("https://example.com").ToList();

            //Arrange
            Assert.IsTrue(result.Exists(p => p.Url == "https://example.com/"));
        }
    }
}
