using System;
using Hd.Portal.Components;
using Hd.QueryExtensions;

namespace Hd.Portal
{
	[Serializable]
	public class SearchFilter
	{
		private string _searchPattern;

		public string SearchPattern
		{
			get { return _searchPattern; }
			set
			{
				if (value != null)
				{
					value = value.Trim();
				}

				if (value == string.Empty)
				{
					value = null;
				}

				_searchPattern = value;
			}
		}


		public void AppendSimpleSearchPattern(SelectQuery selectQuery)
		{
			var groupName = new WhereClause(WhereClauseRelationship.And);
			var groupDescription = new WhereClause(WhereClauseRelationship.And);

			string[] words = StringUtils.SmartSplit(SearchPattern);

			AppendLikeExpression(words, selectQuery, groupName, "Name");
			AppendLikeExpression(words, selectQuery, groupDescription, "Description");

			var group = new WhereClause(WhereClauseRelationship.Or);

			group.SubClauses.Add(groupName);
			group.SubClauses.Add(groupDescription);

			selectQuery.WherePhrase.SubClauses.Add(group);
		}

		private void AppendLikeExpression(string[] words, SelectQuery selectQuery, WhereClause group, string columnName)
		{
			foreach (string word in words)
			{
				if (word.Length == 0)
				{
					continue;
				}

				var parameter = new Parameter("%" + word + "%");
                

				group.Terms.Add(
					WhereTerm.CreateCompare(SqlExpression.Field(columnName, selectQuery.FromClause.BaseTable),
					                        SqlExpression.Parameter(), CompareOperator.Like));

				selectQuery.Parameters.Add(parameter);
			}
		}
	}
}