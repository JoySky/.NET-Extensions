using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using System.Diagnostics;

namespace PGK.Extensions.Test4
{
    [TestClass]
    public class ConcurrentExtensionsTest
    {
        [TestMethod]
        public void ConcurrentDictionary_GetOrAddTest()
        {
            ConcurrentDictionary<int, Lazy<string>> cd = new ConcurrentDictionary<int,Lazy<string>>();

            int n = -1;
            int x = -1;
            Task[] tasks = new Task[10];
            Parallel.For(0, 10, (i) =>
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {

                    string s = cd.GetOrAdd(5000, (k) =>
                    {
                        Debug.WriteLine("Print once");
                        if (n == -1) n = i;
                        x = i;
                        Thread.Sleep(1000);
                        return "string " + k.ToString();
                    });
                });
            });

            Task.WaitAll(tasks);
            
            Assert.IsTrue(n > -1 && x > -1 && n == x);
        }

        [TestMethod]
        public void ConcurrentDictionary_AddOrUpdateTest()
        {
            ConcurrentDictionary<int, Lazy<string>> cd = new ConcurrentDictionary<int, Lazy<string>>();

            int n = 0;
            int x = -1;

            Task[] tasks = new Task[10];
            Parallel.For(0, 10, (i) =>
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {

                    string s = cd.AddOrUpdate(5000, 
                        (k) =>
                        {
                            Debug.WriteLine("Print once (task {0}): value added", i);
                            Interlocked.Increment(ref n);
                            Interlocked.Exchange(ref x, i);
                            Thread.Sleep(1000);
                            return "string " + i.ToString();
                        }, 
                        (k, v) =>
                        {
                            Debug.WriteLine("Print each (task {0}): value updated", i);
                            Assert.AreNotEqual(x, i);
                            Interlocked.Increment(ref n);
                            Interlocked.Exchange(ref x, i);
                            Thread.Sleep(100);
                            return "string " + i.ToString();
                        }
                    );
                });
            });

            Task.WaitAll(tasks);
            Assert.IsTrue(n == 10 && cd[5000].Value == string.Format("string {0}", x));
        }
    }
}
