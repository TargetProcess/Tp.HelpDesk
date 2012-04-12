// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Hd.QueryExtensions
{
	/// <summary>
	/// A collection of elements of type SqlConstant
	/// </summary>
	[Serializable]
	public class SqlConstantCollection : System.Collections.CollectionBase
	{
		/// <summary>
		/// Initializes a new empty instance of the SqlConstantCollection class.
		/// </summary>
		public SqlConstantCollection()
		{
			// empty
		}

		/// <summary>
		/// Initializes a new empty instance of the SqlConstantCollection class with the specified initial capacity
		/// </summary>
		public SqlConstantCollection(int capacity)
		{
			InnerList.Capacity = capacity;
		}

		/// <summary>
		/// Initializes a new instance of the SqlConstantCollection class, containing elements
		/// copied from an array.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the new SqlConstantCollection.
		/// </param>
		public SqlConstantCollection(SqlConstant[] items)
		{
			AddRange(items);
		}

		/// <summary>
		/// Initializes a new instance of the SqlConstantCollection class, containing elements
		/// copied from another instance of SqlConstantCollection
		/// </summary>
		/// <param name="items">
		/// The SqlConstantCollection whose elements are to be added to the new SqlConstantCollection.
		/// </param>
		public SqlConstantCollection(SqlConstantCollection items)
		{
			AddRange(items);
		}

		/// <summary>
		/// Creates a SqlConstantCollection from a list of values.
		/// </summary>
		/// <remarks>
		/// The type of SqlConstant items in the collection is determined automatically.
		/// See <see cref="Add"/> method for more info.
		/// </remarks>
		/// <param name="values"></param>
		/// <returns></returns>
		public static SqlConstantCollection FromList(IList values)
		{
			SqlConstantCollection collection = new SqlConstantCollection(values.Count);
			foreach (object val in values)
			{
				collection.Add(val);
			}
			return collection;
		}

		/// <summary>
		/// Adds a value
		/// </summary>
		/// <param name="val">The value which is to be added</param>
		/// <remarks>
		/// This method automatically determins the type of the value and creates the appropriate SqlConstant object.
		/// </remarks>
		public void Add(object val)
		{
			if (val == null)
			{
				return;
			}

			SqlConstant constant;
			if (val is string)
			{
				constant = SqlConstant.String((string) val);
			}
			else if (val is int)
			{
				constant = SqlConstant.Number((int) val);
			}
			else if (val is DateTime)
			{
				constant = SqlConstant.Date((DateTime) val);
			}
			else if (val is double)
			{
				constant = SqlConstant.Number((double) val);
			}
			else if (val is float)
			{
				constant = SqlConstant.Number((double) val);
			}
			else if (val is decimal)
			{
				constant = SqlConstant.Number((decimal) val);
			}
			else
			{
				constant = SqlConstant.String(val.ToString());
			}

			Add(constant);
		}

		/// <summary>
		/// Adds the elements of an array to the end of this SqlConstantCollection.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the end of this SqlConstantCollection.
		/// </param>
		public virtual void AddRange(SqlConstant[] items)
		{
			foreach (SqlConstant item in items)
			{
				List.Add(item);
			}
		}

		/// <summary>
		/// Adds the elements of another SqlConstantCollection to the end of this SqlConstantCollection.
		/// </summary>
		/// <param name="items">
		/// The SqlConstantCollection whose elements are to be added to the end of this SqlConstantCollection.
		/// </param>
		public virtual void AddRange(SqlConstantCollection items)
		{
			foreach (SqlConstant item in items)
			{
				List.Add(item);
			}
		}

		/// <summary>
		/// Adds an instance of type SqlConstant to the end of this SqlConstantCollection.
		/// </summary>
		/// <param name="value">
		/// The SqlConstant to be added to the end of this SqlConstantCollection.
		/// </param>
		public virtual void Add(SqlConstant value)
		{
			List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic SqlConstant value is in this SqlConstantCollection.
		/// </summary>
		/// <param name="value">
		/// The SqlConstant value to locate in this SqlConstantCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this SqlConstantCollection;
		/// false otherwise.
		/// </returns>
		public virtual bool Contains(SqlConstant value)
		{
			return List.Contains(value);
		}

		/// <summary>
		/// Return the zero-based index of the first occurrence of a specific value
		/// in this SqlConstantCollection
		/// </summary>
		/// <param name="value">
		/// The SqlConstant value to locate in the SqlConstantCollection.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of the _ELEMENT value if found;
		/// -1 otherwise.
		/// </returns>
		public virtual int IndexOf(SqlConstant value)
		{
			return List.IndexOf(value);
		}

		/// <summary>
		/// Inserts an element into the SqlConstantCollection at the specified index
		/// </summary>
		/// <param name="index">
		/// The index at which the SqlConstant is to be inserted.
		/// </param>
		/// <param name="value">
		/// The SqlConstant to insert.
		/// </param>
		public virtual void Insert(int index, SqlConstant value)
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Gets or sets the SqlConstant at the given index in this SqlConstantCollection.
		/// </summary>
		public virtual SqlConstant this[int index]
		{
			get { return (SqlConstant) List[index]; }
			set { List[index] = value; }
		}

		/// <summary>
		/// Removes the first occurrence of a specific SqlConstant from this SqlConstantCollection.
		/// </summary>
		/// <param name="value">
		/// The SqlConstant value to remove from this SqlConstantCollection.
		/// </param>
		public virtual void Remove(SqlConstant value)
		{
			List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by SqlConstantCollection.GetEnumerator.
		/// </summary>
		public class Enumerator : System.Collections.IEnumerator
		{
			private readonly System.Collections.IEnumerator wrapped;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="collection"></param>
			public Enumerator(SqlConstantCollection collection)
			{
				wrapped = ((System.Collections.CollectionBase) collection).GetEnumerator();
			}

			/// <summary>
			/// 
			/// </summary>
			public SqlConstant Current
			{
				get { return (SqlConstant) (wrapped.Current); }
			}

			object System.Collections.IEnumerator.Current
			{
				get { return (SqlConstant) (wrapped.Current); }
			}

			/// <summary>
			/// 
			/// </summary>
			/// <returns></returns>
			public bool MoveNext()
			{
				return wrapped.MoveNext();
			}

			/// <summary>
			/// 
			/// </summary>
			public void Reset()
			{
				wrapped.Reset();
			}
		}

		/// <summary>
		/// Returns an enumerator that can iterate through the elements of this SqlConstantCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new virtual SqlConstantCollection.Enumerator GetEnumerator()
		{
			return new SqlConstantCollection.Enumerator(this);
		}
	}
}