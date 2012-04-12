// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Hd.Components
{
	public class Formatter
	{
		/// <summary>
		/// Formats decimal string. Cut off "00" 
		/// </summary>
		/// <param name="input">Input string (decimal value)</param>
		/// <returns></returns>
		public static string FormatDecimal(object input)
		{
			if (input == null)
			{
				return "#N/A";
			}

			string separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

			// Fix #398. Bug on saving feature on saving with initial Estimate 1000
			// remove group separators
			string groupSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
			string inp = input.ToString().Replace(groupSeparator, "");

			string[] parts = inp.ToString().Split(new char[] {Convert.ToChar(separator)});
			if (parts.Length == 1)
			{
				return parts[0];
			}
			else if (parts.Length == 2)
			{
				if (parts[1] == "0" || parts[1] == "00" || parts[1] == "0000")
				{
					return parts[0];
				}
			}

			if (parts[1].Length > 2)
			{
				return parts[0] + separator + parts[1].Substring(0, 2);
			}
			else
			{
				return parts[0] + separator + parts[1];
			}
		}
	}
}