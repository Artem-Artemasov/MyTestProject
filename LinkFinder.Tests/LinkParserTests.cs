using LinkFinder.Logic.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkFinder.Logic.Tests
{
    public class LinkParserTests
    {
        LinkParser LinkParser;
        
        [SetUp]
        public void SetUp()
        {
            LinkParser = new LinkParser();
        }

        [Test]
        public void Parse_EmptyString_ReturnEmptyList()
        {
            //Act
            var result = LinkParser.Parse("");

            //Assert
            Assert.AreEqual(result.Count(), 0);
        }

        [Test]
        public void Parse_MessageWithContainsLinks_ReturnParsedLinks()
        {
            //Act
            var result = LinkParser.Parse("text<a href=\"https://www.example.com/afraid\"></a>link<a>texttext<img src=\"//www.example.com/\"></src>link<a>texttext<loc>https://example.com/books</loc>ttext")
                                   .ToList();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.Contains("https://www.example.com/afraid"));
                Assert.IsTrue(result.Contains("//www.example.com/"));
                Assert.IsTrue(result.Contains("https://example.com/books"));
            });
        }

        [Test]
        public void Parse_MessageHaveNewLineSymbol_ReturnParsedLinks()
        {
            //Act
            var result = LinkParser.Parse("text<a href=\"https://www.example.com/afraid\"></a>link<a>\ntexttext<img src=\"//www.example.com/\"></src>link<a>text\ntext<loc>https://example.com/books</loc>ttext")
                                   .ToList();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.Contains("https://www.example.com/afraid"));
                Assert.IsTrue(result.Contains("//www.example.com/"));
                Assert.IsTrue(result.Contains("https://example.com/books"));
            });
        }

        [Test]
        public void Parse_NotHaveEndTag_ReturnParsedLinks()
        {
            //Act
            var result = LinkParser.Parse("text<a href=\"https://www.example.com/afraid something text")
                                   .ToList();

            //Assert
            Assert.IsTrue(result.Contains("https://www.example.com/afraid"));
        }
    }
}
