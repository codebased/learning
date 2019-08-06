using Learning.BoyerMooreHorspoolSearch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace LearningTests
{
    [TestClass]
    public class BoyerMooreHorspoolSearchTests
    {

        [TestMethod]
        public void BoyerMooreHorspool_MustReturn_SingleCount()
        {
            var text = "Mobile citi was happy to oblige to another request happily.".ToLower();
            var sut = new BoyerMooreHorspool(new BadMatchTable("happily"));
            var searchResult = sut.Search(text, "happily");
            Assert.AreEqual(1, searchResult.Count());
            Assert.AreEqual(51, searchResult.First().StartIndex);
        }


        [TestMethod]
        public void BoyerMooreHorspool_MustReturn_TwoCount()
        {
            var text = "great people talk only greatest thing with great people".ToLower();
            var sut = new BoyerMooreHorspool(new BadMatchTable("great"));
            var searchResult = sut.Search(text, "great");
            Assert.AreEqual(3, searchResult.Count());
            Assert.AreEqual(0, searchResult.First().StartIndex);
        }
    }
}