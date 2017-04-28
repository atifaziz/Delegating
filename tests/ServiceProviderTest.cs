namespace Delegating.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static Delegate;

    [TestClass]
    public class ServiceProviderTest
    {
        [TestMethod]
        public void ThrowsWithNullDelegatee()
        {
            var e = Assert.ThrowsException<ArgumentNullException>(() => ServiceProvider(null));
            Assert.AreEqual("delegatee", e.ParamName);
        }

        [TestMethod]
        public void GetServiceWithNullServiceTypeThrows()
        {
            var serviceProvider = ServiceProvider(_ => null);
            var e = Assert.ThrowsException<ArgumentNullException>(() => serviceProvider.GetService(null));
            Assert.AreEqual("serviceType", e.ParamName);
        }

        [TestMethod]
        public void UsesDelegatee()
        {
            var arg = default(Type);
            var result = new object();
            var serviceProvider = ServiceProvider(type => Capture.CaptureInvocation(out arg, type, result));
            var serviceType = typeof(object);
            Assert.AreSame(result, serviceProvider.GetService(serviceType));
            Assert.AreSame(serviceType, arg);
        }
    }
}
