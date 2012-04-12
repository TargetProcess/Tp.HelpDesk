using Hd.QueryExtensions;

namespace Hd.Portal
{
	public class IdeasQuery : BusinessQuery
	{
		public override SelectQuery InitialQuery
		{
			get
			{
				var query = RequestQueryFactory.CreateQuery().InitialQuery;
				var term = WhereTerm.CreateCompare(
					SqlExpression.Field("RequestType.Name"),
					SqlExpression.String("Idea"), CompareOperator.Equal);
				query.WherePhrase.Terms.Add(term);
				return query;
			}
		}
	}
}