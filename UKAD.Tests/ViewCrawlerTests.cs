using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using UKAD.Logic.Crawlers;
using UKAD.Logic.Filters;
using UKAD.Logic.Services;

namespace UKAD.Tests
{
    class ViewCrawlerTests
    {
        ViewCrawler    ViewCrawler;
        Mock<RequestService> mockRequestService;
        Mock<RequestService> mockRequestService

        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void ViewCrawler_EmptyString_EmptyResult()
        {
            //Arrange
            Mock<LinkProcessing> mockLinkProcessing = new Mock<LinkProcessing>();
            Mock<RequestService> mockRequestService = new Mock<RequestService>();
            Mock<RequestService> mockRequestService = new Mock<LinkFilter>();

            mockLinkFilter.Setup(p => p.IsCorrectLink(It.IsAny<string>())).Returns(false);

            ViewCrawler = new ViewCrawler(mockRequestService.Object, mockLinkProcessing.Object, mockLinkFilter.Object);

            //Act
            var result = ViewCrawler.GetViewLinks("");

            //Assert
            Assert.AreEqual(result.Count(), 0);
        }
        
    }
}
