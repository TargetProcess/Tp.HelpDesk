// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Text;

namespace Hd.QueryExtensions
{
	/// <summary>
	/// A collection of elements of type OrderByTerm
	/// </summary>
	[System.Serializable]
	public class OrderByTermCollection : System.Collections.CollectionBase
	{
		/// <summary>
		/// Initializes a new empty instance of the OrderByTermCollection class.
		/// </summary>
		public OrderByTermCollection()
		{
			// empty
		}

		/// <summary>
		/// Initializes a new instance of the OrderByTermCollection class, containing elements
		/// copied from an array.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the new OrderByTermCollection.
		/// </param>
		public OrderByTermCollection(OrderByTerm[] items)
		{
			AddRange(items);
		}

		/// <summary>
		/// Initializes a new instance of the OrderByTermCollection class, containing elements
		/// copied from another instance of OrderByTermCollection
		/// </summary>
		/// <param name="items">
		/// The OrderByTermCollection whose elements are to be added to the new OrderByTermCollection.
		/// </param>
		public OrderByTermCollection(OrderByTermCollection items)
		{
			AddRange(items);
		}

		/// <summary>
		/// Adds the elements of an array to the end of this OrderByTermCollection.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the end of this OrderByTermCollection.
		/// </param>
		public virtual void AddRange(OrderByTerm[] items)
		{
			foreach (OrderByTerm item in items)
			{
				List.Add(item);
			}
		}

		/// <summary>
		/// Adds the elements of another OrderByTermCollection to the end of this OrderByTermCollection.
		/// </summary>
		/// <param name="items">
		/// The OrderByTermCollection whose elements are to be added to the end of this OrderByTermCollection.
		/// </param>
		public virtual void AddRange(OrderByTermCollection items)
		{
			foreach (OrderByTerm item in items)
			{
				List.Add(item);
			}
		}

		/// <summary>
		/// Adds an instance of type OrderByTerm to the end of this OrderByTermCollection.
		/// </summary>
		/// <param name="value">
		/// The OrderByTerm to be added to the end of this OrderByTermCollection.
		/// </param>
		public virtual void Add(OrderByTerm value)
		{
			List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic OrderByTerm value is in this OrderByTermCollection.
		/// </summary>
		/// <param name="value">
		/// The OrderByTerm value to locate in this OrderByTermCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this OrderByTermCollection;
		/// false otherwise.
		/// </returns>
		public virtual bool Contains(OrderByTerm value)
		{
			return List.Contains(value);
		}

		/// <summary>
		/// Return the zero-based index of the first occurrence of a specific value
		/// in this OrderByTermCollection
		/// </summary>
		/// <param name="value">
		/// The OrderByTerm value to locate in the OrderByTermCollection.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of the _ELEMENT value if found;
		/// -1 otherwise.
		/// </returns>
		public virtual int IndexOf(OrderByTerm value)
		{
			return List.IndexOf(value);
		}

		/// <summary>
		/// Inserts an element into the OrderByTermCollection at the specified index
		/// </summary>
		/// <param name="index">
		/// The index at which the OrderByTerm is to be inserted.
		/// </param>
		/// <param name="value">
		/// The OrderByTerm to insert.
		/// </param>
		public virtual void Insert(int index, OrderByTerm value)
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Gets or sets the OrderByTerm at the given index in this OrderByTermCollection.
		/// </summary>
		public virtual OrderByTerm this[int index]
		{
			get { return (OrderByTerm) List[index]; }
			set { List[index] = value; }
		}

		/// <summary>
		/// Removes the first occurrence of a specific OrderByTerm from this OrderByTermCollection.
		/// </summary>
		/// <param name="value">
		/// The OrderByTerm value to remove from this OrderByTermCollection.
		/// </param>
		public virtual void Remove(OrderByTerm value)
		{
			List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by OrderByTermCollection.GetEnumerator.
		/// </summary>
		public class Enumerator : System.Collections.IEnumerator
		{
			private readonly System.Collections.IEnumerator wrapped;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="collection"></param>
			public Enumerator(OrderByTermCollection collection)
			{
				wrapped = ((System.Collections.CollectionBase) collection).GetEnumerator();
			}

			/// <summary>
			/// 
			/// </summary>
			public OrderByTerm Current
			{
				get { return (OrderByTerm) (wrapped.Current); }
			}

			object System.Collections.IEnumerator.Current
			{
				get { return (OrderByTerm) (wrapped.Current); }
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
		/// Returns an enumerator that can iterate through the elements of this OrderByTermCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new virtual OrderByTermCollection.Enumerator GetEnumerator()
		{
			return new OrderByTermCollection.Enumerator(this);
		}
	}
}