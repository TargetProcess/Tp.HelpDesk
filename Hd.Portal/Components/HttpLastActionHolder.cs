// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

using Tp.EntityStateServiceProxy;

namespace Hd.Portal.Components.LastActionProcessor
{
	internal class HttpLastActionHolder : ILastActionHolder
	{
		private readonly string LAST_ACTION_KEY = "LAST_ACTION_KEY";
		private readonly string LAST_ACTION_TYPE_KEY = "LAST_ACTION_TYPE_KEY";
		private readonly string LAST_ENTITY_KEY = "LAST_ENTITY_KEY";
		private readonly string LAST_IS_ERROR_KEY = "LAST_IS_ERROR_KEY";

		public HttpLastActionHolder()
		{
			if (ReferenceEquals(HttpContext.Current, null))
			{
				throw new ApplicationException("The component is available only for web");
			}
		}

		public string LastAction
		{
			get { return GetValue(LAST_ACTION_KEY) as string; }
			set { SetValue(LAST_ACTION_KEY, value); }
		}

		public bool IsError
		{
			get
			{
				object o = GetValue(LAST_IS_ERROR_KEY);

				if (o == null)
				{
					return false;
				}

				return (bool) o;
			}
			set { SetValue(LAST_IS_ERROR_KEY, value); }
		}

		public IEntity LastEntity
		{
			get { return GetValue(LAST_ENTITY_KEY) as IEntity; }
			set { SetValue(LAST_ENTITY_KEY, value); }
		}

		public ActionTypeEnum LastActionTypeEnum
		{
			get
			{
				if (GetValue(LAST_ACTION_TYPE_KEY) == null)
				{
					return ActionTypeEnum.None;
				}

				return (ActionTypeEnum) HttpContext.Current.Session[LAST_ACTION_TYPE_KEY];
			}
			set { SetValue(LAST_ACTION_TYPE_KEY, value); }
		}

		public void Clear()
		{
			SetValue(LAST_ACTION_KEY, null);
			SetValue(LAST_ENTITY_KEY, null);
			SetValue(LAST_ACTION_TYPE_KEY, null);
			SetValue(LAST_IS_ERROR_KEY, null);
		}

		private object GetValue(string key)
		{
			if (HttpContext.Current.Session == null)
			{
				return null;
			}

			return HttpContext.Current.Session[key];
		}

		private void SetValue(string key, object value)
		{
			if (HttpContext.Current.Session == null)
			{
				return;
			}

			HttpContext.Current.Session[key] = value;
		}
	}
}