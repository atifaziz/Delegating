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
        public void ReturnsRequestedService()
        {
            var service = new object();
            var serviceProvider = ServiceProvider(_ => service);
            Assert.AreSame(service, serviceProvider.GetService(service.GetType()));
        }
    }
}
