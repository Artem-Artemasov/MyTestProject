using LinkFinder.DbWorker.Models;
using LinkFinder.WebApi.Logic.Filters;
using LinkFinder.WebApi.Logic.Request;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkFinder.WebApi.Logic.Tests.Filters
{

    public class ResultFilterTests
    {
        ResultFilter resultFilter;
        List<Result> inputResults;

        [SetUp]
        public void SetUp()
        {
            resultFilter = new ResultFilter();

            inputResults = new List<Result>
            {
                new Result(){ Id = 1, InHtml = true, InSitemap = false },
                new Result(){ Id = 2, InHtml = false, InSitemap = true },
                new Result(){ Id = 3, InHtml = true, InSitemap = true },
            };
        }

        [Test]
        public void Filter_ParamsForReturnAll_ReturnInputList()
        {
            //Arrange
            var param = new GetTestDetailParam() 
            { 
                InHtml = false,
                InSitemap = false,
                Page = 1, 
                CountResultsOnPage = 1 
            };

            //Act
            var result = resultFilter.Filter(inputResults.AsQueryable(), param);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.Any(p => p.Id == 1));
                Assert.IsTrue(result.Any(p => p.Id == 2));
                Assert.IsTrue(result.Any(p => p.Id == 3));
            });
        }

        [Test]
        public void Filter_OnlyInHtml_ShouldResultsOnlyForHtml()
        {
            //Arrange
            var param = new GetTestDetailParam()
            {
                InHtml = true,
                InSitemap = false,
                Page = 1,
                CountResultsOnPage = 1
            };

            //Act
            var result = resultFilter.Filter(inputResults.AsQueryable(), param);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(result.Count(), 1);
                Assert.IsTrue(result.Any(p => p.Id == 1));
            });
        }
    }
}
