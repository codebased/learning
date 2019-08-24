using Learning.Heap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningTests
{
    [TestClass]
    public class MinHeapTests
    {
        [TestMethod]
        public void Test()
        {
            var sut = new MinHeap();
            sut.Add(20);
            sut.Add(1);

            var result = sut.Remove();
            Assert.AreEqual(1, result);
        }
    }
}
