namespace Delegating.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static Delegate;
    using static Capture;

    [TestClass]
    public class EqualityComparerTest
    {
        [TestMethod]
        public void ThrowsWithNullForEquals()
        {
            var e = Assert.ThrowsException<ArgumentNullException>(() => EqualityComparer(null, NotImplementedFunc.Of<int, int>()));
            Assert.AreEqual("equals", e.ParamName);
        }

        [TestMethod]
        public void ThrowsWithNullForGetHashCode()
        {
            var e = Assert.ThrowsException<ArgumentNullException>(() => EqualityComparer(NotImplementedFunc.Of<int, int, bool>(), null));
            Assert.AreEqual("getHashCode", e.ParamName);
        }

        [TestMethod]
        public void UsesEquals()
        {
            var args = default((int, int));
            const bool result = false;
            var comparer = EqualityComparer((a, b) => CaptureInvocation(out args, a, b, result), NotImplementedFunc.Of<int, int>());
            Assert.AreEqual(result, comparer.Equals(123, 456));
            (var first, var second) = args;
            Assert.AreEqual(123, first);
            Assert.AreEqual(456, second);
        }

        [TestMethod]
        public void UsesGetHashCode()
        {
            var arg = default(int);
            const int result = 42;
            var comparer = EqualityComparer(getHashCode: a => CaptureInvocation(out arg, a, result),
                                            equals: NotImplementedFunc.Of<int, int, bool>());
            Assert.AreEqual(result, comparer.GetHashCode(123));
            Assert.AreEqual(123, arg);
        }
    }
}