using Learning.BoyerMooreHorspoolSearch;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LearningTests
{
    [TestClass]
    public class BadMatchTableTests
    {

        [TestMethod]
        public void BadMatchTable_Duplicate_TwoLastCharacter_MustReturnsValidResponse()
        {
            var sut = new BadMatchTable("abcdbb");
            Assert.AreEqual(4, sut.Table.Count);
            var expectedValues = new int[] { 5, 1, 3, 2 };
            int i = 0;
            foreach (var item in sut.Table)
            {
                Assert.AreEqual(expectedValues[i++], item.Value);
            }
        }

        [TestMethod]
        public void BadMatchTable_NextJumpForInvalidCharacter_should_be_pattern_length()
        {
            var sut = new BadMatchTable("abcdbb");
            Assert.AreEqual(6, sut.NextJump(' '));
        }

        [TestMethod]
        public void BadMatchTable_Duplicate_Character_MustReturnsValidResponse()
        {
            var sut = new BadMatchTable("happily");
            Assert.AreEqual(5, sut.Table.Count);
            var expectedValues = new int[] { 6, 5, 3, 2, 1 };
            int i = 0;
            foreach (var item in sut.Table)
            {
                Assert.AreEqual(expectedValues[i++], item.Value);
            }
        }
        [TestMethod]
        public void BadMatchTable_NextJumpForDuplicateMustMatch()
        {
            var sut = new BadMatchTable("happily");
            Assert.AreEqual(3, sut.NextJump('p'));
        }
        [TestMethod]
        public void BadMatchTable_NextJumpForNonMatchedCharacter_Should_return_substringlength()
        {
            var sut = new BadMatchTable("happily");
            Assert.AreEqual(7, sut.NextJump(' '));
        }
    }
}
