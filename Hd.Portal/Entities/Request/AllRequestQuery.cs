using Hd.QueryExtensions;

namespace Hd.Portal
{
	public class AllRequestQuery : BusinessQuery
	{
		public override SelectQuery InitialQuery
		{
			get
			{
				SelectQuery query = RequestQueryFactory.CreateQuery().InitialQuery;		
				return query;
			}
		}
	}
}