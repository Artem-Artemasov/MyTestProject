using NUnit.Framework;
using UKAD.Repository;
using UKAD.Models;
using UKAD.Enums;
using System.Linq;


namespace UKAD.Tests
{
    // Не забыть написать такс на изменение коллекции из вне
    public class LinkRepositoryTests
    {
        LinkRepository linkRepository;
        [SetUp]
        public void SetUp()
        {
            linkRepository = new LinkRepository();
        }

        [Test]
        public void IsProcessing_EmptyString_ReturnFalse() 
        {
            //Act
            var result = linkRepository.IsProcessing(new Link("", LocationUrl.NotFound));

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Add_CorrectLink_ShouldAddIt()
        {
            //Arrange
            var link = new Link("https://wwww.ukad-group.com/", LocationUrl.InView, 100);

            //Act
            linkRepository.AddAsync(link).Wait();

            //Assert
            Assert.IsTrue(linkRepository.GetAllLinksAsync().Result.ToList().Count == 1);

        }

        [Test]
        public void Add_NewCorrectLink_ShouldAddAsNew()
        {
            //Arrange
            var link = new Link("https://wwww.ukad-group.com/", LocationUrl.InView);

            //Act
            var result = linkRepository.AddAsync(link).Result;

            //Assert
            Assert.AreEqual(result, AddState.AddAsNew);
        }

        [Test]
        public void Add_ExitsLinkWithotTime_AddStateWithOutTime()
        {
            //Arrange
            var link = new Link("https://wwww.ukad-group.com/", LocationUrl.InView);

            //Act
            linkRepository.AddAsync(link).Wait();
            var result = linkRepository.AddAsync(link).Result;

            //Assert
            Assert.AreEqual(result, AddState.ExistWithoutTime);
        }

        [Test]
        public void Add_LinkExistAndHaveDifferentLocation_AddStateAsAllLocation()
        {
            //Arrange
            var link = new Link("https://wwww.ukad-group.com/", LocationUrl.InView);
            linkRepository.AddAsync(link).Wait();
            link.Url = "";

            //Act
            var f = linkRepository.GetAllLinksAsync().Result.First();
            //Assert
            Assert.AreEqual(f.Url,"");
        }
    }
}