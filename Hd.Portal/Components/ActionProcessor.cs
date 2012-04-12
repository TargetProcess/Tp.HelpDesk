// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;

using Tp.EntityStateServiceProxy;

namespace Hd.Portal.Components.LastActionProcessor
{
	public class ActionProcessor
	{
		public static bool Enabled = true;

		public void ProcessAction(ActionTypeEnum actionTypeEnum, IEntity entity)
		{
			if (!Enabled)
			{
				return;
			}

			string action = string.Empty;
			string postAction = string.Empty;

			switch (actionTypeEnum)
			{
				case ActionTypeEnum.Add:
					postAction = "added";
					break;
				case ActionTypeEnum.Update:
					postAction = "updated";
					break;
				case ActionTypeEnum.Delete:
					postAction = "deleted";
					break;
				case ActionTypeEnum.ChangeState:
					action = "The state of ";
					postAction = "changed";
					break;
				default:
					break;
			}

			action += GetEntityName(entity);

			PropertyDescriptorCollection descriptors = TypeDescriptor.GetProperties(entity.GetType());

			PropertyDescriptor namePropertyInfo = descriptors["Name"];
			PropertyDescriptor loginPropertyInfo = descriptors["Login"];

			string description = string.Empty;

			if (namePropertyInfo != null)
			{
				description += namePropertyInfo.GetValue(entity) as string;
			}

			if ((description == null || description == string.Empty) && loginPropertyInfo != null)
			{
				description = loginPropertyInfo.GetValue(entity) as string;
			}

			if (description != null && description != string.Empty)
			{
				action += " '" + description + "'";
			}

			action += " was " + postAction;

			LastAction = action;
			LastEntity = entity;
			LastActionTypeEnum = actionTypeEnum;
		}

		private static string GetEntityName(IEntity entity)
		{
			return entity.GetType().Name;
		}

		public static ILastActionHolder GetHolder()
		{
			if (!ReferenceEquals(HttpContext.Current, null))
			{
				return new HttpLastActionHolder();
			}

			return new LastActionHolder();
		}

		public static void ReplaceLastAction(string message)
		{
			LastAction = message;
		}

		public static void SetLastAction(string message, IEntity entity, ActionTypeEnum actionTypeEnum)
		{
			LastAction = message;
			LastEntity = entity;
			LastActionTypeEnum = actionTypeEnum;
		}

		public static IEntity LastEntity
		{
			get
			{
				ILastActionHolder lastActionHolder = GetHolder();
				return lastActionHolder.LastEntity;
			}
			set
			{
				ILastActionHolder lastActionHolder = GetHolder();
				lastActionHolder.LastEntity = value;
			}
		}

		public static ActionTypeEnum LastActionTypeEnum
		{
			get
			{
				ILastActionHolder lastActionHolder = GetHolder();
				return lastActionHolder.LastActionTypeEnum;
			}
			set
			{
				ILastActionHolder lastActionHolder = GetHolder();
				lastActionHolder.LastActionTypeEnum = value;
			}
		}

		public static bool IsError
		{
			get
			{
				ILastActionHolder lastActionHolder = GetHolder();
				return lastActionHolder.IsError;
			}
			set
			{
				ILastActionHolder lastActionHolder = GetHolder();
				lastActionHolder.IsError = value;
			}
		}

		public static string LastAction
		{
			get
			{
				ILastActionHolder lastActionHolder = GetHolder();
				return lastActionHolder.LastAction;
			}
			set
			{
				ILastActionHolder lastActionHolder = GetHolder();
				lastActionHolder.LastAction = value;
			}
		}

		public static void Clear()
		{
			ILastActionHolder lastActionHolder = GetHolder();
			if (lastActionHolder != null)
			{
				lastActionHolder.Clear();
			}
		}
	}
}