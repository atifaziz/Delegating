namespace Delegating.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static Delegate;

    [TestClass]
    public class DisposableTest
    {
        [TestMethod]
        public void ThrowsWithNullDelegatee()
        {
            var e = Assert.ThrowsException<ArgumentNullException>(() => Disposable(null));
            Assert.AreEqual("delegatee", e.ParamName);
        }

        [TestMethod]
        public void CallsDelegateeOnDispose()
        {
            var disposed = false;
            Disposable(() => disposed = true).Dispose();
            Assert.IsTrue(disposed);
        }

        [TestMethod]
        public void CallsDelegateeOnce()
        {
            var disposals = 0;
            var disposable = Disposable(() => disposals++);
            disposable.Dispose();
            Assert.AreEqual(1, disposals);
            disposable.Dispose();
            Assert.AreEqual(1, disposals);
        }

        [TestMethod]
        public void DelegateeErrorIsRaisedThrough()
        {
            const string error = "Error!";
            var e = Assert.ThrowsException<Exception>(() =>
                Disposable(() => throw new Exception(error)).Dispose());
            Assert.AreEqual(error, e.Message);
        }
    }
}
