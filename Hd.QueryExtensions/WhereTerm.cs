// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Text;

namespace Hd.QueryExtensions
{
	public enum WhereTermType
	{
		Compare,
		Between,
		In,
		NotIn,
		InSubQuery,
		NotInSubQuery,
		IsNull,
		IsNotNull,
		Exists,
		NotExists
	}

	/// <summary>
	/// Represents one term in a WHERE clause
	/// </summary>
	/// <remarks>
	/// <see cref="WhereTerm"/> usually consists of one or more <see cref="SqlExpression"/> objects and an a conditional operator which applies to those expressions.
	/// <see cref="WhereTerm"/> has no public constructor. Use one of the supplied static methods to create a term. 
	/// <para>
	/// Use <see cref="WhereTerm.CreateCompare"/> to create a comparison term. A comparison term can apply one of <see cref="CompareOperator"/> operators on the supplied expressions.
	/// Use <see cref="WhereTerm.CreateIn"/> to create a term which checks wheather an expression exists in a list of supplied values.
	/// Use <see cref="WhereTerm.CreateBetween"/> to create a term which checks wheather an expression value is between a supplied lower and upper bounds.
	/// </para>
	/// </remarks>
	[Serializable()]
	public class WhereTerm : ICloneable
	{
		private SqlExpression expr1, expr2, expr3;
		private CompareOperator op;
		private WhereTermType type;
		private SqlConstantCollection values;
		private string subQuery;

		public WhereTerm() {}

		/// <summary>
		/// Creates a comparison WhereTerm.
		/// </summary>
		/// <param name="expr1">Expression on the left side of the operator</param>
		/// <param name="expr2">Expression on the right side of the operator</param>
		/// <param name="op">Conditional operator to be applied on the expressions</param>
		/// <returns>A new conditional WhereTerm</returns>
		/// <remarks>
		/// A comparison term compares two expression on the basis of their values. Expressions can be of any type but their results must be of comparible types. 
		/// For instance, you can not compare a database field of type 'date' and a static value of type 'int'.
		/// </remarks>
		/// <example>
		/// <code>
		/// ...
		/// query.WherePhrase.Terms.Add(WhereTerm.CreateCompare(SqlExpression.Field("name", tCustomers), SqlExpression.String("J%"), CompareOperator.Like));
		/// </code>
		/// </example>
		public static WhereTerm CreateCompare(SqlExpression expr1, SqlExpression expr2, CompareOperator op)
		{
			WhereTerm term = new WhereTerm();
			term.expr1 = expr1;
			term.expr2 = expr2;
			term.op = op;

			term.type = WhereTermType.Compare;

			return term;
		}

		/// <summary>
		/// Creates a WhereTerm which represents SQL IN clause
		/// </summary>
		/// <param name="expr">Expression to be looked up</param>
		/// <param name="sql">Sub query</param>
		/// <returns></returns>
		public static WhereTerm CreateIn(SqlExpression expr, string sql)
		{
			WhereTerm term = new WhereTerm();
			term.expr1 = expr;
			term.subQuery = sql;

			term.type = WhereTermType.InSubQuery;

			return term;
		}

		/// <summary>
		/// Creates a WhereTerm which represents SQL IN clause
		/// </summary>
		/// <param name="expr">Expression to be looked up</param>
		/// <param name="values">List of values</param>
		/// <returns></returns>
		public static WhereTerm CreateIn(SqlExpression expr, SqlConstantCollection values)
		{
			WhereTerm term = new WhereTerm();
			term.expr1 = expr;
			term.values = values;

			term.type = WhereTermType.In;

			return term;
		}

		/// <summary>
		/// Creates a WhereTerm which represents SQL NOT IN clause
		/// </summary>
		/// <param name="expr">Expression to be looked up</param>
		/// <param name="sql">Sub query</param>
		/// <returns></returns>
		public static WhereTerm CreateNotIn(SqlExpression expr, string sql)
		{
			WhereTerm term = new WhereTerm();
			term.expr1 = expr;
			term.subQuery = sql;

			term.type = WhereTermType.NotInSubQuery;

			return term;
		}

