// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Hd.Portal.Entities.EntityState;
using Hd.QueryExtensions;
using Hd.QueryExtensions.Render;

using log4net;


using Tp.AttachmentServiceProxy;
using Tp.CommentServiceProxy;
using Tp.EntityStateServiceProxy;
using Tp.GeneralUserServiceProxy;
using Tp.PriorityServiceProxy;
using Tp.RequesterServiceProxy;
using Tp.RequestServiceProxy;
using Tp.RequestTypeServiceProxy;
using Tp.RoleServiceProxy;
using Tp.UserServiceProxy;

namespace Hd.Portal
{
	public class DataPortal
	{
		private static DataPortal _instance = null;
		private static readonly object _syncRoot = new object();
		private readonly Dictionary<string, IDataFactory> _dataFactories = new Dictionary<string, IDataFactory>();
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		static DataPortal()
		{
			log.Debug("Initializing DataPortal");
			Initialize();
		}

		public static void Initialize()
		{
			lock (_syncRoot)
			{
				_instance = new DataPortal();
				_instance._dataFactories.Add(GetTypeKey(typeof (Request)), new ServiceFactory<RequestService>());
				_instance._dataFactories.Add(GetTypeKey(typeof (EntityState)), new ServiceFactory<EntityStateService>(true));
				_instance._dataFactories.Add(GetTypeKey(typeof (RequestType)), new ServiceFactory<RequestTypeService>(true));
				_instance._dataFactories.Add(GetTypeKey(typeof (Role)), new ServiceFactory<RoleService>(true));
				_instance._dataFactories.Add(GetTypeKey(typeof (User)), new ServiceFactory<UserService>(true));
				_instance._dataFactories.Add(GetTypeKey(typeof (Requester)), new ServiceFactory<RequesterService>(true));
				_instance._dataFactories.Add(GetTypeKey(typeof (Priority)), new ServiceFactory<PriorityService>(true));
				_instance._dataFactories.Add(GetTypeKey(typeof (GeneralUser)), new ServiceFactory<GeneralUserService>(true));
				_instance._dataFactories.Add(GetTypeKey(typeof (Attachment)), new ServiceFactory<AttachmentService>());
				_instance._dataFactories.Add(GetTypeKey(typeof (Comment)), new ServiceFactory<CommentService>());
			}
		}

		private DataPortal()
		{
			log.Debug("DataPortal ctor");
		}

		public void ResetCachedValue(Type type, int? identity)
		{
			var factory = _dataFactories[GetTypeKey(type)];
			factory.RetrieveWithRecache(identity, true);
		}

		public static DataPortal Instance
		{
			get { return _instance; }
		}

		public void SetFactory<T>(IDataFactory factory)
		{
			Type type = typeof (T);
			string typeKey = GetTypeKey(type);

			if (!_dataFactories.ContainsKey(typeKey))
			{
				_dataFactories.Add(typeKey, factory);
			}

			_dataFactories[typeKey] = factory;
		}

		private static string GetTypeKey(Type type)
		{
			return type.Name;
		}

		public IList Retrieve(SelectQuery query)
		{
			Type type = GetType(query);
			string key = GetTypeKey(type);
			IList list = GetList(key, query);
			return DataConverter<object>.Convert(list, type);
		}

		private static Type GetType(SelectQuery query)
		{
			return Type.GetType("Hd.Portal." + query.FromClause.BaseTable.Expression);
		}

		public List<T> RetrieveAll<T>() where T : new()
		{
			string key = GetTypeKey(typeof (T));
			IDataFactory factory = GetFactorySafe(key);
			IList list = factory.RetieveAll();
			var converter = new DataConverter<T>();
			return converter.Convert(list);
		}

		public List<T> Retrieve<T>(SelectQuery query) where T : new()
		{
			string key = GetTypeKey(typeof (T));

			IList list = GetList(key, query);

			var converter = new DataConverter<T>();
			return converter.Convert(list);
		}

		public List<T> Retrieve<T>(string hql, params object[] parameters) where T : new()
		{
			string key = GetTypeKey(typeof (T));

			IDataFactory factory = GetFactorySafe(key);

			IList list = factory.Retieve(hql, parameters);

			var converter = new DataConverter<T>();
			return converter.Convert(list);
		}

		private IList GetList(string typeKey, SelectQuery query)
		{
			var hqlRenderer = new HqlRenderer();
			string hql = hqlRenderer.RenderSelect(query);

			List<object> parameters = GetParameters(query);

			IDataFactory factory = GetFactorySafe(typeKey);

			return factory.Retieve(hql, query.PageSettings.PageIndex,
			                       query.PageSettings.PageSize, parameters.ToArray());
		}

		private static List<object> GetParameters(SelectQuery query)
		{
			var parameters = new List<object>();

			foreach (Parameter parameter in query.Parameters)
			{
				parameters.Add(parameter.Value);
			}
			return parameters;
		}

		private IDataFactory GetFactorySafe(string key)
		{
			if (!_dataFactories.ContainsKey(key))
			{
				throw new Exception(string.Format("Data operations with {0} type are not supported", key));
			}

			return _dataFactories[key];
		}

		public IEntity Retrieve(Type type, int? identity)
		{
			if (!identity.HasValue)
			{
				return null;
			}

			if (identity <= 0)
			{
				return null;
			}

			string key = GetTypeKey(type);

			IDataFactory factory = GetFactorySafe(key);

			object entity = factory.Retrieve(identity.Value);
			object instance = Activator.CreateInstance(type);

			return (IEntity) DataConverter<object>.Convert(entity, instance);
		}

		public int RetrieveCount(SelectQuery query)
		{
			var renderer = new HqlRenderer();
			string hql = renderer.RenderRowCount(query);
			IDataFactory factory = GetFactorySafe(GetTypeKey(query));

			return factory.RetrieveCount(hql, GetParameters(query).ToArray());
		}

		private static string GetTypeKey(SelectQuery query)
		{
			return GetTypeKey(GetType(query));
		}
	}
}