using NUnit.Framework;
using System.Linq;
using UKAD.Enums;
using UKAD.Services;
using System.Collections.Generic;

namespace UKAD.Tests
{
    public class RequestServiceTests
    {
        RequestService RequestService;

        [SetUp]
        public void SetUp()
        {
            RequestService = new RequestService("https://www.example.com");
        }

        [Test]
        public void CutAllUrls_EmptyString_EmptyList()
        {
            //Act
            var result = RequestService.CutAllUrls("", new Dictionary<string, string>());

            //Assert
            Assert.AreEqual(result.Count(), 0);
        }

        [Test]
        public void CutAllUrls_EmptyDelimiters_EmptyList()
        {
            //Act
            var result = RequestService.CutAllUrls("", new Dictionary<string, string>());

            //Assert
            Assert.AreEqual(result.Count(), 0);
        }

        [Test]
        public void CutAllUrls_CorrectResponseMessage_ShouldCutUrls()
        {
            //Arrange
            var responseMessage = "<a href=\"https://www.example.com/\">somethingText</a>";
            var delimiters = new Dictionary<string, string>() { { "href=\"", "\"" } };

            //Act
            var result = RequestService.CutAllUrls(responseMessage, delimiters).ToList();

            //Assert
            Assert.AreEqual(result[0], "https://www.example.com/");
        }

        [Test]
        public void ToAbsoluteUrlList_AbsolutePath_ReturnListWithItPath()
        {
            //Arrange 
            var urls = new List<string>() { "https://www.example.com" };

            //Act
            var result = RequestService.ToAbsoluteUrlList(urls, "", LocationUrl.InView).ToList();

            //Assert
            Assert.AreEqual(result[0].Url, "https://www.example.com");
        }

        [Test]
        public void ToAbsoluteUrlList_PathWithOutProtocol_AddProtocol()
        {
            //Arrange 
            var urls = new List<string>() { "//www.example.com" };

            //Act
            var result = RequestService.ToAbsoluteUrlList(urls, "", LocationUrl.InView).ToList();

            //Assert
            Assert.AreEqual(result[0].Url, "https://www.example.com");
        }

        [Test]
        public void ToAbsoluteUrlList_SimpleSlash_AddBaseUrl()
        {
            //Arrange 
            var urls = new List<string>() { "/" };

            //Act
            var result = RequestService.ToAbsoluteUrlList(urls, "", LocationUrl.InView).ToList();

            //Assert
            Assert.AreEqual(result[0].Url, "https://www.example.com");
        }

        [Test]
        public void ToAbsoluteUrlList_PathContainsPartOfRelavetilyUrl_IntersectionUrl()
        {
            //Arrange
            var urls = new List<string>() { "/books/example" };

            //Act
            var result = RequestService.ToAbsoluteUrlList(urls, "https://www.example.com/books", LocationUrl.InView).ToList();

            //Assert
            Assert.AreEqual(result[0].Url, "https://www.example.com/books/example");
        }

        [Test]
        public void ToAbsoluteUrlList_RelativePathWithRelatilyInput_SinglePath()
        {
            //Arrange
            var urls = new List<string>() { "/books/example" };

            //Act
            var result = RequestService.ToAbsoluteUrlList(urls, "https://www.example.com/", LocationUrl.InView).ToList();

            //Assert
            Assert.AreEqual(result[0].Url, "https://www.example.com/books/example");
        }

        [Test]
        public void ToAbsoluteUrlList_InputPathContainsDotSplash_SinglePath()
        {
            //Arrange
            var urls = new List<string>() { "././books/example" };

            //Act
            var result = RequestService.ToAbsoluteUrlList(urls, "https://www.example.com/shop/coffee", LocationUrl.InView).ToList();

            //Assert
            Assert.AreEqual(result[0].Url, "https://www.example.com/books/example");
        }

        [Test]
        public void ToAbsoluteUrlLis_Correcturl_ShouldSetCorrectLocation()
        {
            //Arrange
            var urls = new List<string>() { "https://www.example.com" };

            //Act
            var result = RequestService.ToAbsoluteUrlList(urls, "", LocationUrl.InView).ToList();

            //Assert
            Assert.AreEqual(result[0].LocationUrl, LocationUrl.InView);
        }

    }
}
