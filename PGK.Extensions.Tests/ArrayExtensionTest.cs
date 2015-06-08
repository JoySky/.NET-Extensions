using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PGK.Extensions.Tests
{
    [TestClass]
    public class ArrayExtensionTest
    {
        #region BlockCopy

        [TestMethod]
        public void BlockCopyTest()
        {
            string[] source = new string[15];
            for (int i = 0; i < source.Length; i++)
            {
                source[i] = "string " + i.ToString();
            }

            string[] block = source.BlockCopy(0, 10);

            Assert.AreEqual(10, block.Length);

            for (int i = 0; i < block.Length; i++)
            {
                Assert.AreEqual(string.Format("string {0}", i), block[i]);
            }
        }

        [TestMethod]
        public void BlockCopyWithPadding()
        {
            string[] source = new string[15];
            for (int i = 0; i < source.Length; i++)
            {
                source[i] = "string " + i.ToString();
            }

            string[] block = source.BlockCopy(10, 10, true);

            Assert.AreEqual(10, block.Length);

            for (int i = 0; i < block.Length; i++)
            {
                if (i < 5)
                    Assert.AreEqual(string.Format("string {0}", i+10), block[i]);
                else
                    Assert.AreEqual(null, block[i]);
            }
        }

        [TestMethod]
        public void BlockCopyWithoutPadding()
        {
            string[] source = new string[15];
            for (int i = 0; i < source.Length; i++)
            {
                source[i] = "string " + i.ToString();
            }

            string[] block = source.BlockCopy(10, 10, false);

            Assert.AreEqual(5, block.Length);

            for (int i = 0; i < block.Length; i++)
            {
                if (i < 5)
                    Assert.AreEqual(string.Format("string {0}", i+10), block[i]);
                else // should never get here
                    Assert.AreEqual(null, block[i]);
            }
        }

        #endregion
    }
}
