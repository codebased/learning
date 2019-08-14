using Learning.Mixin;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LearningTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var player = new DvdPlayer();
            player.PlayAudio();
            player.PlayVideo();
        }
    }
}

