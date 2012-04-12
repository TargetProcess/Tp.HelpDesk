// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tp.Web.Extensions.Components
{
	public static class Encoder
	{
		/// <summary>
		/// Encodes the specified <paramref name="input"/> 
		/// and writes encoded text for use in HTML to the specified <paramref name="output"/>.
		/// </summary>
		/// <param name="input">
		/// The text to encode, may be <c>null</c>.
		/// </param>
		/// <param name="output">
		/// Output stream where to write encoded text.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="output"/> is <c>null</c>.
		/// </exception>
		public static void HtmlEncode(string input, TextWriter output)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (string.IsNullOrEmpty(input))
			{
				return;
			}
			foreach (char ch in input)
			{
				HtmlEncode(ch, output);
			}
		}

		/// <summary>
		/// Reads text from the specified <paramref name="input"/> 
		/// and writes encoded text for use in HTML to the specified <paramref name="output"/>.
		/// </summary>
		/// <param name="input">
		/// Input stream containing text to encode.
		/// </param>
		/// <param name="output">
		/// Output stream where to write encoded text.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="input"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// If <paramref name="output"/> is <c>null</c>.
		/// </exception>
		public static void HtmlEncode(TextReader input, TextWriter output)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			int ch;
			while ((ch = input.Read()) != -1)
			{
				HtmlEncode(ch, output);
			}
		}

		/// <summary>
		/// Encodes the specified input string for use in HTML.
		/// </summary>
		/// <remarks>
		/// This method is <c>null</c> safe. 
		/// </remarks>
		/// <param name="input">
		/// The string to encode, may be <c>null</c>.
		/// </param>
		/// <returns>
		/// <c>null</c> if <paramref name="input"/> is <c>null</c>, encoded string otherwise.
		/// </returns>
		public static string HtmlEncode(string input)
		{
			if (input == null)
			{
				return null;
			}
			if (input == "")
			{
				return "";
			}
			var output = new StringWriter();
			output.GetStringBuilder().EnsureCapacity(input.Length*2);
			HtmlEncode(input, output);
			return output.ToString();
		}

		public static void HtmlAttributeEncode(string input, TextWriter output)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (string.IsNullOrEmpty(input))
			{
				return;
			}
			foreach (char ch in input)
			{
				HtmlAttributeEncode(ch, output);
			}
		}

		public static void HtmlAttributeEncode(TextReader input, TextWriter output)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			int ch;
			while ((ch = input.Read()) != -1)
			{
				HtmlAttributeEncode(ch, output);
			}
		}

		public static string HtmlAttributeEncode(string input)
		{
			if (input == null)
			{
				return null;
			}
			if (input.Length == 0)
			{
				return "";
			}
			var output = new StringWriter();
			output.GetStringBuilder().EnsureCapacity(input.Length*2);
			HtmlAttributeEncode(input, output);
			return output.ToString();
		}

		private static void HtmlEncode(int ch, TextWriter output)
		{
			if (Char.IsWhiteSpace((char) ch))
			{
				output.Write((char) ch);
			}
			else if (ch == '<')
			{
				output.Write("&lt;");
			}
			else if (ch == '>')
			{
				output.Write("&gt;");
			}
			else if (ch == '"')
			{
				output.Write("&quot;");
			}
			else if (
				(ch > '`' && ch < '{')
				|| (ch > '@' && ch < '[')
				|| (ch > '/' && ch < ':')
				|| (ch == '.' || ch == '-' || ch == '_')
				)
			{
				output.Write((char) ch);
			}
			else
			{
				output.Write("&#");
				output.Write(ch);
				output.Write(";");
			}
		}

		private static void HtmlAttributeEncode(int ch, TextWriter output)
		{
			if (Char.IsWhiteSpace((char) ch))
			{
				output.Write((char) ch);
			}
			else if (ch == '<')
			{
				output.Write("&lt;");
			}
			else if (ch == '>')
			{
				output.Write("&gt;");
			}
			else if (ch == '"')
			{
				output.Write("&quot;");
			}
			else if ((ch > '`' && ch < '{')
			         || (ch > '@' && ch < '[')
			         || (ch > '/' && ch < ':')
			         || (ch == '.' || ch == ',' || ch == '-' || ch == '_')
				)
			{
				output.Write((char) ch);
			}
			else
			{
				output.Write("&#");
				output.Write(ch);
				output.Write(";");
			}
		}
	}
}