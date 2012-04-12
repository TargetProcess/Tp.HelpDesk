// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Hd.Portal.Components;

using log4net;

using Microsoft.Web.Services3;

namespace Hd.Portal
{
	public class ServiceFactory<T> : IDataFactory where T : WebServicesClientProtocol, new()
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private readonly Cache _cache = new Cache();
		private readonly T _webService = ServiceManager.GetService<T>();
		private bool _isCacheable = false;

		public bool IsCacheable
		{
			get { return _isCacheable; }
			set { _isCacheable = value; }
		}

		public ServiceFactory() {}

		public ServiceFactory(bool isCacheable)
		{
			IsCacheable = isCacheable;
		}

		public IList Retieve(string hql, int? pageIndex, int? pageSize, object[] parameters)
		{
			log.DebugFormat("Execute query '{0}'", hql);
			if (!pageIndex.HasValue)
			{
				pageIndex = -1;
			}

			if (!pageSize.HasValue)
			{
				pageSize = -1;
			}

			Object result =
				InvokeMethod("RetrievePage", new object[] {hql, pageIndex.Value, pageSize.Value, parameters});

			return GetList(result);
		}

		public IList Retieve(string hql, object[] parameters)
		{
			Object result =
				InvokeMethod("Retrieve", new object[] {hql, parameters});

			return GetList(result);
		}

		private static IList GetList(object result)
		{
			log.Debug("Form list");

			List<object> objects = new List<object>();

			Array array = (Array) result;

			foreach (object o in array)
			{
				objects.Add(o);
			}

			return objects;
		}

		public IList RetieveAll()
		{
			log.Debug("Retrieve all");

			string key = typeof (T).Name + "_All";
			if (IsCacheAvailable)
			{
				object cached = _cache.Get(key);

				if (cached != null)
				{
					return cached as IList;
				}
			}

			Object result =
				InvokeMethod("RetrieveAll", new object[] {});

			IList list = GetList(result);

			if (IsCacheAvailable && list != null)
			{
				_cache.Set(key, list, 10);
			}

			return list;
		}

		public object Retrieve(int? identity)
		{
			return RetrieveWithRecache(identity, false);
		}

		public object RetrieveWithRecache(int? identity, bool recache)
		{
			log.DebugFormat("Retrieve by identity '{0}' with recaching options set to '{1}'", identity, recache);

			string key = GetCacheKey(identity);

			if (IsCacheAvailable)
			{
				object cached = _cache.Get(key);

				if (cached != null && !recache)
				{
					return cached;
				}
			}

			log.Debug("No found in cache. Do invoke");

			object[] parameters = new object[] {identity.Value};
			string methodName = "GetByID";

			object result = InvokeMethod(methodName, parameters);

			if (IsCacheAvailable && result != null)
			{
				_cache.Set(key, result, 10);
			}

			log.Debug("Return result");

			return result;
		}

		private bool IsCacheAvailable
		{
			get { return IsCacheable && _cache.IsCacheAvailable; }
		}

		private static string GetCacheKey(int? identity)
		{
			return typeof (T).Name + "_" + identity;
		}

		private object InvokeMethod(string methodName, object[] parameters)
		{
			log.DebugFormat("Invoke method '{0}' for '{1}'", methodName, _webService.GetType().Name);
			Object result = _webService.GetType().InvokeMember(methodName, BindingFlags.InvokeMethod,
			                                                   Type.DefaultBinder, _webService, parameters);

			return result;
		}

		public int RetrieveCount(string hql, object[] parameters)
		{
			Object result = InvokeMethod("RetrieveCount", new object[] {hql, parameters});
			return (int) result;
		}
	}
}