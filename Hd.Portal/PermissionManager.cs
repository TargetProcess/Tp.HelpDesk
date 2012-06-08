//  
//  Copyright (c) 2005-2011 TargetProcess. All rights reserved.
//  TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.

using System.Linq;
using Hd.Portal.Components;

namespace Hd.Portal
{
	public class PermissionManager
	{
		public static bool HaveRightToViewRequest(Request request)
		{
			if (request.OwnerID == Requester.LoggedUserID)
			{
				return true;
			}
			if (request.Requesters.Any(x => x.RequesterID == Requester.LoggedUserID))
			{
				return true;
			}
			if (request.IsPrivate == true)
			{
				return false;
			}
			if (Settings.Scope == RequestScope.Private)
			{
				return false;
			}

			return true;
		}
	}
}