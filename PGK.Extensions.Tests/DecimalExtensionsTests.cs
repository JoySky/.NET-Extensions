using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PGK.Extensions.Tests
{
    [TestClass]
    public class DecimalExtensionsTests
    {
        [TestMethod]
        public void AbsTest()
        {
            Assert.IsTrue((-1000.5M).Abs() == 1000.5M);
            Assert.IsTrue((1000.5M).Abs() == 1000.5M);
        }

        [TestMethod]
        public void IEnumerableAbsTest()
        {
            decimal[] decimals = new decimal[] { -1000.5M, -2000.5M, 3000.5M, 4000.5M };
            foreach (decimal d in decimals.Abs())
            {
                Assert.IsTrue(d > 0);
            }
        }
    }
}
