// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Hd.Portal.Components;

namespace Tp.Web.Extensions.Components
{
	public class WordsHighlighter
	{
		private const int PREVIEW_LETTERS = 50;
		private const int MAX_MATCH_CYCLES = 1;

		public static string Highlight(string value, string searchPattern)
		{
			value = PlainTextRenderer.RenderToPlainText(value) ?? "";

			if (StringUtils.IsBlank(searchPattern))
			{
				return HttpUtility.HtmlEncode(value); // The text may contain angle braces read from html entities &lt; and &gt;, so encode it.
			}

			string[] words = StringUtils.SmartSplit(searchPattern);

			string trim = string.Join("|", words);
			trim = trim.Trim();
			string regex = string.Empty;

			foreach (char c in trim)
			{
				if (c == '|')
				{
					regex += c;
					continue;
				}
				if (c == '\\')
				{
					regex += "[\\\\]";
					continue;
				}
				if (c == '^')
				{
					regex += "[\\^]";
					continue;
				}

				regex += "[" + c + "]";
			}

			var rx = new Regex(regex, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase);
			MatchCollection matches = rx.Matches(value);

			if (matches.Count == 0)
			{
				return HttpUtility.HtmlEncode(value); // The text may contain angle braces read from html entities &lt; and &gt;, so encode it.
			}

			string text = "";

			var foundMatches = new Dictionary<string, int>();

			for (int i = 0; i < matches.Count; i++)
			{
				Match match = matches[i];

				if (!foundMatches.ContainsKey(match.Value))
				{
					foundMatches.Add(match.Value, 1);
				}
				else if (foundMatches[match.Value] < MAX_MATCH_CYCLES)
				{
					foundMatches[match.Value] = foundMatches[match.Value]++;
				}
				else
				{
					continue;
				}

				int start = DefineStartPosition(match);
				int length = DefineLength(value, match, start);

				if (start == 0 && length == value.Length)
				{
					text = value;
					break;
				}

				text += CutText(value, start, length);
			}

			var result = new StringBuilder(text.Length * 2);
			matches = rx.Matches(text);
			int offset = 0;
			for (int i = 0; i < matches.Count; i++)
			{
				Match match = matches[i];
				result.Append(HttpUtility.HtmlEncode(text.Substring(offset, match.Index - offset)));
				result.Append("<span class=\"wordHighlight\">").Append(HttpUtility.HtmlEncode(match.Value)).Append("</span>");
				if (i == matches.Count - 1)
				{
					result.Append(HttpUtility.HtmlEncode(text.Substring(match.Index + match.Length)));
				}
				offset = match.Index + match.Length;
			}
			return result.ToString();
		}

		

		private static string CutText(string value, int start, int length)
		{
			string text = "";

			if (start != 0)
			{
				text += "...";
			}

			text += value.Substring(start, length);

			if (length != value.Length)
			{
				text += "...";
			}

			return text;
		}

		private static int DefineLength(string text, Match match, int start)
		{
			int targetLength = match.Length + PREVIEW_LETTERS*2;
			int length = targetLength;
			if (start + targetLength > text.Length)
			{
				length = text.Length - start;
			}

			return length;
		}

		private static int DefineStartPosition(Match match)
		{
			int start = 0;
			if (match.Index - PREVIEW_LETTERS > 0)
			{
				start = match.Index - PREVIEW_LETTERS;
			}
			return start;
		}
	}
}