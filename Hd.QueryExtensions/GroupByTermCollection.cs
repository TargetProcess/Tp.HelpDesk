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
	/// A collection of elements of type GroupByTerm
	/// </summary>
	[Serializable]
	public class GroupByTermCollection : CollectionBase
	{
		/// <summary>
		/// Initializes a new empty instance of the GroupByTermCollection class.
		/// </summary>
		public GroupByTermCollection()
		{
			// empty
		}

		/// <summary>
		/// Initializes a new instance of the GroupByTermCollection class, containing elements
		/// copied from an array.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the new GroupByTermCollection.
		/// </param>
		public GroupByTermCollection(GroupByTerm[] items)
		{
			AddRange(items);
		}

		/// <summary>
		/// Initializes a new instance of the GroupByTermCollection class, containing elements
		/// copied from another instance of GroupByTermCollection
		/// </summary>
		/// <param name="items">
		/// The GroupByTermCollection whose elements are to be added to the new GroupByTermCollection.
		/// </param>
		public GroupByTermCollection(GroupByTermCollection items)
		{
			AddRange(items);
		}

		/// <summary>
		/// Adds the elements of an array to the end of this GroupByTermCollection.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the end of this GroupByTermCollection.
		/// </param>
		public virtual void AddRange(GroupByTerm[] items)
		{
			foreach (GroupByTerm item in items)
			{
				List.Add(item);
			}
		}

		/// <summary>
		/// Adds the elements of another GroupByTermCollection to the end of this GroupByTermCollection.
		/// </summary>
		/// <param name="items">
		/// The GroupByTermCollection whose elements are to be added to the end of this GroupByTermCollection.
		/// </param>
		public virtual void AddRange(GroupByTermCollection items)
		{
			foreach (GroupByTerm item in items)
			{
				List.Add(item);
			}
		}

		/// <summary>
		/// Adds an instance of type GroupByTerm to the end of this GroupByTermCollection.
		/// </summary>
		/// <param name="value">
		/// The GroupByTerm to be added to the end of this GroupByTermCollection.
		/// </param>
		public virtual void Add(GroupByTerm value)
		{
			List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic GroupByTerm value is in this GroupByTermCollection.
		/// </summary>
		/// <param name="value">
		/// The GroupByTerm value to locate in this GroupByTermCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this GroupByTermCollection;
		/// false otherwise.
		/// </returns>
		public virtual bool Contains(GroupByTerm value)
		{
			return List.Contains(value);
		}

		/// <summary>
		/// Return the zero-based index of the first occurrence of a specific value
		/// in this GroupByTermCollection
		/// </summary>
		/// <param name="value">
		/// The GroupByTerm value to locate in the GroupByTermCollection.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of the _ELEMENT value if found;
		/// -1 otherwise.
		/// </returns>
		public virtual int IndexOf(GroupByTerm value)
		{
			return List.IndexOf(value);
		}

		/// <summary>
		/// Inserts an element into the GroupByTermCollection at the specified index
		/// </summary>
		/// <param name="index">
		/// The index at which the GroupByTerm is to be inserted.
		/// </param>
		/// <param name="value">
		/// The GroupByTerm to insert.
		/// </param>
		public virtual void Insert(int index, GroupByTerm value)
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Gets or sets the GroupByTerm at the given index in this GroupByTermCollection.
		/// </summary>
		public virtual GroupByTerm this[int index]
		{
			get { return (GroupByTerm) List[index]; }
			set { List[index] = value; }
		}

		/// <summary>
		/// Removes the first occurrence of a specific GroupByTerm from this GroupByTermCollection.
		/// </summary>
		/// <param name="value">
		/// The GroupByTerm value to remove from this GroupByTermCollection.
		/// </param>
		public virtual void Remove(GroupByTerm value)
		{
			List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by GroupByTermCollection.GetEnumerator.
		/// </summary>
		public class Enumerator : IEnumerator
		{
			private readonly IEnumerator wrapped;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="collection"></param>
			public Enumerator(GroupByTermCollection collection)
			{
				wrapped = ((CollectionBase) collection).GetEnumerator();
			}

			/// <summary>
			/// 
			/// </summary>
			public GroupByTerm Current
			{
				get { return (GroupByTerm) (wrapped.Current); }
			}

			object IEnumerator.Current
			{
				get { return (GroupByTerm) (wrapped.Current); }
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
		/// Returns an enumerator that can iterate through the elements of this GroupByTermCollection.
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