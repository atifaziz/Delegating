namespace Delegating.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static Delegate;
    using static Capture;

    [TestClass]
    public class ComparerTest
    {
        [TestMethod]
        public void ThrowsWithNullDelegatee()
        {
            var e = Assert.ThrowsException<ArgumentNullException>(() => Comparer<int>(null));
            Assert.AreEqual("comparer", e.ParamName);
        }

        [TestMethod]
        public void UsesDelegatee()
        {
            var args = default((int, int));
            const int result = 42;
            var comparer = Comparer((int a, int b) => CaptureInvocation(out args, a, b, result));
            Assert.AreEqual(result, comparer.Compare(123, 456));
            var (first, second) = args;
            Assert.AreEqual(123, first);
            Assert.AreEqual(456, second);
        }
    }
}
