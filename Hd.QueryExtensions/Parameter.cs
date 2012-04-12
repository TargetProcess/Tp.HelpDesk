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
	public class Parameter
	{
		private Object _value = null;
		private string _name = string.Empty;

		public Parameter() {}

		public Parameter(object value)
		{
			_value = value;
		}

		public Parameter(string name, object value)
		{
			_name = name;
			_value = value;
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public Object Value
		{
			get { return _value; }
			set { _value = value; }
		}
	}
}