// Point.cs - MonoWorks Project
//
//  Copyright (C) 2009 Andy Selvig
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

namespace MonoWorks.Base
{
	/// <summary>
	/// The Point class represents a point real in 3D space.
	/// The point is defined by 3 Length dimensionals.
	/// </summary>
	public class Point : MwxDummyObject, ICloneable, IStringParsable
	{
		/// <summary>
		/// Default constructor.
		/// Sets the coordinates to the origin.
		/// </summary>
		public Point()
		{
			_val = new Length[]{new Length(), new Length(), new Length()};
		}
		
		/// <summary>
		/// Initialization constructor.
		/// Uses the default units.
		/// </summary>
		/// <param name="x"> The x coordinate. </param>
		/// <param name="y"> The y coordinate. </param>
		/// <param name="z"> The z coordinate.  </param>
		public Point(double x, double y, double z)
		{
			_val = new Length[]{new Length(x), new Length(y), new Length(z)};
		}			
		
		/// <summary>
		/// Initialization constructor.
		/// </summary>
		/// <param name="x"> The x coordinate. </param>
		/// <param name="y"> The y coordinate. </param>
		/// <param name="z"> The z coordinate.  </param>
		public Point(Length x, Length y, Length z)
		{
			_val = new Length[]{x, y, z};
		}
		
		/// <summary>
		/// Initialize the point with a unitless vector.
		/// </summary>
		public Point(Vector vec)
		{
			_val = new Length[] {new Length(vec.X), new Length(vec.Y), new Length(vec.Z)};
		}
			
					
		#region Position

		/// <summary>
		/// The position of the point.
		/// </summary>
		protected Length[] _val;
		
		/// <summary>
		/// Accesses individual dimensions.
		/// </summary>
		public Length this[int index]
		{
			get
			{
				// ensure the index has appropriate range
				if (index<0 || index>2)
					throw new Exception("index is out of bounds!");					
				return _val[index];
			}
			set
			{
				// ensure the index has appropriate range
				if (index<0 || index>2)
					throw new Exception("index is out of bounds!");
				_val[index] = value;
			}
		}
				
		/// <summary>
		/// Sets the raw position.
		/// </summary>
		/// <param name="vector"></param>
		/// <remarks>This operation is slightly unsafe as it uses 
		/// the default units to convert from doubles to length.</remarks>
		public void SetPosition(Vector vector)
		{
			for (int i = 0; i < _val.Length; i++)
				_val[i].Value = vector[i];
		}
		
		/// <summary>
		/// The x value.
		/// </summary>
		[MwxProperty]
		public Length X {
			get { return _val[0]; }
			set { _val[0] = value; }
		}

		/// <summary>
		/// The x value.
		/// </summary>
		[MwxProperty]
		public Length Y {
			get { return _val[1]; }
			set { _val[1] = value; }
		}

		/// <summary>
		/// The z value.
		/// </summary>
		[MwxProperty]
		public Length Z {
			get { return _val[2]; }
			set { _val[2] = value; }
		}
		
		/// <summary>
		/// Parses a string containing a point definition of the form [x,y,z] in default units.
		/// </summary>
		public void Parse(string valString)
		{
			if (!valString.StartsWith("[") || !valString.EndsWith("]"))
				throw new Exception("Vector literals should be surrounded by []");
			var comps = valString.Substring(1, valString.Length - 2).Split(',');
			if (comps.Length != 3)
				throw new Exception("Vector literals should have 3 comma-separated components");
			for (int i = 0; i < 3; i++) {
				_val[i] = new Length(double.Parse(comps[i]));
			}
		}
		
		#endregion


		#region Operator Overloading

		/// <summary>
		/// Adds the elements of each point.
		/// </summary>
		/// <param name="lhs"> The left hand operand. </param>
		/// <param name="rhs"> The right hand operand. </param>
		/// <returns> The resulting <see cref="Point"/>. </returns>
		public static Point operator +(Point lhs, Point rhs)
		{
			return new Point(lhs[0]+rhs[0], lhs[1]+rhs[1], lhs[2]+rhs[2]);
		}
		
		/// <summary>
		/// Subtracts the elements of each point.
		/// </summary>
		/// <param name="lhs"> The left hand operand. </param>
		/// <param name="rhs"> The right hand operand. </param>
		/// <returns> The resulting <see cref="Point"/>. </returns>
		public static Point operator-(Point lhs, Point rhs)
		{
			return new Point(lhs[0]-rhs[0], lhs[1]-rhs[1], lhs[2]-rhs[2]);
		}
		
		/// <summary>
		/// Multiplies each element in point by factor and returns the result.
		/// </summary>
		/// <param name="vector"> The left hand operand. </param>
		/// <param name="factor"> The right hand operand. </param>
		/// <returns> The resulting <see cref="Point"/>. </returns>
		public static Point operator*(Point vector, double factor)
		{
			return new Point(vector[0]*factor, vector[1]*factor, vector[2]*factor);
		}
		
		/// <summary>
		/// Divides each element in point by factor and returns the result.
		/// </summary>
		/// <param name="vector"> The left hand operand. </param>
		/// <param name="factor"> The right hand operand. </param>
		/// <returns> The resulting <see cref="Point"/>. </returns>
		public static Point operator/(Point vector, double factor)
		{
			return new Point(vector[0]/factor, vector[1]/factor, vector[2]/factor);
		}
		
#endregion


		/// <summary>
		/// The magnitude of the point.
		/// </summary>
		public Length Magnitude
		{
			get
			{
				return new Length(Math.Sqrt(_val[0].Value * _val[0].Value +
					_val[1].Value * _val[1].Value + _val[2].Value * _val[2].Value));
			}
		}

		/// <summary>
		/// Converts the point to a dimensionless vector.
		/// </summary>
		public Vector ToVector()
		{
			return new Vector(_val[0].Value, _val[1].Value, _val[2].Value);
		}

		/// <summary>
		/// Overriden to just display the vector values.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return ToVector().ToString();
		}
		
		/// <summary>
		/// Creates a clone of the point.
		/// </summary>
		public object Clone()
		{
			Point point = new Point();
			for (int i = 0; i < _val.Length; i++)
				point._val[i] = _val[i].Clone() as Length;
			return point;
		}
		
	}
}
