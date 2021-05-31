using LinkFounder.Logic.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkFounder.Logic.Tests
{
    public class LinkConverterTests
    {
        LinkConverter LinkConverter;

        [SetUp]
        public void SetUp()
        {
            LinkConverter = new LinkConverter();
        }
        [Test]
        public void RelativeToAbsolute_AbsolutePath_ReturnListWithItPath()
        {
            //Arrange 
            var urls = new List<string>() { "https://www.example.com" };

            //Act
            var result = LinkConverter.RelativeToAbsolute(urls, "https://www.example.com/").ToList();

            //Assert
            Assert.AreEqual(result[0], "https://www.example.com");
        }

        [Test]
        public void RelativeToAbsolute_PathWithOutProtocol_AddProtocol()
        {
            //Arrange 
            var urls = new List<string>() { "//www.example.com" };

            //Act
            var result = LinkConverter.RelativeToAbsolute(urls, "https://www.example.com").ToList();

            //Assert
            Assert.AreEqual(result[0], "https://www.example.com");
        }

        [Test]
        public void RelativeToAbsolute_SimpleSlash_AddBaseUrl()
        {
            //Arrange 
            var urls = new List<string>() { "/" };

            //Act
            var result = LinkConverter.RelativeToAbsolute(urls, "https://www.example.com/").ToList();

            //Assert
            Assert.AreEqual(result[0], "https://www.example.com/");
        }

        [Test]
        public void RelativeToAbsolute_PathContainsPartOfRelavetilyUrl_IntersectionUrl()
        {
            //Arrange
            var urls = new List<string>() { "/books/example" };

            //Act
            var result = LinkConverter.RelativeToAbsolute(urls,"https://www.example.com/books/").ToList();

            //Assert
            Assert.AreEqual(result[0], "https://www.example.com/books/example");
        }

        [Test]
        public void RelativeToAbsolute_RelativePathWithRelatilyInput_SinglePath()
        {
            //Arrange
            var urls = new List<string>() { "/books/example" };

            //Act
            var result = LinkConverter.RelativeToAbsolute(urls, "https://www.example.com").ToList();

            //Assert
            Assert.AreEqual(result[0], "https://www.example.com/books/example");
        }

        [Test]
        public void RelativeToAbsolute_InputPathContainsDotSplash_SinglePath()
        {
            //Arrange
            var urls = new List<string>() { "././books/example" };

            //Act
            var result = LinkConverter.RelativeToAbsolute(urls, "https://www.example.com/shop/coffee/").ToList();

            //Assert
            Assert.AreEqual(result[0], "https://www.example.com/books/example");
        }
    }
}
