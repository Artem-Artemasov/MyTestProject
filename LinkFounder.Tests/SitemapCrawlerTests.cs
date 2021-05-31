using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Moq;
using NUnit.Framework;
using LinkFounder.Logic.Crawlers;
using LinkFounder.Logic.Services;
using LinkFounder.Logic.Validators;
using LinkFounder.Logic.Models;

namespace LinkFounder.Logic.Tests
{
    class SitemapCrawlerTests
    {
        Mock<LinkValidator> mockLinkValidator;
        Mock<LinkConverter> mockLinkConverter;
        Mock<RequestService> mockRequestService;
        Mock<LinkParser> mockLinkParser;

        [SetUp]
        public void SetUp()
        {
            mockLinkValidator = new Mock<LinkValidator>();
            mockLinkConverter = new Mock<LinkConverter>();
            mockRequestService = new Mock<RequestService>();
            mockLinkParser = new Mock<LinkParser>();

            mockLinkValidator.Setup(p => p.IsCorrectLink(It.IsAny<string>()))
                             .Returns(true);
            mockLinkValidator.Setup(p => p.IsInDomain(It.IsAny<string>(), It.IsAny<string>()))
                             .Returns(true);
            mockLinkValidator.Setup(p => p.IsFileLink(It.IsAny<string>()))
                             .Returns(false);
        }

        [Test]
        public void SitemapCrawler_EmptyString__ReturnEmptyList()
        {
            //Arrange
            mockLinkValidator.Setup(p => p.IsCorrectLink(""))
                             .Returns(false);
           
            mockLinkParser.Setup(p => p.Parse(It.IsAny<string>()))
                          .Returns(new List<string>());
           
            mockRequestService.Setup(p => p.DownloadPage(It.IsAny<Link>()))
                              .Returns("");
           
            mockLinkConverter.Setup(p => p.RelativeToAbsolute(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
                             .Returns(new List<string> { });
            
            int timeResponse = 1;
            mockRequestService.Setup(p => p.SendRequest("", out timeResponse)).Returns(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.BadRequest});

            SitemapCrawler sitemapCrawler = new SitemapCrawler(mockRequestService.Object, mockLinkConverter.Object, mockLinkParser.Object, mockLinkValidator.Object);

            //Act
            var result = sitemapCrawler.GetLinks("");

            //Assert
            Assert.AreEqual(result.Count(), 0);                 
        }


        [Test]
        public void SitemapCrawler_SiteWithHaveCorrectSitemap_ReturnLinksList()
        {
            //Arrange
            mockRequestService.Setup(p => p.DownloadPage(new Link("https://example.com/sitemap.xml",0)))
                              .Returns("text<loc>https://example.com/</loc>text<loc>https://example.com/afraid</loc>text");

            mockLinkParser.Setup(p => p.Parse(It.IsAny<string>()))
                          .Returns(new List<string>() { "https://example.com/", "https://example.com/afraid" });
           
            mockLinkConverter.Setup(p => p.RelativeToAbsolute(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
                             .Returns(new List<string>() { "https://example.com/", "https://example.com/afraid" });
          
            int timeResponse = 1;
            mockRequestService.Setup(p => p.SendRequest("", out timeResponse)).Returns(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK });

            SitemapCrawler sitemapCrawler = new SitemapCrawler(mockRequestService.Object, mockLinkConverter.Object, mockLinkParser.Object, mockLinkValidator.Object);

            //Act
            var result = sitemapCrawler.GetLinks("https://example.com").ToList();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(result[0].Url, "https://example.com/");
                Assert.AreEqual(result[1].Url, "https://example.com/afraid");
            });
        }
        [Test]
        public void SitemapCrawler_SiteWithNotHaveSitemap_ReturnEmptyList()
        {
            //Arrange
            mockLinkParser.Setup(p => p.Parse(It.IsAny<string>()))
                          .Returns(new List<string>());
            
            mockRequestService.Setup(p => p.DownloadPage(It.IsAny<Link>()))
                              .Returns("");
            
            mockLinkConverter.Setup(p => p.RelativeToAbsolute(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
                             .Returns(new List<string> { });
            
            int timeResponse = 1;
            mockRequestService.Setup(p => p.SendRequest("", out timeResponse)).Returns(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.BadRequest });

            SitemapCrawler sitemapCrawler = new SitemapCrawler(mockRequestService.Object, mockLinkConverter.Object, mockLinkParser.Object, mockLinkValidator.Object);

            //Act
            var result = sitemapCrawler.GetLinks("https://example.com/");

            //Assert
            Assert.AreEqual(result.Count(), 0);
        }
    }
}
