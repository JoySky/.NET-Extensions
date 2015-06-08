using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace PGK.Extensions.Tests
{


    /// <summary>
    ///This is a test class for FileInfoExtensionsTest and is intended
    ///to contain all FileInfoExtensionsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FileInfoExtensionsTest
    {
        private const string TestDir = "C:\\PGTestFolder\\";

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            if (Directory.Exists(TestDir))
                Directory.Delete(TestDir, true);
            Directory.CreateDirectory(TestDir);
        }

        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            if (Directory.Exists(TestDir))
                Directory.Delete(TestDir, true);
        }
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///A test for Rename
        ///</summary>
        /// <remarks>
        /// 	Contributed by dkillewo, http://www.codeplex.com/site/users/view/dkillewo
        /// </remarks>
        [TestMethod()]
        public void RenameTest()
        {
            var fileName = "test_" + MethodBase.GetCurrentMethod().Name + ".txt";
            File.Create(TestDir + fileName).Close();

            var file = new FileInfo(TestDir + fileName);
            var newFileName = "newTestFile.data";
            var newFileFullName = TestDir + newFileName;

            if (File.Exists(newFileFullName))
                File.Delete(newFileFullName);
            file.Rename(newFileFullName);

            Assert.AreEqual(newFileName, file.Name);
        }

        /// <summary>
        ///A test for ChangeExtension
        ///</summary>
        /// <remarks>
        /// 	Contributed by dkillewo, http://www.codeplex.com/site/users/view/dkillewo
        /// </remarks>
        [TestMethod()]
        public void ChangeExtensionTest()
        {
            var fileName = "test_" + MethodBase.GetCurrentMethod().Name + ".txt";
            File.Create(TestDir + fileName).Close();

            var newExtension = ".data";

            var file = new FileInfo(TestDir + fileName);
            file.ChangeExtension(newExtension);

            Assert.AreEqual(newExtension, Path.GetExtension(file.FullName));
        }

        /// <summary>
        ///A test for ChangeExtensions
        ///</summary>
        /// <remarks>
        /// 	Contributed by dkillewo, http://www.codeplex.com/site/users/view/dkillewo
        /// </remarks>
        [TestMethod()]
        public void ChangeExtensionsTest()
        {
            var fileNames = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                var name = "test_" + i + "_" + MethodBase.GetCurrentMethod().Name + ".txt";
                fileNames.Add(TestDir + name);
                File.Create(TestDir + name).Close();
            }
            List<FileInfo> fileInfos = new List<FileInfo>();
            foreach (var fileName in fileNames)
                fileInfos.Add(new FileInfo(fileName));
            fileInfos.ToArray().ChangeExtensions("data");

            foreach (var fileInfo in fileInfos)
                Assert.AreEqual(".data", Path.GetExtension(fileInfo.FullName));
        }

        /// <summary>
        ///A test for CopyTo
        ///</summary>
        /// <remarks>
        /// 	Contributed by dkillewo, http://www.codeplex.com/site/users/view/dkillewo
        /// </remarks>
        [TestMethod()]
        public void CopyToTest()
        {
            var fileNames = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                var name = "test_" + i + "_" + MethodBase.GetCurrentMethod().Name + ".txt";
                fileNames.Add(TestDir + name);
                File.Create(TestDir + name).Close();
            }
            var fileInfos = new List<FileInfo>();
            foreach (var fileName in fileNames)
                fileInfos.Add(new FileInfo(fileName));

            var target = TestDir + "MoveToDir";
            Directory.CreateDirectory(target);

            fileInfos.ToArray().CopyTo(target);

            foreach (var fileInfo in fileInfos)
            {
                Assert.IsTrue(File.Exists(TestDir + fileInfo.Name));
                Assert.IsTrue(File.Exists(fileInfo.FullName));
            }
        }

        /// <summary>
        ///A test for Delete
        ///</summary>
        /// <remarks>
        /// 	Contributed by dkillewo, http://www.codeplex.com/site/users/view/dkillewo
        /// </remarks>
        [TestMethod()]
        public void DeleteTest()
        {
            var fileNames = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                var name = "test_" + i + "_" + MethodBase.GetCurrentMethod().Name + ".txt";
                fileNames.Add(TestDir + name);
                File.Create(TestDir + name).Close();
            }
            var fileInfos = new List<FileInfo>();
            foreach (var fileName in fileNames)
                fileInfos.Add(new FileInfo(fileName));
            fileInfos.ToArray().Delete();

            foreach (var fileInfo in fileInfos)
                Assert.IsFalse(File.Exists(fileInfo.FullName));
        }

        /// <summary>
        ///A test for MoveTo
        ///</summary>
        /// <remarks>
        /// 	Contributed by dkillewo, http://www.codeplex.com/site/users/view/dkillewo
        /// </remarks>
        [TestMethod()]
        public void MoveToTest()
        {
            var fileNames = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                var name = "test_" + i + "_" + MethodBase.GetCurrentMethod().Name + ".txt";
                fileNames.Add(TestDir + name);
                File.Create(TestDir + name).Close();
            }
            var fileInfos = new List<FileInfo>();
            foreach (var fileName in fileNames)
                fileInfos.Add(new FileInfo(fileName));

            var target = TestDir + "MoveToDir";
            Directory.CreateDirectory(target);

            fileInfos.ToArray().MoveTo(target);

            foreach (var fileInfo in fileInfos)
            {
                Assert.IsFalse(File.Exists(TestDir + fileInfo.Name));
                Assert.IsTrue(File.Exists(fileInfo.FullName));
            }
        }

        /// <summary>
        ///A test for RenameFileWithoutExtension
        ///</summary>
        /// <remarks>
        /// 	Contributed by dkillewo, http://www.codeplex.com/site/users/view/dkillewo
        /// </remarks>
        [TestMethod()]
        public void RenameFileWithoutExtensionTest()
        {
            var name = "test_" + MethodBase.GetCurrentMethod().Name + ".txt";
            File.Create(TestDir+name).Close();
            var newName = "renamedTest";

            var fileInfo = new FileInfo(TestDir + name);
            fileInfo.RenameFileWithoutExtension(newName);

            Assert.AreEqual(newName+".txt",fileInfo.Name);
        }

    }
}
