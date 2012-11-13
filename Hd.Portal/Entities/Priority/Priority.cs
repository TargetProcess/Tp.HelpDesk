// 
// Copyright (c) 2005-2012 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System.Collections.Generic;
using Tp.PriorityServiceProxy;

namespace Hd.Portal
{
	public class Priority : PriorityDTO, IEntity
	{
		public static IList<Priority> RetrieveAllForRequest()
		{
			return DataPortal.Instance.Retrieve<Priority>("from Priority as p where p.EntityType.EntityTypeID = 17");
		}
	}
}