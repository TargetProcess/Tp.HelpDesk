// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Tp.RequestServiceProxy;

namespace Hd.Portal
{
	public class RequestersCountCache : ChildEntityCache
	{
		protected override void ProcessEntries(Hashtable hashtable, int[] ids)
		{
			RequestService requestServiceWse = ServiceManager.GetService<RequestService>();
			RequestersCount[] requestersCounts = requestServiceWse.GetRequestersCountArray(ids);

			foreach (RequestersCount requestersCount in requestersCounts)
			{
				List<RequestersCount> list = new List<RequestersCount>();
				list.Add(requestersCount);

				AppendGroupToHashtable(requestersCount.RequestID, list, hashtable);
			}
		}
	}
}