		/// <summary>
		/// Creates a WhereTerm which represents SQL NOT IN clause
		/// </summary>
		/// <param name="expr">Expression to be looked up</param>
		/// <param name="values"></param>
		/// <returns></returns>
		public static WhereTerm CreateNotIn(SqlExpression expr, SqlConstantCollection values)
		{
			WhereTerm term = new WhereTerm();
			term.expr1 = expr;
			term.values = values;

			term.type = WhereTermType.NotIn;

			return term;
		}

		/// <summary>
		/// Creates a WhereTerm which returns TRUE if an expression is NULL
		/// </summary>
		/// <param name="expr">Expression to be evaluated</param>
		/// <returns></returns>
		public static WhereTerm CreateIsNull(SqlExpression expr)
		{
			WhereTerm term = new WhereTerm();
			term.expr1 = expr;
			term.type = WhereTermType.IsNull;
			return term;
		}

		/// <summary>
		/// Creates a WhereTerm which returns TRUE if an expression is NOT NULL
		/// </summary>
		/// <param name="expr"></param>
		/// <returns></returns>
		public static WhereTerm CreateIsNotNull(SqlExpression expr)
		{
			WhereTerm term = new WhereTerm();
			term.expr1 = expr;
			term.type = WhereTermType.IsNotNull;
			return term;
		}

		/// <summary>
		/// Creates a WhereTerm which encapsulates SQL EXISTS clause
		/// </summary>
		/// <param name="sql">Sub query for the EXISTS clause</param>
		/// <returns></returns>
		public static WhereTerm CreateExists(string sql)
		{
			WhereTerm term = new WhereTerm();
			term.subQuery = sql;
			term.type = WhereTermType.Exists;
			return term;
		}

		/// <summary>
		/// Creates a WhereTerm which encapsulates SQL NOT EXISTS clause
		/// </summary>
		/// <param name="sql">Sub query for the NOT EXISTS clause</param>
		/// <returns></returns>
		public static WhereTerm CreateNotExists(string sql)
		{
			WhereTerm term = new WhereTerm();
			term.subQuery = sql;
			term.type = WhereTermType.NotExists;
			return term;
		}

		/// <summary>
		/// Creates a WhereTerm which checks weather a value is in a specifed range.
		/// </summary>
		/// <param name="expr">Expression which yeilds the value to be checked</param>
		/// <param name="lowBound">Expression which yeilds the low bound of the range</param>
		/// <param name="highBound">Expression which yeilds the high bound of the range</param>
		/// <returns>A new WhereTerm</returns>
		/// <remarks>
		/// CreateBetween only accepts expressions which yeild a 'Date' or 'Number' values.
		/// All expressions must be of compatible types.
		/// </remarks>
		public static WhereTerm CreateBetween(SqlExpression expr, SqlExpression lowBound, SqlExpression highBound)
		{
			WhereTerm term = new WhereTerm();
			term.expr1 = expr;
			term.expr2 = lowBound;
			term.expr3 = highBound;

			term.type = WhereTermType.Between;

			return term;
		}

		public SqlExpression Expr1
		{
			get { return expr1; }
			set { expr1 = value; }
		}

		public SqlExpression Expr2
		{
			get { return expr2; }
			set { expr2 = value; }
		}

		public SqlExpression Expr3
		{
			get { return expr3; }
			set { expr3 = value; }
		}

		public CompareOperator Op
		{
			get { return op; }
			set { op = value; }
		}

		public WhereTermType Type
		{
			get { return type; }
			set { type = value; }
		}

		public SqlConstantCollection Values
		{
			get { return values; }
			set { values = value; }
		}

		public string SubQuery
		{
			get { return subQuery; }
			set { subQuery = value; }
		}

		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Creates a copy of this WhereTerm
		/// </summary>
		/// <returns>A new WhereTerm which exactly the same as the current one.</returns>
		public WhereTerm Clone()
		{
			WhereTerm a = new WhereTerm();

			a.expr1 = expr1;
			a.expr2 = expr2;
			a.expr3 = expr3;
			a.op = op;
			a.type = type;
			a.subQuery = subQuery;
			a.values = new SqlConstantCollection(values);

			return a;
		}
	}
}