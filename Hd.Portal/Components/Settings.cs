// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

using Tp.AuthenticationServiceProxy;

namespace Hd.Portal.Components
{
	public class Settings
	{
		public static Exception CheckSettings()
		{
			try
			{
				AuthenticationService service = ServiceManager.GetService<AuthenticationService>();
				service.Authenticate();
			}
			catch (Exception exception)
			{
				return exception;
			}

			return null;
		}

		public static string Title
		{
			get { return ConfigurationManager.AppSettings["Title"]; }
		}

		public static RequestScope Scope
		{
			get
			{
				string scopeValue = ConfigurationManager.AppSettings["Scope"];
				if (string.IsNullOrEmpty(scopeValue))
				{
					return RequestScope.Private;
				}
				RequestScope scope = (RequestScope) Enum.Parse(typeof (RequestScope), scopeValue);
				return scope;
			}
		}

		public static string Password
		{
			get { return ConfigurationManager.AppSettings["AdminPassword"]; }
		}

		public static bool ActiveDirectoryMode
		{
			get
			{
				string setting = ConfigurationManager.AppSettings["ActiveDirectoryMode"] ?? string.Empty;
				return setting.ToLower() == "true";
			}
		}

		public static string Login
		{
			get { return ConfigurationManager.AppSettings["AdminLogin"]; }
		}

		public static string TargetProcessPath
		{
			get
			{
				string url = ConfigurationManager.AppSettings["TargetProcessPath"];

				if (!string.IsNullOrEmpty(url))
				{
					url = url.TrimEnd(new[] {'/', '\\'});
				}

				return url;
			}
		}

		public static bool IsPublicMode
		{
			get
			{
				string setting = ConfigurationManager.AppSettings["IsPublic"] ?? string.Empty;
				return setting.ToLower() == "true" && Scope == RequestScope.Global;
			}
		}
	}
}
