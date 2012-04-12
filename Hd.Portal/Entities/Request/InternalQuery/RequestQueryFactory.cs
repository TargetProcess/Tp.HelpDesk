//  
// Copyright (c) 2005-2009 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using Hd.Portal.Components;

namespace Hd.Portal
{
	internal static class RequestQueryFactory
	{
		public static BusinessQuery CreateQueryToEdit()
		{
			return new RequestToEditQuery();
		}

		public static BusinessQuery CreateOwnerQuery()
		{
			return new PrivateRequestQuery();
		}

		public static BusinessQuery CreateQuery()
		{
			switch (Settings.Scope)
			{
				case RequestScope.Private:
					return new PrivateRequestQuery();
				case RequestScope.Global:
					return new GlobalRequestQuery();
			}
			throw new ApplicationException("Undefined Request Scope.");
		}

		public static BusinessQuery CreateRetreaveQuery()
		{
			return new RequestToViewQuery();
		}

		public static BusinessQuery CreateDefaultQuery()
		{
			return new RequestDefaultQuery();
		}
	}
}