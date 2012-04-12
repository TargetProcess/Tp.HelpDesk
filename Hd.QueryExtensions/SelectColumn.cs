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
	/// Specifies which function should be applied on a column
	/// </summary>
	public enum SqlAggregationFunction
	{
		/// <summary>No function</summary>
		None,
		/// <summary>Count rows</summary>
		Count,
		/// <summary>Sum</summary>
		Sum,
		/// <summary>Sum</summary>
		Max,
	}

	/// <summary>
	/// Describes a column of a select clause
	/// </summary>
	public class SelectColumn
	{
		private SqlExpression expr;
		private string alias;

		public SelectColumn()
			: this(string.Empty, null) {}

		/// <summary>
		/// Creates a SelectColumn with a column name, no table, no column alias and no function
		/// </summary>
		/// <param name="columnName">Name of a column</param>
		public SelectColumn(string columnName) : this(columnName, null) {}

		/// <summary>
		/// Creates a SelectColumn with a column name, table, no column alias and no function
		/// </summary>
		/// <param name="columnName">Name of a column</param>
		/// <param name="table">The table this field belongs to</param>
		public SelectColumn(string columnName, FromTerm table) : this(columnName, table, null) {}

		public SelectColumn(Enum columnName, FromTerm table)
			: this(columnName.ToString(), table, null) {}

		/// <summary>
		/// Creates a SelectColumn with a column name, table and column alias
		/// </summary>
		/// <param name="columnName">Name of a column</param>
		/// <param name="table">The table this field belongs to</param>
		/// <param name="columnAlias">Alias of the column</param>
		public SelectColumn(string columnName, FromTerm table, string columnAlias) : this(columnName, table, columnAlias, SqlAggregationFunction.None) {}

		/// <summary>
		/// Creates a SelectColumn with a column name, table, column alias and optional aggregation function
		/// </summary>
		/// <param name="columnName">Name of a column</param>
		/// <param name="table">The table this field belongs to</param>
		/// <param name="columnAlias">Alias of the column</param>
		/// <param name="function">Aggregation function to be applied to the column. Use SqlAggregationFunction.None to specify that no function should be applied.</param>
		public SelectColumn(string columnName, FromTerm table, string columnAlias, SqlAggregationFunction function)
		{
			if (function == SqlAggregationFunction.None)
			{
				expr = SqlExpression.Field(columnName, table);
			}
			else
			{
				expr = SqlExpression.Function(function, SqlExpression.Field(columnName, table));
			}
			alias = columnAlias;
		}

		public SelectColumn(Enum columnName, FromTerm table, SqlAggregationFunction function) : this(columnName.ToString(), table, function) {}

		public SelectColumn(string columnName, FromTerm table, SqlAggregationFunction function)
		{
			if (function == SqlAggregationFunction.None)
			{
				expr = SqlExpression.Field(columnName, table);
			}
			else
			{
				expr = SqlExpression.Function(function, SqlExpression.Field(columnName, table));
			}
		}

		/// <summary>
		/// Creates a SelectColumn
		/// </summary>
		/// <param name="expr">Expression</param>
		/// <param name="columnAlias">Column alias</param>
		public SelectColumn(SqlExpression expr, string columnAlias)
		{
			this.expr = expr;
			alias = columnAlias;
		}

		/// <summary>
		/// Gets the column alias for this SelectColumn
		/// </summary>
		public string ColumnAlias
		{
			get { return alias; }
			set { alias = value; }
		}

		public SqlExpression Expression
		{
			get { return expr; }
			set { expr = value; }
		}
	}
}