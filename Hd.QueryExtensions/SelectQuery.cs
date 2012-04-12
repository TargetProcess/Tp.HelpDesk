//  
// Copyright (c) 2005-2009 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;

namespace Hd.QueryExtensions
{
	/// <summary>
	/// Specifies how tow operands are to be compared
	/// </summary>
	public enum CompareOperator
	{
		/// <summary>Equal</summary>
		Equal,
		/// <summary>Different</summary>
		NotEqual,
		/// <summary>Left operand is greater</summary>
		Greater,
		/// <summary>Left operand is less</summary>
		Less,
		/// <summary>Left operand is less or equal</summary>
		LessOrEqual,
		/// <summary>Left operand is greater or equal</summary>
		GreaterOrEqual,
		/// <summary>Make a bitwise AND and check the result for being not null (ex: (a &amp; b) > 0) ) </summary>
		BitwiseAnd,
		/// <summary>Substring. Use '%' signs in the value to match anything</summary>
		Like,

		Is,

		IsNot
	}

	/// <summary>
	/// Encapsulates a SQL SELECT statement.
	/// </summary>
	/// <remarks>
	/// Use SelectQuery to define and modify a query. 
	/// When the query is ready to be executed it can be rendered to SQL using one of the renderers derived from <see cref="Render.ISqlOmRenderer">ISqlOMRenderer</see>
	/// and executed using standard .Net query execution methods.
	/// <para>SelectQuery can be useful for dynamic SQL generation for reports and filters.</para>
	/// <para>It can also be used to render SQL to retrieve pages of data from databases which do not directly support this feature (i.e. SqlServer).</para>
	/// </remarks>
	/// <example>
	/// The following example creates a select query which returns two columns from two inner joined tables and renders it to be executed on MySql
	/// <code>
	/// FromTerm tCustomers = FromTerm.Table("customers");
	/// FromTerm tProducts = FromTerm.Table("products", "p");
	/// FromTerm tOrders = FromTerm.Table("orders", "o");
	/// 
	/// SelectQuery query = new SelectQuery();
	/// query.Columns.Add(new SelectColumn("name", tCustomers));
	/// query.Columns.Add(new SelectColumn("name", tProducts));
	/// query.FromClause.BaseTable = tCustomers;
	/// query.FromClause.Join(JoinType.Inner, query.FromClause.BaseTable, tOrders, "customerId", "customerId");
	/// query.FromClause.Join(JoinType.Inner, tOrders, tProducts, "productId", "productId");
	/// 
	/// MySqlRenderer renderer = new MySqlRenderer();
	///	string sql = renderer.RenderSelect(query);
	///	...
	/// </code>
	///</example>
	/// <example>
	/// This example creates a select query which returns the second page of a result-set and renders it to be executed on SqlServer
	/// <code>
	/// 
	/// int totalRows = 50; //The total number of rows can be obtained using SelectQuery as well
	/// 
	/// SelectQuery query = new SelectQuery();
	/// 
	/// query.Columns.Add(new SelectColumn("name"));
	/// query.FromPhrase.BaseTable = FromClause.Table("customers");
	/// query.OrderByClauses.Add(new OrderByClause("name", null, OrderByDirection.Descending));
	/// query.OrderByClauses.Add(new OrderByClause("birthDate", null, OrderByDirection.Ascending));
	/// 
	/// SqlServerRenderer renderer = new SqlServerRenderer();
	///	sql = renderer.RenderPage(2, 10, totalRows, query);
	///	...
	/// </code>
	///</example>
	[Serializable]
	public class SelectQuery : ICloneable
	{
		private SelectColumnCollection columns = new SelectColumnCollection();
		private WhereClause wherePhrase = new WhereClause();
		private WhereClause havingPhrase = new WhereClause();
		private FromClause fromClause = new FromClause();

		private OrderByTermCollection orderByTerms = new OrderByTermCollection();
		private GroupByTermCollection groupByTerms = new GroupByTermCollection();
		private ParameterCollection parameters = new ParameterCollection();
		private PageSettings pageSettings = new PageSettings();
		private bool usingCache;

		public PageSettings PageSettings
		{
			get { return pageSettings; }
			set { pageSettings = value; }
		}

		private int top = -1;
		private bool distinct;
		private string tableSpace;

		/// <summary>
		/// Creates a new SelectQuery
		/// </summary>
		public SelectQuery()
		{
		}

		/// <summary>
		/// Creates a new SelectQuery
		/// </summary>
		public SelectQuery(Enum entity)
		{
			FromTerm term = FromTerm.Table(entity.ToString(), entity.ToString().ToLower());
			FromClause.BaseTable = term;
		}

		public SelectQuery(Type t)
		{
			string[] parts = t.ToString().Split(new[] {'.'});
			string entityName = parts[parts.Length - 1];
			FromTerm term = FromTerm.Table(entityName, entityName.ToLower());
			FromClause.BaseTable = term;
		}

		/// <summary>
		/// Gets the FROM definition for this SelectQuery
		/// </summary>
		public FromClause FromClause
		{
			get { return fromClause; }
			set { fromClause = value; }
		}

		/// <summary>
		/// Gets the GROUP BY definition for this SelectQuery
		/// </summary>
		public GroupByTermCollection GroupByTerms
		{
			get { return groupByTerms; }
		}

		public void AddParameter(object value)
		{
			Parameters.Add(new Parameter(value));
		}

		public ParameterCollection Parameters
		{
			get { return parameters; }
			set { parameters = value; }
		}

