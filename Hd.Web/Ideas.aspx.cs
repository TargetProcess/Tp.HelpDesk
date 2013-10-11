﻿// 
// Copyright (c) 2005-2013 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using Hd.Web.Extensions;

namespace Hd.Web
{
	public partial class Ideas : PersisterBasePage
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			controller.RefreshGrid();
		}
	}
}