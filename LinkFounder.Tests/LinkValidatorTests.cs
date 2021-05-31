using LinkFounder.Logic.Validators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkFounder.Logic.Tests
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
            var result = linkValidator.IsCorrectLink("");

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsCorrectLink_OnlyProtocol_False()
        {
            //Act
            var result = linkValidator.IsCorrectLink("https://");

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsCorrectLink_ProtocolAndDot_False()
        {
            //Act
            var result = linkValidator.IsCorrectLink("https://.");

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsCorrectLink_ProtocolAndWwwDot_False()
        {
            //Act
            var result = linkValidator.IsCorrectLink("https://www.");

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsCorrectLink_WithOutDomain_False()
        {
            //Act
            var result = linkValidator.IsCorrectLink("https://www.example");

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsCorrectLink_CorrectLink_True()
        {
            //Act
            var result = linkValidator.IsCorrectLink("https://www.example.com/");

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


    }
}
