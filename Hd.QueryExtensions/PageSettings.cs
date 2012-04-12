// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Text;

namespace Hd.QueryExtensions
{
	[Serializable]
	public class PageSettings
	{
		private int pageSize = 20;
		private int pageIndex = int.MinValue;

		public int PageSize
		{
			get { return pageSize; }
			set { pageSize = value; }
		}

		public int PageIndex
		{
			get { return pageIndex; }
			set { pageIndex = value; }
		}

		public bool IsPageable
		{
			get { return pageIndex != int.MinValue && pageSize != 0; }
		}
	}
}