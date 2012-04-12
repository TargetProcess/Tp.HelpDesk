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
	/// Specifies how a result set should be ordered.
	/// </summary>
	public enum OrderByDirection
	{
		/// <summary>Ascending Order</summary>
		Ascending,
		/// <summary>Descending Order</summary>
		Descending
	}

	/// <summary>
	/// Represents one term in an ORDER BY clause
	/// </summary>
	/// <remarks>
	/// Use OrderByTerm to specify how a result-set should be ordered.
	/// </remarks>
	[System.Serializable]
	public class OrderByTerm
	{
		private string field;
		private FromTerm table;
		private OrderByDirection direction;

		/// <summary>
		/// Creates an ORDER BY term 
		/// </summary>
		public OrderByTerm()
		{
			field = null;
			table = null;
			direction = OrderByDirection.Ascending;
		}

		/// <summary>
		/// Creates an ORDER BY term with field name and table alias
		/// </summary>
		/// <param name="field">Name of a field to order by</param>
		/// <param name="table">The table this field belongs to</param>
		/// <param name="dir">Order by direction</param>
		public OrderByTerm(string field, FromTerm table, OrderByDirection dir)
		{
			this.field = field;
			this.table = table;
			direction = dir;
		}

		public OrderByTerm(Enum field, FromTerm table, OrderByDirection dir) : this(field.ToString(), table, dir) {}

		/// <summary>
		/// Creates an ORDER BY term with field name and no table alias
		/// </summary>
		/// <param name="field">Name of a field to order by</param>
		/// <param name="dir">Order by direction</param>
		public OrderByTerm(string field, OrderByDirection dir) : this(field, null, dir) {}

		/// <summary>
		/// Gets the direction for this OrderByTerm
		/// </summary>
		public OrderByDirection Direction
		{
			get { return direction; }
			set { direction = value; }
		}

		/// <summary>
		/// Gets the name of a field to order by
		/// </summary>
		public string Field
		{
			get { return field; }
			set { field = value; }
		}

		/// <summary>
		/// Gets the table alias for this OrderByTerm
		/// </summary>
		/// <remarks>
		/// Gets the name of a FromTerm the field specified by <see cref="OrderByTerm.Field">Field</see> property.
		/// </remarks>
		public string TableAlias
		{
			get { return (table == null) ? null : table.RefName; }
		}

		/// <summary>
		/// Returns the FromTerm associated with this OrderByTerm
		/// </summary>
		public FromTerm Table
		{
			get { return table; }
			set { table = value; }
		}
	}
}