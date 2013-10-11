// 
// Copyright (c) 2005-2013 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using Hd.Portal;
using Hd.Web.Extensions;

public partial class Default : PersisterBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		phMessage.Visible = !Requester.IsLogged;
	}
}