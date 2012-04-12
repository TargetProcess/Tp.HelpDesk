// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Hd.Portal.Components
{
	public static class StringUtils
	{
		public static string StripTags(string text)
		{
			return Regex.Replace(text, @"<(.|\n)*?>", " ");
		}

		public static bool IsBlank(
#if DOTNET3
			this
#endif
			string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return true;
			}
			for (int n = 0; n < value.Length; n++)
			{
				if (!Char.IsWhiteSpace(value[n]))
				{
					return false;
				}
			}
			return true;
		}

		public static string TrimToNull(
#if DOTNET3
			this
#endif
			string value)
		{
			if (IsBlank(value))
			{
				return null;
			}
			value = value.Trim();
			if (value.Length == 0)
			{
				return null;
			}
			return value;
		}

		public static string[] SmartSplit(string input)
		{
			var clauses = new List<string>();
			if (input == null)
				return clauses.ToArray();

			int position = 0;
			while (position < input.Length)
			{
				while (position < input.Length && input[position] == ' ')
				{
					position++;
				}
				if (position < input.Length)
				{
					int index = position;
					if (input[position] == '\"')
					{
						index++;
						position++;
						while (index < input.Length && input[index] != '\"')
						{
							index++;
						}
						clauses.Add(input.Substring(position, index - position));
						position = index + 1;
					}
					else
					{
						while (index < input.Length && input[index] != ' ')
						{
							index++;
						}
						clauses.Add(input.Substring(position, index - position));
						position = index;
					}
				}
			}

			return clauses.ToArray();
		}
	}
}