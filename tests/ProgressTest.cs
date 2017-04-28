namespace Delegating.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static Delegate;

    [TestClass]
    public class ProgressTest
    {
        [TestMethod]
        public void ThrowsWithNullDelegatee()
        {
            var e = Assert.ThrowsException<ArgumentNullException>(() => Progress<int>(null));
            Assert.AreEqual("delegatee", e.ParamName);
        }

        [TestMethod]
        public void CallsDelegateeOnReport()
        {
            string arg = null;
            const string message = "done";
            Progress((string value) => arg = value).Report(message);
            Assert.AreEqual(message, arg);
        }
    }
}