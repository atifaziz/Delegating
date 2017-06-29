namespace Delegating.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static Delegate;
    using static Capture;

    [TestClass]
    public class ObservableTest
    {
        [TestMethod]
        public void ThrowsWithNullDelegatee()
        {
            var e = Assert.ThrowsException<ArgumentNullException>(() => Observable<object>(null));
            Assert.AreEqual("delegatee", e.ParamName);
        }

        [TestMethod]
        public void UsesDelegatee()
        {
            var disposable = Disposable(delegate { });
            IObserver<object> observer;
            var observable = Observable<object>(o => CaptureInvocation(out observer, o, disposable));
            Assert.IsNotNull(observable);
            Assert.AreSame(disposable, observable.Subscribe(Observer<object>(delegate { })));
        }
    }
}
