// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Text;

namespace Hd.QueryExtensions.Render
{
	/// <summary>
	/// Defines methods common to all SqlOM renderers.
	/// </summary>
	/// <remarks>
	/// Derive from <see cref="ISqlOmRenderer"/> when you wish to develop a brand new renderer. 
	/// You can write 100% proprietery code for while implementing the interface methods but it is not advised.
	/// Instead you can inherit the <see cref="SqlOmRenderer"/> class which implements 80-95% of your rendering functionality.
	/// All renderers must return a string as their rendering result.
	/// </remarks>
	public interface ISqlOmRenderer
	{
		/// <summary>
		/// Sets or returns default date format for the database
		/// </summary>
		/// <remarks>
		/// Set DateFormat property when your database is configured to use a different date format
		/// then "yyyy-MM-dd HH:mm:ss". SqlServer and MySql are configured to this format by default.
		/// Oracle's default date format is "dd-MMM-yy HH:mm:ss".
		/// </remarks>
		string DateFormat { get; set; }

		/// <summary>
		/// Renders a SELECT statement
		/// </summary>
		/// <param name="query">Query definition</param>
		/// <returns>Generated SQL statement</returns>
		string RenderSelect(SelectQuery query);

		/// <summary>
		/// Renders a row count SELECT statement. 
		/// </summary>
		/// <param name="query">Query definition to count rows for</param>
		/// <returns>Generated SQL statement</returns>
		/// <remarks>
		/// Renders a SQL statement which returns a result set with one row and one cell which contains the number of rows <paramref name="query"/> can generate. 
		/// The generated statement will work nicely with <see cref="System.Data.IDbCommand.ExecuteScalar"/> method.
		/// </remarks>
		string RenderRowCount(SelectQuery query);

		/// <summary>
		/// Renders a paged SELECT statement
		/// </summary>
		/// <param name="pageIndex">The zero based index of the page to be returned</param>
		/// <param name="pageSize">The size of a page</param>
		/// <param name="totalRowCount">Total number of rows the query would yeild if not paged</param>
		/// <param name="query">Query definition to apply paging on</param>
		/// <returns>Generated SQL statement</returns>
		/// <remarks>
		/// Generating pagination SQL is different on different databases because every database offers different levels of support for such functioanality.
		/// Some databases (SqlServer 2000) require the programmer to supply the total number of rows to produce a page. If your renderer does not use the totalRowCount parameter, please state so in your implementation documentation.
		/// </remarks>
		string RenderPage(int pageIndex, int pageSize, int totalRowCount, SelectQuery query);
	}
}