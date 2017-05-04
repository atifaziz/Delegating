namespace Delegating.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static Delegate;

    [TestClass]
    public class EnumerableTest
    {
        [TestMethod]
        public void ThrowsWithNullDelegatee()
        {
            var e = Assert.ThrowsException<ArgumentNullException>(() => Enumerable<object>(null));
            Assert.AreEqual("delegatee", e.ParamName);
        }

        [TestMethod]
        public void UsesDelegatee()
        {
            var result = System.Linq.Enumerable.Range(0, 10).GetEnumerator();
            var enumerable = Enumerable(() => result);
            Assert.IsNotNull(enumerable);
            Assert.AreSame(result, enumerable.GetEnumerator());
        }
    }
}
