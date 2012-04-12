// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Text;

namespace Hd.Portal
{
	public class Team
	{
		private string _userName = string.Empty;
		private string _actorName = string.Empty;

		public string UserName
		{
			get { return _userName; }
			set { _userName = value; }
		}

		public string ActorName
		{
			get { return _actorName; }
			set { _actorName = value; }
		}
	}
}