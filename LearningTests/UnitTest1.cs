using Learning.Adapter;
using Learning.Strategy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LearningTests
{
    [TestClass]
    public class PatternTests
    {
        [TestMethod]
        public void StrategyPatternTest()
        {
            var strategryPattern = new StrategyMain(new AesEncryption());
            strategryPattern.StoreSecureData("data");

            strategryPattern.EncryptionStrategy = new DesEncryption();
            strategryPattern.StoreSecureData("data1");
        }

        [TestMethod]
        public void AdapterPattern()
        {
            var adapterMain = new AdapterMain();
            Assert.AreEqual("Bonjour Amit", adapterMain.Do());
        }
    }
}
