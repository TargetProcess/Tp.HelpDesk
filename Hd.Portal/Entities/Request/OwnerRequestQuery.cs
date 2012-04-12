using Hd.QueryExtensions;

namespace Hd.Portal
{
	public class OwnerRequestQuery : BusinessQuery
	{
		public override SelectQuery InitialQuery
		{
			get { return RequestQueryFactory.CreateOwnerQuery().InitialQuery; }
		}
	}
}