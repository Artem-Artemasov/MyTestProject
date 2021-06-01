using LinkFounder.Logic.Crawlers;
using LinkFounder.Logic.Interfaces;
using LinkFounder.Logic.Models;
using LinkFounder.Logic.Services;
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
        public void GetLinks_OverlappedLists_ShouldCorrectConcat()
        {
            //Arrange
            mockHtmlCrawler.Setup(p => p.GetLinks(It.IsAny<string>()))
                           .Returns(new List<Link> { new Link("https://example.com/",0) });
            mockSitemapCrawler.Setup(p => p.GetLinks(It.IsAny<string>()))
                            .Returns(new List<Link> { new Link("https://example.com/"),
                                                      new Link("https://example.com/books")});
            //Act
            var result = fullSiteCrawler.GetLinks("https://example.com");

            //Arrange
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(result.FirstOrDefault(p => p.Url == "https://example.com/"));
                Assert.IsNotNull(result.FirstOrDefault(p => p.Url == "https://example.com/books"));
            });
        }
    }
}
