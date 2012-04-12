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
	/// A collection of elements of type SelectColumn
	/// </summary>
	[Serializable]
	public class SelectColumnCollection : CollectionBase
	{
		/// <summary>
		/// Initializes a new empty instance of the SelectColumnCollection class.
		/// </summary>
		public SelectColumnCollection()
		{
			// empty
		}

		/// <summary>
		/// Initializes a new instance of the SelectColumnCollection class, containing elements
		/// copied from an array.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the new SelectColumnCollection.
		/// </param>
		public SelectColumnCollection(SelectColumn[] items)
		{
			AddRange(items);
		}

		/// <summary>
		/// Initializes a new instance of the SelectColumnCollection class, containing elements
		/// copied from another instance of SelectColumnCollection
		/// </summary>
		/// <param name="items">
		/// The SelectColumnCollection whose elements are to be added to the new SelectColumnCollection.
		/// </param>
		public SelectColumnCollection(SelectColumnCollection items)
		{
			AddRange(items);
		}

		/// <summary>
		/// Adds the elements of an array to the end of this SelectColumnCollection.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the end of this SelectColumnCollection.
		/// </param>
		public virtual void AddRange(SelectColumn[] items)
		{
			foreach (SelectColumn item in items)
			{
				List.Add(item);
			}
		}

		/// <summary>
		/// Adds the elements of another SelectColumnCollection to the end of this SelectColumnCollection.
		/// </summary>
		/// <param name="items">
		/// The SelectColumnCollection whose elements are to be added to the end of this SelectColumnCollection.
		/// </param>
		public virtual void AddRange(SelectColumnCollection items)
		{
			foreach (SelectColumn item in items)
			{
				List.Add(item);
			}
		}

		/// <summary>
		/// Adds an instance of type SelectColumn to the end of this SelectColumnCollection.
		/// </summary>
		/// <param name="value">
		/// The SelectColumn to be added to the end of this SelectColumnCollection.
		/// </param>
		public virtual void Add(SelectColumn value)
		{
			List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic SelectColumn value is in this SelectColumnCollection.
		/// </summary>
		/// <param name="value">
		/// The SelectColumn value to locate in this SelectColumnCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this SelectColumnCollection;
		/// false otherwise.
		/// </returns>
		public virtual bool Contains(SelectColumn value)
		{
			return List.Contains(value);
		}

		/// <summary>
		/// Return the zero-based index of the first occurrence of a specific value
		/// in this SelectColumnCollection
		/// </summary>
		/// <param name="value">
		/// The SelectColumn value to locate in the SelectColumnCollection.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of the _ELEMENT value if found;
		/// -1 otherwise.
		/// </returns>
		public virtual int IndexOf(SelectColumn value)
		{
			return List.IndexOf(value);
		}

		/// <summary>
		/// Inserts an element into the SelectColumnCollection at the specified index
		/// </summary>
		/// <param name="index">
		/// The index at which the SelectColumn is to be inserted.
		/// </param>
		/// <param name="value">
		/// The SelectColumn to insert.
		/// </param>
		public virtual void Insert(int index, SelectColumn value)
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Gets or sets the SelectColumn at the given index in this SelectColumnCollection.
		/// </summary>
		public virtual SelectColumn this[int index]
		{
			get { return (SelectColumn) List[index]; }
			set { List[index] = value; }
		}

		/// <summary>
		/// Removes the first occurrence of a specific SelectColumn from this SelectColumnCollection.
		/// </summary>
		/// <param name="value">
		/// The SelectColumn value to remove from this SelectColumnCollection.
		/// </param>
		public virtual void Remove(SelectColumn value)
		{
			List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by SelectColumnCollection.GetEnumerator.
		/// </summary>
		public class Enumerator : IEnumerator
		{
			private readonly IEnumerator wrapped;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="collection"></param>
			public Enumerator(SelectColumnCollection collection)
			{
				wrapped = ((CollectionBase) collection).GetEnumerator();
			}

			/// <summary>
			/// 
			/// </summary>
			public SelectColumn Current
			{
				get { return (SelectColumn) (wrapped.Current); }
			}

			object IEnumerator.Current
			{
				get { return (SelectColumn) (wrapped.Current); }
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
		/// Returns an enumerator that can iterate through the elements of this SelectColumnCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new virtual Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}
	}
}