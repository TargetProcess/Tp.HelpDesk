// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Hosting;

namespace Hd.Portal.Components
{
	public class Cache
	{
		public bool IsCacheAvailable
		{
			get { return HostingEnvironment.Cache != null; }
		}

		public void Set(string key, object value, int minutes)
		{
			DateTime time = DateTime.Now.AddMinutes(minutes);

			if (minutes == 0)
			{
				time = DateTime.Now.AddSeconds(5);
			}

			HostingEnvironment.Cache.Insert(key, value, null, time, TimeSpan.Zero);
		}

		public object Get(string key)
		{
			return HostingEnvironment.Cache.Get(key);
		}
	}
}