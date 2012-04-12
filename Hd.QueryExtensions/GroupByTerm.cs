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
	/// Represents one term in a GROUP BY clause
	/// </summary>
	/// <remarks>
	/// Use OrderByTerm to specify how rows of a result-set should be grouped. 
	/// Please note that when you use GROUP BY, your SELECT statement can only include columns which are specified in the GROUP BY clause and aggregation columns.
	/// </remarks>
	[Serializable]
	public class GroupByTerm
	{
		private string field;
		private FromTerm table;

		public GroupByTerm() {}

		/// <summary>
		/// Creates a GROUP BY term with field name and table alias
		/// </summary>
		/// <param name="field">Name of a field to group by</param>
		/// <param name="table">The table this field belongs to</param>
		public GroupByTerm(string field, FromTerm table)
		{
			this.field = field;
			this.table = table;
		}

		/// <summary>
		/// Creates a GROUP BY term with field name and no FromTerm alias
		/// </summary>
		/// <param name="field">Name of a field to group by</param>
		public GroupByTerm(string field) : this(field, null) {}

		/// <summary>
		/// Gets the name of a field to group by
		/// </summary>
		public string Field
		{
			get { return field; }
			set { field = value; }
		}

		/// <summary>
		/// Gets the table the field belongs to
		/// </summary>
		public FromTerm Table
		{
			get { return table; }
			set { table = value; }
		}

		/// <summary>
		/// Gets the table alias for this GroupByTerm
		/// </summary>
		/// <remarks>
		/// Gets the name of a FromTerm the field specified by <see cref="GroupByTerm.Field">Field</see> property.
		/// </remarks>
		internal string TableAlias
		{
			get { return (table == null) ? null : table.RefName; }
		}
	}
}