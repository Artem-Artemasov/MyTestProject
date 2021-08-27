using LinkFinder.Logic.Validators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkFinder.Logic.Tests
{
    public class LinkValidatorTests
    {
        LinkValidator linkValidator;

        [SetUp]
        public void SetUp()
        {
            linkValidator = new LinkValidator();
        }

        [Test]
        public void IsCorrectLink_EmptyString()
        {
            //Act
            var result = linkValidator.IsCorrectLink("",out string errorMessage);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result);
                Assert.AreNotEqual(errorMessage, "");
            });
        }

        [Test]
        public void IsCorrectLink_OnlyProtocol_False()
        {
            //Act
            var result = linkValidator.IsCorrectLink("https://", out string errorMessage);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result);
                Assert.AreNotEqual(errorMessage, "");
            });
        }

        [Test]
        public void IsCorrectLink_ProtocolAndWwwDot_False()
        {
            //Act
            var result = linkValidator.IsCorrectLink("https://www.", out string errorMessage);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result);
                Assert.AreNotEqual(errorMessage, "");
            });
        }

        [Test]
        public void IsCorrectLink_WithOutDomain_False()
        {
            //Act
            var result = linkValidator.IsCorrectLink("https://www.example", out string errorMessage);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsFalse(result);
                Assert.AreNotEqual(errorMessage, "");
            });
        }

        [Test]
        public void IsCorrectLink_CorrectLink_True()
        {
            //Act
            var result = linkValidator.IsCorrectLink("https://www.example.com/", out string errorMessage);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsInCurrentSite_EmtyURl_False()
        {
            //Act
            var result = linkValidator.IsInCurrentSite("","https://example.com");

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsInCurrentSite_CorrectLink_True()
        {
            //Act
            var result = linkValidator.IsInCurrentSite("https://example.com/books", "https://example.com");

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsFileLink_FileLink_True()
        {
            //Act
            var result = linkValidator.IsFileLink("https://example.com/books.exe");

            //Assert
            Assert.IsTrue(result);
        }
    }
}
