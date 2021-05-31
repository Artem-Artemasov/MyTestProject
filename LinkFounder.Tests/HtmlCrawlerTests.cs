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

            mockLinkValidator.Setup(p => p.IsCorrectLink(It.IsAny<string>()))
                          .Returns(true);
            mockLinkValidator.Setup(p => p.IsInDomain(It.IsAny<string>(), It.IsAny<string>()))
                             .Returns(true);
            mockLinkValidator.Setup(p => p.IsFileLink(It.IsAny<string>()))
                             .Returns(false);
        }

        [Test]
        public void HtmlCrawler_EmptyString_ReturnEmptyList()
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
        public void HtmlCrawler_PageWithLinkedOtherPage_ShouldCrawItAll()
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

       
    }
}
