// 
// Copyright (c) 2005-2013 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using Hd.QueryExtensions;

namespace Hd.Portal
{
	internal class RequestQueryBase : BusinessQuery
	{
		public override SelectQuery InitialQuery
		{
			get
			{
				var selectQuery = new SelectQuery(typeof (Request));
				selectQuery.OrderByTerms.Clear();
				selectQuery.AddCompare("Project.DeleteDate", SqlExpression.Null(), CompareOperator.Equal);
				selectQuery.AddOrderBy("CreateDate", OrderByDirection.Descending);

				return selectQuery;
			}
		}
	}
}