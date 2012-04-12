// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Hd.Portal
{
	public interface IDataFactory
	{
		IList Retieve(string hql, int? pageIndex, int? pageSize, object[] parameters);
		IList Retieve(string hql, object[] parameters);
		IList RetieveAll();
		object Retrieve(int? identity);
		object RetrieveWithRecache(int? identity, bool recache);
		int RetrieveCount(string hql, object[] parameters);
	}
}