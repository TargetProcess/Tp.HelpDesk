//  
//  Copyright (c) 2005-2011 TargetProcess. All rights reserved.
//  TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.

using System;

namespace Hd.Portal
{
	public class AccessDeniedException : Exception
	{
		public AccessDeniedException()
			: base("You don't have permissions to see the requested entity")
		{
		}
	}
}