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
	/// A collection of elements of type Parameter
	/// </summary>
	[System.Serializable]
	public class ParameterCollection : CollectionBase
	{
		/// <summary>
		/// Initializes a new empty instance of the ParameterCollection class.
		/// </summary>
		public ParameterCollection()
		{
			// empty
		}

		/// <summary>
		/// Initializes a new instance of the ParameterCollection class, containing elements
		/// copied from an array.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the new ParameterCollection.
		/// </param>
		public ParameterCollection(Parameter[] items)
		{
			AddRange(items);
		}

		/// <summary>
		/// Initializes a new instance of the ParameterCollection class, containing elements
		/// copied from another instance of ParameterCollection
		/// </summary>
		/// <param name="items">
		/// The ParameterCollection whose elements are to be added to the new ParameterCollection.
		/// </param>
		public ParameterCollection(ParameterCollection items)
		{
			AddRange(items);
		}

		/// <summary>
		/// Adds the elements of an array to the end of this ParameterCollection.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the end of this ParameterCollection.
		/// </param>
		public virtual void AddRange(Parameter[] items)
		{
			foreach (Parameter item in items)
			{
				List.Add(item);
			}
		}

		/// <summary>
		/// Adds the elements of another ParameterCollection to the end of this ParameterCollection.
		/// </summary>
		/// <param name="items">
		/// The ParameterCollection whose elements are to be added to the end of this ParameterCollection.
		/// </param>
		public virtual void AddRange(ParameterCollection items)
		{
			foreach (Parameter item in items)
			{
				List.Add(item);
			}
		}

		/// <summary>
		/// Adds an instance of type Parameter to the end of this ParameterCollection.
		/// </summary>
		/// <param name="value">
		/// The Parameter to be added to the end of this ParameterCollection.
		/// </param>
		public virtual void Add(Parameter value)
		{
			List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic Parameter value is in this ParameterCollection.
		/// </summary>
		/// <param name="value">
		/// The Parameter value to locate in this ParameterCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this ParameterCollection;
		/// false otherwise.
		/// </returns>
		public virtual bool Contains(Parameter value)
		{
			return List.Contains(value);
		}

		/// <summary>
		/// Return the zero-based index of the first occurrence of a specific value
		/// in this ParameterCollection
		/// </summary>
		/// <param name="value">
		/// The Parameter value to locate in the ParameterCollection.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of the _ELEMENT value if found;
		/// -1 otherwise.
		/// </returns>
		public virtual int IndexOf(Parameter value)
		{
			return List.IndexOf(value);
		}

		/// <summary>
		/// Inserts an element into the ParameterCollection at the specified index
		/// </summary>
		/// <param name="index">
		/// The index at which the Parameter is to be inserted.
		/// </param>
		/// <param name="value">
		/// The Parameter to insert.
		/// </param>
		public virtual void Insert(int index, Parameter value)
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Gets or sets the Parameter at the given index in this ParameterCollection.
		/// </summary>
		public virtual Parameter this[int index]
		{
			get { return (Parameter) List[index]; }
			set { List[index] = value; }
		}

		/// <summary>
		/// Removes the first occurrence of a specific Parameter from this ParameterCollection.
		/// </summary>
		/// <param name="value">
		/// The Parameter value to remove from this ParameterCollection.
		/// </param>
		public virtual void Remove(Parameter value)
		{
			List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by ParameterCollection.GetEnumerator.
		/// </summary>
		public class Enumerator : IEnumerator
		{
			private readonly IEnumerator wrapped;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="collection"></param>
			public Enumerator(ParameterCollection collection)
			{
				wrapped = ((CollectionBase) collection).GetEnumerator();
			}

			/// <summary>
			/// 
			/// </summary>
			public Parameter Current
			{
				get { return (Parameter) (wrapped.Current); }
			}

			object IEnumerator.Current
			{
				get { return (Parameter) (wrapped.Current); }
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
		/// Returns an enumerator that can iterate through the elements of this ParameterCollection.
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