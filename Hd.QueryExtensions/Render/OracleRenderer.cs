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
	/// Renderer for Oracle
	/// </summary>
	/// <remarks>
	/// Use OracleRenderer to render SQL statements for Oracle database.
	/// This version of Sql.Net has been tested with Oracle 9i.
	/// </remarks>
	public class OracleRenderer : SqlOmRenderer
	{
		/// <summary>
		/// Creates a new instance of OracleRenderer
		/// </summary>
		public OracleRenderer() : base('"', '"')
		{
			DateFormat = "dd-MMM-yy";
			DateTimeFormat = "dd-MMM-yy HH:mm:ss";
		}

		/// <summary>
		/// Renders IfNull SqlExpression
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="expr"></param>
		protected override void IfNull(StringBuilder builder, SqlExpression expr)
		{
			builder.Append("nvl(");
			Expression(builder, expr.SubExpr1);
			builder.Append(", ");
			Expression(builder, expr.SubExpr2);
			builder.Append(")");
		}

		/// <summary>
		/// Returns true. 
		/// </summary>
		protected override bool UpperCaseIdentifiers
		{
			get { return true; }
		}

		/// <summary>
		/// Renders bitwise and
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="term"></param>
		protected override void BitwiseAnd(StringBuilder builder, WhereTerm term)
		{
			builder.Append("BITAND(");
			Expression(builder, term.Expr1);
			builder.Append(", ");
			Expression(builder, term.Expr2);
			builder.Append(") > 0");
		}

		/// <summary>
		/// Renders a SELECT statement
		/// </summary>
		/// <param name="query">Query definition</param>
		/// <returns>Generated SQL statement</returns>
		public override string RenderSelect(SelectQuery query)
		{
			if (query.Top > -1 && query.OrderByTerms.Count > 0)
			{
				string baseSql = RenderSelect(query, -1);

				SelectQuery countQuery = new SelectQuery();
				SelectColumn col = new SelectColumn("*");
				countQuery.Columns.Add(col);
				countQuery.FromClause.BaseTable = FromTerm.SubQuery(baseSql, "t");
				return RenderSelect(countQuery, query.Top);
			}
			else
			{
				return RenderSelect(query, query.Top);
			}
		}

		private string RenderSelect(SelectQuery query, int limitRows)
		{
			query.Validate();

			StringBuilder selectBuilder = new StringBuilder();

			//Start the select statement
			Select(selectBuilder, query.Distinct);

			//Render select columns
			SelectColumns(selectBuilder, query.Columns);

			FromClause(selectBuilder, query.FromClause, query.TableSpace);

			WhereClause fullWhereClause = new WhereClause(WhereClauseRelationship.And);
			fullWhereClause.SubClauses.Add(query.WherePhrase);
			if (limitRows > -1)
			{
				fullWhereClause.Terms.Add(WhereTerm.CreateCompare(SqlExpression.PseudoField("rownum"), SqlExpression.Number(limitRows), CompareOperator.LessOrEqual));
			}

			Where(selectBuilder, fullWhereClause);
			WhereClause(selectBuilder, fullWhereClause);

			OrderBy(selectBuilder, query.OrderByTerms);
			OrderByTerms(selectBuilder, query.OrderByTerms);

			return selectBuilder.ToString();
		}

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
			string baseSql = RenderSelect(query, -1);

			SelectQuery countQuery = new SelectQuery();
			SelectColumn col = new SelectColumn("*", null, "cnt", SqlAggregationFunction.Count);
			countQuery.Columns.Add(col);
			countQuery.FromClause.BaseTable = FromTerm.SubQuery(baseSql, "t");
			return RenderSelect(countQuery);
		}
	}
}