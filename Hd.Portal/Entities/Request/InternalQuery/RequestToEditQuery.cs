//  
// Copyright (c) 2005-2009 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using Hd.QueryExtensions;

namespace Hd.Portal
{
	internal class RequestToEditQuery : BusinessQuery
	{
		public override SelectQuery InitialQuery
		{
			get
			{
				var selectQuery = new SelectQuery(typeof (Request));
				int requesterID = Requester.LoggedUserID.Value;
				selectQuery.AddCompare("Owner.UserID", new Parameter(requesterID), CompareOperator.Equal);
				selectQuery.OrderByTerms.Clear();
				selectQuery.AddOrderBy("CreateDate", OrderByDirection.Descending);
				return selectQuery;
			}
		}
	}
}