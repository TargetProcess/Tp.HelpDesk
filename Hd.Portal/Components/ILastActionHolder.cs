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
	public interface ILastActionHolder
	{
		string LastAction { get; set; }
		bool IsError { get; set; }
		IEntity LastEntity { get; set; }
		ActionTypeEnum LastActionTypeEnum { get; set; }
		void Clear();
	}
}