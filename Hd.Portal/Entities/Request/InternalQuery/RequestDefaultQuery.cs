using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hd.QueryExtensions;

namespace Hd.Portal
{
	internal class RequestDefaultQuery : BusinessQuery
	{
		public override SelectQuery InitialQuery
		{
			get
			{
				var selectQuery = new SelectQuery(typeof(Request));				
				selectQuery.OrderByTerms.Clear();
				selectQuery.AddOrderBy("CreateDate", OrderByDirection.Descending);
				return selectQuery;
			}
		}
	}
}
