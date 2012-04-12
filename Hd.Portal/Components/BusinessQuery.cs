//  
// Copyright (c) 2005-2009 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using Hd.QueryExtensions;

namespace Hd.Portal
{
	public abstract class BusinessQuery
	{
		public SelectQuery _selectQuery;
		public abstract SelectQuery InitialQuery { get; }

		public SelectQuery Query
		{
			get { return _selectQuery ?? (_selectQuery = InitialQuery); }
			set { _selectQuery = value; }
		}

		public void Reinitialize()
		{
			_selectQuery = InitialQuery;
		}

		public static BusinessQuery CreateInstance(string typeName)
		{
			return (BusinessQuery) Activator.CreateInstance(typeof (BusinessQuery).Assembly.FullName, typeName).Unwrap();
		}
	}
}