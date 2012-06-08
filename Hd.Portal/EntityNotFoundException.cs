//  
//  Copyright (c) 2005-2011 TargetProcess. All rights reserved.
//  TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.

using System;

namespace Hd.Portal
{
	public class EntityNotFoundException : Exception
	{
		public EntityNotFoundException()
			: base("Entity could not be found")
		{
		}
	}
}