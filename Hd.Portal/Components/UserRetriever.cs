// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Text;

namespace Hd.Portal.Components
{
	public class UserRetriever : IUserRetriever
	{
		private static readonly object syncRoot = new object();

		public  void SetUser(Requester user)
		{
			lock (syncRoot)
				Context.SetValue("USER", user);
		}

		public Requester LoggedUser
		{
			get { return Context.GetValue("USER") as Requester; }
		}
	}
}