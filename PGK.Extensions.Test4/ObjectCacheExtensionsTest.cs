using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace PGK.Extensions.Test4
{
    [TestClass]
    public class ObjectCacheExtensionsTest
    {
        [TestMethod]
        public void GetOrAddTest()
        {
            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy policy = new CacheItemPolicy();

            int n = -1;
            int x = -1;
            Task[] tasks = new Task[10];
            Parallel.For(0, 10, (i) =>
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    string s = cache.GetOrAdd(5000, (k) =>
                    {
                        Debug.WriteLine("Print once");
                        if (n == -1) n = i;
                        x = i;
                        Thread.Sleep(1000);
                        return "string " + k.ToString();
                    }, policy);
                });
            });

            Task.WaitAll(tasks);

            Assert.IsTrue(n > -1 && x > -1 && n == x);
        }
    }
}
