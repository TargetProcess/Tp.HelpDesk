// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web;

namespace Hd.Portal.Components
{
	public sealed class Context
	{
		private Context()
		{
			throw new NotSupportedException("must not be instantiated");
		}

		public static object GetValue(string name)
		{
			HttpContext httpContext = HttpContext.Current;
			if (ReferenceEquals(httpContext, null))
			{
				return CallContext.GetData(name);
			}
			else
			{
				return httpContext.Items[name];
			}
		}

		public static bool Contains(string name)
		{
			HttpContext httpContext = HttpContext.Current;
			if (ReferenceEquals(httpContext, null))
			{
				return CallContext.GetData(name) != null;
			}
			else
			{
				return httpContext.Items.Contains(name);
			}
		}

		public static void SetValue(string name, object value)
		{
			HttpContext httpContext = HttpContext.Current;
			if (ReferenceEquals(httpContext, null))
			{
				CallContext.SetData(name, value);
			}
			else
			{
				httpContext.Items[name] = value;
			}
		}

		public static void Remove(string name)
		{
			HttpContext httpContext = HttpContext.Current;
			if (ReferenceEquals(httpContext, null))
			{
				CallContext.FreeNamedDataSlot(name);
			}
			else
			{
				httpContext.Items.Remove(name);
			}
		}

		public static void Clear()
		{
			HttpContext httpContext = HttpContext.Current;
			if (ReferenceEquals(httpContext, null)) {}
			else
			{
				httpContext.Items.Clear();
			}
		}
	}
}