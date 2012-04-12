// 
// Copyright (c) 2005-2009 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System.Reflection;
using Hd.Portal.Components;
using log4net;
using Microsoft.Web.Services3;
using Tp.Service.Proxies;

namespace Hd.Portal
{
	public class ServiceManager
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public static T GetService<T>() where T : WebServicesClientProtocol, new()
		{
			log.DebugFormat("Getting service proxy '{0}'", typeof(T));
			var serviceWse = new T
			{
				Url = Settings.TargetProcessPath + "/Services/" + typeof(T).Name + ".asmx"
			};

			log.DebugFormat("Service URL is '{0}'", serviceWse.Url);
			CredentialInitializer.InitCredentials(serviceWse, Settings.ActiveDirectoryMode, Settings.Login, Settings.Password);
			return serviceWse;
		}
	}
}