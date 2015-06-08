using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/// <summary>
/// 	Extension methods for the DirectoryInfo class
/// </summary>
public static class DirectoryInfoExtensions 
{
    /// <summary>
    /// 	Gets all files in the directory matching one of the several (!) supplied patterns (instead of just one in the regular implementation).
    /// </summary>
    /// <param name = "directory">The directory.</param>
    /// <param name = "patterns">The patterns.</param>
    /// <returns>The matching files</returns>
    /// <remarks>
    /// 	This methods is quite perfect to be used in conjunction with the newly created FileInfo-Array extension methods.
    /// </remarks>
    /// <example>
    /// 	<code>
    /// 		var files = directory.GetFiles("*.txt", "*.xml");
    /// 	</code>
    /// </example>
    public static FileInfo[] GetFiles(this DirectoryInfo directory, params string[] patterns) 
	{
        var files = new List<FileInfo>();
        foreach (var pattern in patterns)
            files.AddRange(directory.GetFiles(pattern));
        return files.ToArray();
    }

    /// <summary>
    /// 	Searches the provided directory recursively and returns the first file matching the provided pattern.
    /// </summary>
    /// <param name = "directory">The directory.</param>
    /// <param name = "pattern">The pattern.</param>
    /// <returns>The found file</returns>
    /// <example>
    /// 	<code>
    /// 		var directory = new DirectoryInfo(@"c:\");
    /// 		var file = directory.FindFileRecursive("win.ini");
    /// 	</code>
    /// </example>
    public static FileInfo FindFileRecursive(this DirectoryInfo directory, string pattern)
	{
        var files = directory.GetFiles(pattern);
        if (files.Length > 0)
            return files[0];

        foreach (var subDirectory in directory.GetDirectories()) 
		{
            var foundFile = subDirectory.FindFileRecursive(pattern);
            if (foundFile != null)
                return foundFile;
        }
        return null;
    }

    /// <summary>
    /// 	Searches the provided directory recursively and returns the first file matching to the provided predicate.
    /// </summary>
    /// <param name = "directory">The directory.</param>
    /// <param name = "predicate">The predicate.</param>
    /// <returns>The found file</returns>
    /// <example>
    /// 	<code>
    /// 		var directory = new DirectoryInfo(@"c:\");
    /// 		var file = directory.FindFileRecursive(f => f.Extension == ".ini");
    /// 	</code>
    /// </example>
    public static FileInfo FindFileRecursive(this DirectoryInfo directory, Func<FileInfo, bool> predicate)
	{
        foreach (var file in directory.GetFiles()) 
		{
            if (predicate(file))
                return file;
        }

        foreach (var subDirectory in directory.GetDirectories())
		{
            var foundFile = subDirectory.FindFileRecursive(predicate);
            if (foundFile != null)
                return foundFile;
        }
        return null;
    }

    /// <summary>
    /// 	Searches the provided directory recursively and returns the all files matching the provided pattern.
    /// </summary>
    /// <param name = "directory">The directory.</param>
    /// <param name = "pattern">The pattern.</param>
    /// <remarks>
    /// 	This methods is quite perfect to be used in conjunction with the newly created FileInfo-Array extension methods.
    /// </remarks>
    /// <returns>The found files</returns>
    /// <example>
    /// 	<code>
    /// 		var directory = new DirectoryInfo(@"c:\");
    /// 		var files = directory.FindFilesRecursive("*.ini");
    /// 	</code>
    /// </example>
    public static FileInfo[] FindFilesRecursive(this DirectoryInfo directory, string pattern) 
	{
        var foundFiles = new List<FileInfo>();
        FindFilesRecursive(directory, pattern, foundFiles);
        return foundFiles.ToArray();
    }

    static void FindFilesRecursive(DirectoryInfo directory, string pattern, List<FileInfo> foundFiles)
	{
        foundFiles.AddRange(directory.GetFiles(pattern));
        directory.GetDirectories().ForEach(d => FindFilesRecursive(d, pattern, foundFiles));
    }

    /// <summary>
    /// 	Searches the provided directory recursively and returns the all files matching to the provided predicate.
    /// </summary>
    /// <param name = "directory">The directory.</param>
    /// <param name = "predicate">The predicate.</param>
    /// <returns>The found files</returns>
    /// <remarks>
    /// 	This methods is quite perfect to be used in conjunction with the newly created FileInfo-Array extension methods.
    /// </remarks>
    /// <example>
    /// 	<code>
    /// 		var directory = new DirectoryInfo(@"c:\");
    /// 		var files = directory.FindFilesRecursive(f => f.Extension == ".ini");
    /// 	</code>
    /// </example>
    public static FileInfo[] FindFilesRecursive(this DirectoryInfo directory, Func<FileInfo, bool> predicate)
	{
        var foundFiles = new List<FileInfo>();
        FindFilesRecursive(directory, predicate, foundFiles);
        return foundFiles.ToArray();
    }

    static void FindFilesRecursive(DirectoryInfo directory, Func<FileInfo, bool> predicate, List<FileInfo> foundFiles)
	{
        foundFiles.AddRange(directory.GetFiles().Where(predicate));
        directory.GetDirectories().ForEach(d => FindFilesRecursive(d, predicate, foundFiles));
    }

    /// <summary>
    /// Copies the entire directory to another one
    /// </summary>
    /// <param name="sourceDirectory">The source directory.</param>
    /// <param name="targetDirectoryPath">The target directory path.</param>
    /// <returns></returns>
    public static DirectoryInfo CopyTo(this DirectoryInfo sourceDirectory, string targetDirectoryPath)
	{
        var targetDirectory = new DirectoryInfo(targetDirectoryPath);
        CopyTo(sourceDirectory, targetDirectory);
        return targetDirectory;
    }

    /// <summary>
    /// Copies the entire directory to another one
    /// </summary>
    /// <param name="sourceDirectory">The source directory.</param>
    /// <param name="targetDirectory">The target directory.</param>
    public static void CopyTo(this DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory) 
	{
        if(targetDirectory.Exists == false) targetDirectory.Create();

        foreach(var childDirectory in sourceDirectory.GetDirectories()) 
		{
            CopyTo(childDirectory, Path.Combine(targetDirectory.FullName, childDirectory.Name));
        }

        foreach(var file in sourceDirectory.GetFiles())
		{
            file.CopyTo(Path.Combine(targetDirectory.FullName, file.Name));
        }
    }
}
