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
	/// Renderer for MySql
	/// </summary>
	/// <remarks>
	/// Use MySqlRenderer to render SQL statements for MySql database.
	/// This version of Sql.Net has been tested with MySql 4
	/// </remarks>
	public class MySqlRenderer : SqlOmRenderer
	{
		/// <summary>
		/// Creates a new MySqlRenderer
		/// </summary>
		public MySqlRenderer() : base('`', '`') {}

		/// <summary>
		/// Renders IfNull SqlExpression
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="expr"></param>
		protected override void IfNull(StringBuilder builder, SqlExpression expr)
		{
			builder.Append("ifnull(");
			Expression(builder, expr.SubExpr1);
			builder.Append(", ");
			Expression(builder, expr.SubExpr2);
			builder.Append(")");
		}

		/// <summary>
		/// Renders a SELECT statement
		/// </summary>
		/// <param name="query">Query definition</param>
		/// <returns>Generated SQL statement</returns>
		/// <remarks>MySql 4.1 does not support GroupByWithCube option. If a query has <see cref="SelectQuery.GroupByWithCube"/> set an <see cref="InvalidQueryException"/> exception will be thrown. </remarks>
		public override string RenderSelect(SelectQuery query)
		{
			return RenderSelect(query, false, 0, query.Top);
		}

		private string RenderSelect(SelectQuery query, bool forRowCount, int offset, int limitRows)
		{
			query.Validate();

			StringBuilder selectBuilder = new StringBuilder();

			//Start the select statement
			Select(selectBuilder, query.Distinct);

			//Render select columns
			if (forRowCount)
			{
				SelectColumn(selectBuilder, new SelectColumn("*", null, "cnt", SqlAggregationFunction.Count));
			}
			else
			{
				SelectColumns(selectBuilder, query.Columns);
			}

			FromClause(selectBuilder, query.FromClause, query.TableSpace);

			Where(selectBuilder, query.WherePhrase);
			WhereClause(selectBuilder, query.WherePhrase);

			OrderBy(selectBuilder, query.OrderByTerms);
			OrderByTerms(selectBuilder, query.OrderByTerms);

			if (limitRows > -1)
			{
				selectBuilder.AppendFormat(" limit {0}, {1}", offset, limitRows);
			}

			return selectBuilder.ToString();
		}

		/*
		void RenderFromPhrase(StringBuilder builder, FromClause fromClause)
		{
			this.From(builder);
			
			this.FromTerm(builder, fromClause.BaseTable);

			foreach(Join join in fromClause.Joins)
			{
				builder.AppendFormat(" {0} join ", join.Type.ToString().ToLower());
				this.FromTerm(builder, join.RightTable);
			
				if (join.Type != JoinType.Cross)
				{
					builder.AppendFormat(" on ");
					this.QualifiedIdentifier(builder, join.LeftTable.RefName, join.LeftField);
					builder.AppendFormat(" = ");
					this.QualifiedIdentifier(builder, join.RightTable.RefName, join.RightField);
				}
			}
		}
*/

		/// <summary>
		/// Renders a row count SELECT statement. 
		/// </summary>
		/// <param name="query">Query definition to count rows for</param>
		/// <returns>Generated SQL statement</returns>
		/// <remarks>
		/// Renders a SQL statement which returns a result set with one row and one cell which contains the number of rows <paramref name="query"/> can generate. 
		/// The generated statement will work nicely with <see cref="System.Data.IDbCommand.ExecuteScalar"/> method.
		/// </remarks>
		public override string RenderRowCount(SelectQuery query)
		{
			string baseSql = RenderSelect(query);

			SelectQuery countQuery = new SelectQuery();
			SelectColumn col = new SelectColumn("*", null, "cnt", SqlAggregationFunction.Count);
			countQuery.Columns.Add(col);
			countQuery.FromClause.BaseTable = FromTerm.SubQuery(baseSql, "t");
			return RenderSelect(countQuery);
		}

		/// <summary>
		/// Renders a SELECT statement which a result-set page
		/// </summary>
		/// <param name="pageIndex">The zero based index of the page to be returned</param>
		/// <param name="pageSize">The size of a page</param>
		/// <param name="totalRowCount">Total number of rows the query would yeild if not paged</param>
		/// <param name="query">Query definition to apply paging on</param>
		/// <returns>Generated SQL statement</returns>
		/// <remarks>
		/// Parameter <paramref name="totalRowCount"/> is ignored.
		/// </remarks>
		public override string RenderPage(int pageIndex, int pageSize, int totalRowCount, SelectQuery query)
		{
			return RenderSelect(query, false, pageIndex*pageSize, pageSize);
		}
	}
}