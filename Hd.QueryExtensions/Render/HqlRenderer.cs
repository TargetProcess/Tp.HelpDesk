// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Text;

namespace Hd.QueryExtensions.Render
{
	public class HqlRenderer : SqlOmRenderer
	{
		public HqlRenderer() : base(' ', ' ') {}

		/// <summary>
		/// Renders a SELECT statement
		/// </summary>
		/// <param name="query">Query definition</param>
		/// <returns>Generated SQL statement</returns>
		public override string RenderSelect(SelectQuery query)
		{
			return RenderSelect(query, true);
		}

		/// <summary>
		/// Renders IfNull SqlExpression
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="expr"></param>
		protected override void IfNull(StringBuilder builder, SqlExpression expr)
		{
			builder.Append("isnull(");
			Expression(builder, expr.SubExpr1);
			builder.Append(", ");
			Expression(builder, expr.SubExpr2);
			builder.Append(")");
		}

		protected override string Prealias
		{
			get { return "as"; }
		}

		/// <summary>
		/// Renders SqlExpression
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="expr"></param>
		protected override void Expression(StringBuilder builder, SqlExpression expr)
		{
			SqlExpressionType type = expr.Type;
			if (type == SqlExpressionType.Field)
			{
				QualifiedIdentifier(builder, expr.TableAlias, expr.Value.ToString());
			}
			else if (type == SqlExpressionType.Function)
			{
				Function(builder, expr.AggFunction, expr.SubExpr1);
			}
			else if (type == SqlExpressionType.Constant)
			{
				Constant(builder, (SqlConstant) expr.Value);
			}
			else if (type == SqlExpressionType.SubQueryText)
			{
				builder.AppendFormat("({0})", (string) expr.Value);
			}
			else if (type == SqlExpressionType.SubQueryObject)
			{
				builder.AppendFormat("({0})", RenderSelect((SelectQuery) expr.Value));
			}
			else if (type == SqlExpressionType.PseudoField)
			{
				builder.AppendFormat("{0}", (string) expr.Value);
			}
			else if (type == SqlExpressionType.Parameter)
			{
				builder.AppendFormat("{0}", (string) expr.Value);
			}
			else if (type == SqlExpressionType.LikeExpressionParameter)
			{
				builder.AppendFormat("'%' + {0} + '%'", (string) expr.Value);
			}
			else if (type == SqlExpressionType.Raw)
			{
				builder.AppendFormat("{0}", (string) expr.Value);
			}
			else if (type == SqlExpressionType.IfNull)
			{
				IfNull(builder, expr);
			}
			else if (type == SqlExpressionType.Null)
			{
				builder.Append("null");
			}
			else
			{
				throw new InvalidQueryException("Unkown expression type: " + type.ToString());
			}
		}

		public override string RenderPage(int pageIndex, int pageSize, int totalRowCount, SelectQuery query)
		{
			throw new Exception("Not supported method");
		}

		private string RenderSelect(SelectQuery query, bool renderOrderBy)
		{
			query.Validate();

			StringBuilder selectBuilder = new StringBuilder();

			if (query.Columns.Count > 0)
			{
				//Start the select statement
				Select(selectBuilder, query.Distinct);
			}

			//Render Top clause
			//if (query.Top > -1)
			//    selectBuilder.AppendFormat("top {0} ", query.Top);

			//Render select columns
			SelectColumns(selectBuilder, query.Columns);

			FromClause(selectBuilder, query.FromClause, query.TableSpace);

			Where(selectBuilder, query.WherePhrase);
			WhereClause(selectBuilder, query.WherePhrase);

			GroupBy(selectBuilder, query.GroupByTerms);
			GroupByTerms(selectBuilder, query.GroupByTerms);

			if (renderOrderBy)
			{
				OrderBy(selectBuilder, query.OrderByTerms);
				OrderByTerms(selectBuilder, query.OrderByTerms);
			}

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
			SelectQuery queryAddition = query.Clone();
			queryAddition.Columns.Clear();

			SelectColumn col = new SelectColumn("*", null, "", SqlAggregationFunction.Count);
			queryAddition.Columns.Add(col);

			queryAddition.OrderByTerms.Clear();

			return RenderSelect(queryAddition);
		}
	}
}