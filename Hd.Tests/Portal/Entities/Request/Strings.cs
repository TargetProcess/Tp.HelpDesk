//  
// Copyright (c) 2005-2009 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;

namespace Hd.Tests.Portal.Entities.Request
{
	public class Strings
	{
		public readonly string[] Values;

		private Strings(string[] values)
		{
			Values = values;
		}

		public override string ToString()
		{
			var result = String.Empty;
			Array.ForEach(Values, x =>
			                      	{
			                      		result += x;
			                      		if (new List<string>(Values).IndexOf(x) != Values.Length - 1)
			                      		{
			                      			result += ", ";
			                      		}
			                      	});
			return result;
		}

		public static Strings Create(params string[] values)
		{
			return new Strings(values);
		}
	}
}