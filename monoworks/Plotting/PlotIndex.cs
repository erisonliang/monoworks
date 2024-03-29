// PlotIndex.cs - Slate Mono Application Framework
//
//  Copyright (C) 2008 Andy Selvig
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 

using System;
using System.Collections;
using System.Collections.Generic;

namespace MonoWorks.Plotting
{
	
	/// <summary>
	/// Represents the indices of something to plot.
	/// </summary>
	public class PlotIndex : IEnumerable<int>
	{
		/// <summary>
		/// Initialization constructor.
		/// </summary>
		/// <param name="size"> The number of points in the index. </param>
		public PlotIndex(int size)
		{
			Resize(size);
		}
		

		/// <summary>
		/// Resizes the indices.
		/// </summary>
		/// <param name="size"> The new size. </param>
		/// <remarks> All values get set to true.</remarks>
		public void Resize(int size)
		{
			values = new bool[size];
			AllOn();
		}
		

		public PlotIndex Copy()
		{
			PlotIndex other = new PlotIndex(Size);
			other.values = (bool[])values.Clone();
			return other;
		}
		
		
#region The Values
		
		protected bool[] values;
		/// <value>
		/// The raw values.
		/// </value>
		public bool[] Values
		{
			get {return values;}
		}
		
		/// <value>
		/// The number of points.
		/// </value>
		public int Size
		{
			get
			{
				if (values==null)
					return 0;
				return values.Length;
			}
		}
		
		/// <summary>
		/// Get the ith value.
		/// </summary>
		/// <param name="i"> </param>
		/// <returns> Whether or not the ith point should be plotted.</returns>
		public bool GetValue(int i)
		{
			if (i < 0 || i >= Size)
				throw new Exception(String.Format("Index {0} is out of bounds.", i));
			return values[i];
		}
		
		/// <summary>
		/// Sets the ith value.
		/// </summary>
		/// <param name="i"> </param>
		/// <param name="val"> The ith value. </param>
		public void SetValue(int i, bool val)
		{
			if (i < 0 || i >= Size)
				throw new Exception(String.Format("Index {0} is out of bounds.", i));
			values[i] = val;
		}
		
		/// <summary>
		/// Indexer for the values.
		/// </summary>
		public bool this[int i]
		{
			get {return GetValue(i);}
			set {SetValue(i, value);}
		}
		
		/// <value>
		/// The number of points that are on (true).
		/// </value>
		public int NumOn
		{
			get
			{
				int numOn = 0;
				foreach (bool val in values)
				{
					if (val)
						numOn++;
				}
				return numOn;
			}
		}

		/// <summary>
		/// The first index that's set, or -1 if none are set.
		/// </summary>
		public int First
		{
			get
			{
				foreach (int i in this)
				{
					return i;
				}
				return -1;
			}
		}
		
#endregion
		
		
#region Operations
		
		/// <summary>
		/// Turns all the values to false.
		/// </summary>
		public void AllOff()
		{
			SetAll(false);
		}
		
		/// <summary>
		/// Turns all the values to true.
		/// </summary>
		public void AllOn()
		{
			SetAll(true);
		}
		
		/// <summary>
		/// Set everything to the given value.
		/// </summary>
		/// <param name="val"> </param>
		public void SetAll(bool val)
		{
			for (int i=0; i<Size; i++)
				values[i] = val;
		}
		

		/// <summary>
		/// Intersects this index with another one.
		/// </summary>
		/// <param name="other"> </param>
		public void Intersect(PlotIndex other)
		{
			for (int i=0; i<Size; i++)
			{
				values[i] = values[i] && other[i];
			}
		}
		
#endregion



#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		public IEnumerator<int> GetEnumerator()
		{
			return new PlotIndexEnum(this);
		}

#endregion

	}


	/// <summary>
	/// Enumerator for the PlotIndex.
	/// </summary>
	public class PlotIndexEnum : IEnumerator<int>
	{

		public PlotIndexEnum(PlotIndex plotIndex)
		{
			this.plotIndex = plotIndex;
		}

		private PlotIndex plotIndex;

		private int currentRow = -1;

		public int Current
		{
			get { return currentRow; }
		}

		public void Dispose()
		{
			//throw new NotImplementedException();
		}

		object System.Collections.IEnumerator.Current
		{
			get { return Current; }
		}

		public bool MoveNext()
		{
			do
				currentRow++;
			while (currentRow < plotIndex.Size && !plotIndex[currentRow]);
			if (currentRow < plotIndex.Size)
				return true;
			return false;
		}

		public void Reset()
		{
			currentRow = -1;
		}

	}

}
