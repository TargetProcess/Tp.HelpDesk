// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Text;

using Tp.EntityStateServiceProxy;

namespace Hd.Portal.Components.LastActionProcessor
{
	internal class LastActionHolder : ILastActionHolder
	{
		private static string _lastAction;
		private static bool _isError = false;
		private static IEntity _lastEntity;
		private static ActionTypeEnum _lastActionTypeEnum = ActionTypeEnum.None;

		public string LastAction
		{
			get { return _lastAction; }
			set
			{
				lock (this)
				{
					_lastAction = value;
				}
			}
		}

		public bool IsError
		{
			get { return _isError; }
			set { _isError = value; }
		}

		public IEntity LastEntity
		{
			get { return _lastEntity; }
			set { _lastEntity = value; }
		}

		public ActionTypeEnum LastActionTypeEnum
		{
			get { return _lastActionTypeEnum; }
			set { _lastActionTypeEnum = value; }
		}

		public void Clear()
		{
			lock (this)
			{
				_lastAction = null;
				_lastEntity = null;
				_lastActionTypeEnum = ActionTypeEnum.None;
				_isError = false;
			}
		}
	}
}