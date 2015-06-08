using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PGK.Extensions.Tests
{
	[TestClass]
	public class DirectoryInfoExtensionsTest
	{
		[TestMethod]
        [DeploymentItem(@"..\..\FilesForTestingPurpose", @".\FilesForTestingPurpose")]
		public void GetFiles()
		{
            var testValue = new DirectoryInfo(@".\FilesForTestingPurpose");
			var results = testValue.GetFiles("*.sln", "*.suo");
			Assert.IsNotNull(results);

			Assert.AreEqual(3, results.Length);
			Assert.AreEqual(1, results.Count(fi => fi.Name == "PGK.Extensions.sln"));
			Assert.AreEqual(1, results.Count(fi => fi.Name == "PGK.Extensions.sln.docstates.suo"));
			Assert.AreEqual(1, results.Count(fi => fi.Name == "PGK.Extensions.suo"));
		}

		[TestMethod]
		public void FindFileRecursive_pattern()
		{
			var testValue = new DirectoryInfo(@"..\..\..\PGK.Extensions");		// that should point to the rott "dnpextensions" directory
			var results = testValue.FindFileRecursive("AssemblyInfo.*");
			Assert.IsNotNull(results);
			Assert.AreEqual("AssemblyInfo.cs", results.Name);
			Assert.IsTrue(results.FullName.EndsWith(@"\Properties\AssemblyInfo.cs"));
		}
		[TestMethod]
		public void FindFileRecursive_Predicate()
		{
			var testValue = new DirectoryInfo(@"..\..\..\PGK.Extensions");		// that should point to the rott "dnpextensions" directory
			var results = testValue.FindFileRecursive(fi=> fi.Name.StartsWith("AssemblyInfo"));
			Assert.IsNotNull(results);
			Assert.AreEqual("AssemblyInfo.cs", results.Name);
			Assert.IsTrue(results.FullName.EndsWith(@"\Properties\AssemblyInfo.cs"));
		}

		[TestMethod]
		public void FindFilesRecursive_pattern()
		{
			var testValue = new DirectoryInfo(@"..\..\..\PGK.Extensions");		// that should point to the rott "dnpextensions" directory
			var results = testValue.FindFilesRecursive("AssemblyInfo.*");
			Assert.IsNotNull(results);
			Assert.AreEqual(1,results.Length);
			var fi = results.First();
			Assert.AreEqual("AssemblyInfo.cs", fi.Name);
			Assert.IsTrue(fi.FullName.EndsWith(@"\Properties\AssemblyInfo.cs"));
		}
		[TestMethod]
		public void FindFilesRecursive_Predicate()
		{
			var testValue = new DirectoryInfo(@"..\..\..\PGK.Extensions");		// that should point to the rott "dnpextensions" directory
			var results = testValue.FindFilesRecursive(fi=> fi.Name.StartsWith("AssemblyInfo"));
			Assert.IsNotNull(results);
			Assert.AreEqual(1,results.Length);
			var f1 = results.First();
			Assert.AreEqual("AssemblyInfo.cs", f1.Name);
			Assert.IsTrue(f1.FullName.EndsWith(@"\Properties\AssemblyInfo.cs"));
		}

	}
}
