namespace Delegating.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static Delegate;
    using static Capture;
    using Unit = System.ValueTuple;

    [TestClass]
    public class ObserverTest
    {
        [TestMethod]
        public void ThrowsWithNullOnNext()
        {
            var e = Assert.ThrowsException<ArgumentNullException>(() => Observer<object>(null));
            Assert.AreEqual("onNext", e.ParamName);
        }

        [TestMethod]
        public void OnNextUsesDelegatee()
        {
            object actual = null;
            var observer = Observer<object>(v => CaptureInvocation(out actual, v, default(Unit)));
            Assert.IsNotNull(observer);
            var input = new object();
            observer.OnNext(input);
            Assert.AreSame(input, actual);
        }

        [TestMethod]
        public void OnCompletedUsesDelegatee()
        {
            var called = false;
            var observer = Observer<object>(delegate { },
                onCompleted: () => { called = true; });
            Assert.IsNotNull(observer);
            observer.OnCompleted();
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void OnCompletedHarmlessWithoutDelegatee()
        {
            Observer<object>(delegate { }).OnCompleted();
        }

        [TestMethod]
        public void OnErrorUsesDelegatee()
        {
            Exception actual = null;
            var observer = Observer<object>(delegate { },
                onError: v => CaptureInvocation(out actual, v, default(Unit)));
            Assert.IsNotNull(observer);
            var input = new Exception();
            observer.OnError(input);
            Assert.AreSame(input, actual);
        }

        [TestMethod]
        public void OnErrorHarmlessWithoutDelegatee()
        {
            Observer<object>(delegate { }).OnError(new Exception());
        }
    }
}
