using System;

/// <summary>
/// 	Extension methods for byte-Arrays
/// </summary>
public static class ByteArrayExtensions
{
    /// <summary>
    /// 	Find the first occurence of an byte[] in another byte[]
    /// </summary>
    /// <param name = "buf1">the byte[] to search in</param>
    /// <param name = "buf2">the byte[] to find</param>
    /// <returns>the first position of the found byte[] or -1 if not found</returns>
    /// <remarks>
    /// 	Contributed by blaumeister, http://www.codeplex.com/site/users/view/blaumeiser
    /// </remarks>
    public static int FindArrayInArray(this byte[] buf1, byte[] buf2)
    {
        if (buf2 == null)
            throw new ArgumentNullException("buf2");

        if (buf1 == null)
            throw new ArgumentNullException("buf1");

        if (buf2.Length == 0)
            return 0;		// by definition empty sets match immediately

        int j = -1;
        int end = buf1.Length - buf2.Length;
        while ((j = Array.IndexOf(buf1, buf2[0], j + 1)) <= end && j != -1)
        {
            int i = 1;
            while (buf1[j + i] == buf2[i])
            {
                if (++i == buf2.Length)
                    return j;
            }
        }
        return -1;
    }

}