		/// <summary>
		/// Gets the ORDER BY definition for this SelectQuery
		/// </summary>
		public OrderByTermCollection OrderByTerms
		{
			get { return orderByTerms; }
			set { orderByTerms = value; }
		}

		/// <summary>
		/// Gets the WHERE conditions for this SelectQuery
		/// </summary>
		public WhereClause WherePhrase
		{
			get { return wherePhrase; }
			set { wherePhrase = value.Clone(); }
		}

		/// <summary>
		/// Gets the collection of columns for this SelectQuery
		/// </summary>
		public SelectColumnCollection Columns
		{
			get { return columns; }
			set { columns = value; }
		}

		public void AddCompare(string column, Parameter parameter, CompareOperator compareOperator)
		{
			WherePhrase.Terms.Add(WhereTerm.CreateCompare(SqlExpression.Field(column, fromClause.BaseTable),
			                                              compareOperator == CompareOperator.Like
			                                              	? SqlExpression.LikeExpressionParameter()
			                                              	: SqlExpression.Parameter(), compareOperator));

			Parameters.Add(parameter);
		}

		public void AddCompare(Enum column, Parameter parameter, CompareOperator compareOperator)
		{
			WherePhrase.Terms.Add(WhereTerm.CreateCompare(SqlExpression.Field(column, fromClause.BaseTable),
			                                              compareOperator == CompareOperator.Like
			                                              	? SqlExpression.LikeExpressionParameter()
			                                              	: SqlExpression.Parameter(), compareOperator));

			Parameters.Add(parameter);
		}

		public void AddCompare(Enum column, SqlExpression expression, CompareOperator compareOperator)
		{
			WherePhrase.Terms.Add(CreateCompare(column, expression, compareOperator));
		}

		public WhereTerm CreateCompare(Enum column, SqlExpression expression, CompareOperator compareOperator)
		{
			return WhereTerm.CreateCompare(SqlExpression.Field(column, fromClause.BaseTable),
			                               expression, compareOperator);
		}

		public void AddCompare(string column, SqlExpression expression, CompareOperator compareOperator)
		{
			WherePhrase.Terms.Add(WhereTerm.CreateCompare(SqlExpression.Field(column, fromClause.BaseTable),
			                                              expression, compareOperator));
		}

		public void AddOrderBy(Enum manyToOneField, Enum column, OrderByDirection direction)
		{
			OrderByTerms.Add(new OrderByTerm(manyToOneField + "." + column, fromClause.BaseTable,
			                                 direction));
		}

		public void AddOrderBy(Enum column, OrderByDirection direction)
		{
			OrderByTerms.Add(new OrderByTerm(column, fromClause.BaseTable,
			                                 direction));
		}

		public void AddOrderBy(string column, OrderByDirection direction)
		{
			OrderByTerms.Add(new OrderByTerm(column, fromClause.BaseTable,
			                                 direction));
		}

		/// <summary>
		/// Gets or sets the result-set row count limitation
		/// </summary>
		/// <remarks>
		/// When Top is less then zero, no limitation will apply on the result-set. To limit
		/// the number of rows returned by this query set Top to a positive integer or zero
		/// </remarks>
		public int Top
		{
			get { return top; }
			set { top = value; }
		}

		/// <summary>
		/// Gets or sets wheather only distinct rows are to be returned.
		/// </summary>
		public bool Distinct
		{
			get { return distinct; }
			set { distinct = value; }
		}

		/// <summary>
		/// Validates the SelectQuery
		/// </summary>
		/// <remarks>
		/// Sql.Net makes its best to validate a query before it is rendered or executed. 
		/// Still, some errors and inconsistancies can only be found on later stages.
		/// </remarks>
		public void Validate()
		{
			//if (columns.Count == 0)
			//    throw new InvalidQueryException("A select query must have at least one column");

			if (fromClause.BaseTable == null)
			{
				throw new InvalidQueryException("A select query must have FromPhrase.BaseTable set");
			}
		}

		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Clones the SelectQuery
		/// </summary>
		/// <returns>A new instance of SelectQuery which is exactly the same as the current one.</returns>
		public SelectQuery Clone()
		{
			var newQuery = new SelectQuery
			               	{
			               		columns = new SelectColumnCollection(columns),
			               		orderByTerms = new OrderByTermCollection(orderByTerms),
			               		groupByTerms = new GroupByTermCollection(groupByTerms),
			               		wherePhrase = wherePhrase.Clone(),
			               		fromClause = fromClause.Clone(),
			               		top = top,
			               		distinct = distinct,
			               		tableSpace = tableSpace,
			               		parameters = new ParameterCollection(parameters)
			               	};

			var newSettings = new PageSettings {PageIndex = pageSettings.PageIndex, PageSize = pageSettings.PageSize};
			newQuery.pageSettings = newSettings;
			return newQuery;
		}

		/// <summary>
		/// Gets or sets the common prefix for all tables in the query
		/// </summary>
		/// <remarks>
		/// You might want to use <see cref="TableSpace"/> property to utilize SQL Server 2000
		/// execution plan cache. For the cache to work in SQL Server 2000, all database objects in a query must be fully qualified.
		/// Setting <see cref="TableSpace"/> property might releive of the duty to fully qualify all table names in the query.
		/// </remarks>
		public string TableSpace
		{
			get { return tableSpace; }
			set { tableSpace = value; }
		}

		public bool UsingCache
		{
			get { return usingCache; }
			set { usingCache = value; }
		}
	}
}