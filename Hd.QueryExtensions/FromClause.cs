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
	/// Represents the FROM clause of a select statement
	/// </summary>
	/// <remarks>
	/// FromClause consists of a base table set by the <see cref="FromClause.BaseTable">BaseTable</see> property
	/// and optional joins defined using the <see cref="FromClause.Join">Join</see> method.
	/// <para>
	/// SqlOM supports inner, outer and cross joins. 
	/// Inner join between two tables returns only rows which exist in both tables.
	/// Outer (Left, Right and Full) joins return rows when at least one of the tables has a matching row. 
	/// Left outer joins returns all rows from the left table and while the missing rows from the right are filled with nulls.
	/// Right outer join is the opposite of left. Full outer join returns all the rows from the left and the right tables while the missing rows from the opposite table are filled with nulls.
	/// Cross join does not match any keys and returns the cartesian product of both tables.
	/// For more information about joins consult SQL documentation.
	/// </para>	
	/// </remarks>
	[Serializable]
	public class FromClause : ICloneable
	{
		private FromTerm baseTable = null;

		public FromClause() {}

		/// <summary>
		/// Gets or sets the base table for the FromClause
		/// </summary>
		/// <remarks>
		///	The base table begins the serie of joins. 
		///	If no joins are specified for the query the base table is the only table in the select statement.
		///	BaseTable must be set before <see cref="SelectQuery">SelectQuery</see> can be rendered.
		/// </remarks>
		public FromTerm BaseTable
		{
			get { return baseTable; }
			set { baseTable = value; }
		}

		/// <summary>
		/// Checks if a term with the specified RefName already exists in the FromClause.
		/// </summary>
		/// <param name="alias">The name of the term to be checked.</param>
		/// <returns>true if the term exists or false otherwise</returns>
		/// <remarks>
		/// TermExists matches <paramref name="alias">alias</paramref> to <see cref="FromTerm.RefName">RefName</see> of all participating FromTerms.
		/// </remarks>
		public bool TermExists(string alias)
		{
			if (baseTable != null)
			{
				return string.Compare(baseTable.RefName, alias) == 0;
			}

			return false;
		}

		/// <summary>
		/// Returns true if this FromClause has no terms at all
		/// </summary>
		public bool IsEmpty
		{
			get { return baseTable == null; }
		}

		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Creates a clone of this FromClause
		/// </summary>
		/// <returns>A new FromClause which exactly the same as the current one.</returns>
		public FromClause Clone()
		{
			FromClause a = new FromClause();
			a.baseTable = baseTable;
			return a;
		}
	}
}