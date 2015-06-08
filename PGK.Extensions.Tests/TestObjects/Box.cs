using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGK.Extensions.Tests.TestObjects
{
	public class Box : IComparable<Box>
	{

		public Box(int h, int l, int w)
		{
			this.Height = h;
			this.Length = l;
			this.Width = w;
		}
		public int Height { get; private set; }
		public int Length { get; private set; }
		public int Width { get; private set; }
        // some extensions we're testing works for uInt or uLong and not integer.  This is the sole purpose of those properties
        public uint WidthAsUInt 
        {
            get { return this.Width.ConvertTo<uint>(); }
        }
        public uint? WidthAsNullableUInt 
        {
            get { return this.WidthAsUInt; }
        }
        public ulong WidthAsULong 
        {
            get { return this.Width.ConvertTo<ulong>(); }
        }
        public ulong? WidthAsNullableULong 
        {
            get { return this.WidthAsULong; }
        }

		public int CompareTo(Box other)
		{
			// Compares Height, Length, and Width.
			int ret = this.Height.CompareTo(other.Height);
			if (ret == 0) ret = this.Length.CompareTo(other.Length);
			if (ret == 0) ret = this.Width.CompareTo(other.Width) ;
			return ret;
		}


		public class LengthFirst : Comparer<Box>
		{
			// Compares by Length, Height, and Width.
			public override int Compare(Box x, Box y)
			{
				int ret =  x.Length.CompareTo(y.Length);
				if (ret == 0) ret = x.Height.CompareTo(y.Height);
				if (ret == 0) ret = x.Width.CompareTo(y.Width);
				return ret;
			}

		}


	}

}
