using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using LinkFounder.Logic.Crawlers;
using LinkFounder.Logic.Services;
using LinkFounder.Logic.Validators;
using LinkFounder.Logic.Models;
using System.Net.Http;

namespace LinkFounder.Logic.Tests
{ 
    public class HtmlCrawlerTests
    {
        Mock<LinkValidator>  mockLinkValidator;
        Mock<LinkConverter>  mockLinkConverter;
        Mock<RequestService> mockRequestService;
        Mock<LinkParser> mockLinkParser;

        [SetUp]
        public void SetUp()
        {
            mockLinkValidator = new Mock<LinkValidator>();
            mockLinkConverter = new Mock<LinkConverter>();
            mockRequestService = new Mock<RequestService>();
            mockLinkParser = new Mock<LinkParser>();

            int timeResponse;
            mockRequestService.Setup(p => p.SendRequest(It.IsAny<string>(), out timeResponse))
                              .Returns(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK });

            mockLinkValidator.Setup(p => p.IsCorrectLink(It.IsAny<string>()))
                          .Returns(true);

            mockLinkValidator.Setup(p => p.IsInCurrentSite(It.IsAny<string>(), It.IsAny<string>()))
                             .Returns(true);

            mockLinkValidator.Setup(p => p.IsFileLink(It.IsAny<string>()))
                             .Returns(false);
        }

        [Test]
        public void GetLinks_EmptyString_ReturnEmptyList()
        {
            //Arrange
            mockLinkValidator
                .Setup(p => p.IsCorrectLink(""))
                .Returns(false);

            HtmlCrawler htmlCrawler = new HtmlCrawler(mockRequestService.Object, mockLinkConverter.Object, mockLinkParser.Object, mockLinkValidator.Object);

            //Act
            var result = htmlCrawler.GetLinks("");

            //Assert
            Assert.AreEqual(result.Count(), 0);
        }

        [Test]
        public void GetLinks_PageWithLinkedOtherPage_ShouldCrawItAll()
        {
            //Arrange
            mockRequestService.SetupSequence(p => p.DownloadPage(It.IsAny<Link>()))
                               .Returns("text<a href = \"https://www.example.com/afraid\"></a>link<a>text")
                               .Returns("somethingGohirtinte <a href =\"https://www.example.com/\"></a><img>");

            mockLinkParser.SetupSequence(p => p.Parse(It.IsAny<string>()))
                          .Returns(new List<string>() { "https://www.example.com/afraid" })
                          .Returns(new List<string>() { "https://www.example.com/" });

            mockLinkConverter.SetupSequence(p => p.RelativeToAbsolute(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
                             .Returns(new List<string>() { "https://www.example.com/afraid" })
                             .Returns(new List<string>() { "https://www.example.com/" });

            HtmlCrawler htmlCrawler = new HtmlCrawler(mockRequestService.Object, mockLinkConverter.Object, mockLinkParser.Object, mockLinkValidator.Object);

            //Act
            var result = htmlCrawler.GetLinks("https://www.example.com/").ToList();

            //Assert
          
            Assert.Multiple(() =>
            {
                Assert.AreEqual(result[0].Url, "https://www.example.com/");
                Assert.AreEqual(result[1].Url, "https://www.example.com/afraid");
            });
        }

        [Test]
        public void GetLinks_UnavaliablePage_ReturnEmptyList()
        {
            //Arrange
            mockLinkValidator
                .Setup(p => p.IsCorrectLink("https://example.com/"))
                .Returns(true);

            int timeResponse;
            mockRequestService.Setup(p => p.SendRequest("https://example.com/", out timeResponse))
                              .Returns(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.BadRequest });

            mockRequestService.SetupSequence(p => p.DownloadPage(It.IsAny<Link>()))
                             .Returns("");

            mockLinkConverter.Setup(p => p.RelativeToAbsolute(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
                 .Returns(new List<string> { });

            HtmlCrawler htmlCrawler = new HtmlCrawler(mockRequestService.Object, mockLinkConverter.Object, mockLinkParser.Object, mockLinkValidator.Object);

            //Act
            var result = htmlCrawler.GetLinks("https://example.com/");

            //Assert
            Assert.AreEqual(result.Count(), 0);
        }
    }
}
