// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using Hd.QueryExtensions;

namespace Hd.Portal
{
	internal class PrivateRequestQuery : BusinessQuery
	{
		public override SelectQuery InitialQuery
		{
			get
			{
				var selectQuery = new SelectQuery(typeof (Request));
				selectQuery.OrderByTerms.Clear();
				selectQuery.AddOrderBy("CreateDate", OrderByDirection.Descending);
				var requesterID = Requester.LoggedUserID.Value;
				var term = WhereTerm.CreateIn(SqlExpression.Field("RequestID"),
				                              string.Format(
				                              	@"select rr.Request.RequestID from RequestRequester as rr
													where
														(rr.Request.ParentProject.Company.CompanyID is null or
															(rr.Request.ParentProject.Company.CompanyID in
															( select rq.Company.CompanyID from Requester as rq where rq.UserID = {0} ))
														)
														and rr.Request.Owner.UserID = {0}",
				                              	requesterID));
				selectQuery.WherePhrase.Terms.Add(term);
				return selectQuery;
			}
		}
	}
}