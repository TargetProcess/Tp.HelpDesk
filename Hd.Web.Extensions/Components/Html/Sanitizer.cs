// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;


namespace Tp.Web.Extensions.Components
{
	/// <summary>
	/// Clean up HTML code, remove dangerous fragments, such as styles, scripts, event attributes, forms, etc...
	/// </summary>
	/// <remarks>
	/// IDEA: Use <see cref="System.Web.UI.HtmlTextWriter">HtmlTextWriter from the .NET framework</see> to write resulting HTML code.
	/// </remarks>
	public class Sanitizer
	{
		/// <summary>
		/// Tags whose outer html to be suppressed. In other words, entire tag with its content will be suppressed.
		/// </summary>
		private readonly HashSet<string> ExcludeTags = new HashSet<string>
		{
			"head", "base", "basefont", "meta", "link", "title",
			"style",
			"script",
			"input", "isindex", "textarea", "button", "option", "select",
			"frameset", "frame", "iframe",
			"object", "embed", "applet", "bgsound",
		};

		/// <summary>
		/// Overrides suppression caused by the tags above, enables outer html.
		/// </summary>
		private readonly HashSet<string> IncludeTags = new HashSet<string>
		{
			"body",
		};

		/// <summary>
		/// Write inner instead of outer html for these tags. In other words, write tag content without tag.
		/// </summary>
		private readonly ISet<string> IgnoreTags = new HashSet<string>
		{
			"html", "body", "form", "blink", "plaintext",
		};

		/// <summary>
		/// These tags need not be closed explicitly.
		/// </summary>
		private readonly ISet<string> EmptyTags = new HashSet<string>
		{
			"base", "basefont", "meta", "link",
			"br", "hr",
			"input", "isindex",
			"img",
			"col",
			"frame",
			"param",
		};

		/// <summary>
		/// These tags may not be explicitly closed, but presense of a new open tag automaticaly closes the previously open but not closed tag.
		/// </summary>
		private readonly ISet<string> AutoClosedTags = new HashSet<string>
		{
			"p", "li",
		};

		/// <summary>
		/// Rewrite some (obsolete, deprecated) tags to another tags.
		/// </summary>
		private readonly IDictionary<string, string> RewriteTags = new Dictionary<string, string> { { "plaintext", "pre" }, };

		/// <summary>
		/// Attributes to be suppressed.
		/// </summary>
		private readonly ISet<string> EventAttributes = new HashSet<string>
		{
			"onabort", "onblur", "onchange", "onclick", "ondblclick",
			"ondragdrop", "onerror", "onfocus", "onkeydown", "onkeypress",
			"onkeyup", "onload", "onmousedown", "onmousemove", "onmouseout",
			"onmouseover", "onmouseup", "onmove", "onreset", "onresize",
			"onselect", "onsubmit", "onunload",
		};

		public Sanitizer()
		{
			RemoveStyles = true;
			RemoveForms = false;
			RemoveIds = true;
			OuterTag = null;
		}

		/// <summary>
		/// Remove <em>&ltstyle...&gt;</em> elements from html.
		/// </summary>
		public bool RemoveStyles { get; set; }

		/// <summary>
		/// Remove <em>&ltform...&gt;</em> and <em>&ltinput...&gt;</em> elements from html.
		/// </summary>
		public bool RemoveForms { get; set; }

		/// <summary>
		/// Remove <em>&ltid=&quot...&quot&gt;</em> attributes from html to avoid duplicate ids.
		/// </summary>
		public bool RemoveIds { get; set; }

		/// <summary>
		/// Wrap result with outer tag, e.g. <em>&ltdiv&gt;sanitized markup&lt/div&gt;</em>.
		/// </summary>
		public string OuterTag { get; set; }

		/// <summary>
		/// Stack with tags.
		/// </summary>
		protected readonly List<string> _tags = new List<string>();

		/// <summary>
		/// Whether to write elements.
		/// </summary>
		protected bool _enabled = true;

		/// <summary>
		/// Sanitize input HTML using default settings.
		/// </summary>
		/// <param name="input">Input HTML. May be <c>null</c>.</param>
		/// <returns>Sanitized HTML.</returns>
		public static string Sanitize(string input)
		{
			if (input == null)
			{
				return null;
			}
			
			var inputReader = new StringReader(input);
			var resultWriter = new StringWriter();
			resultWriter.GetStringBuilder().EnsureCapacity(input.Length + 32);
			resultWriter.NewLine = "\n";
			var sanitizer = new Sanitizer();
			sanitizer.Sanitize(inputReader, resultWriter);
			return resultWriter.ToString();
		}

		public void Sanitize(TextReader input, TextWriter result)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (result == null)
			{
				throw new ArgumentNullException("result");
			}
			using (var htmlReader = new HtmlReader(input))
			{
				Sanitize(htmlReader, result);
			}
			Reset();
		}

		/// <summary>
		/// Reset internal state left from previous run.
		/// </summary>
		protected virtual void Reset()
		{
			_tags.Clear();
			_enabled = true;
		}

		protected virtual void Sanitize(HtmlReader htmlReader, TextWriter result)
		{
			Before(result);
			while (htmlReader.Read())
			{
				switch (htmlReader.NodeType)
				{
					case HtmlNodeType.Element:
						CaseElement(htmlReader, result);
						break;

					case HtmlNodeType.EndElement:
						CaseEndElement(htmlReader, result);
						break;

					case HtmlNodeType.Text:
						CaseText(htmlReader, result);
						break;

					case HtmlNodeType.CDATA:
						CaseCData(htmlReader, result);
						break;
				}
			}
			// close unclosed tags
			while (_tags.Count > 0)
			{
				string tagName = PopTag();
				CheckStack();
				if (_enabled && !IgnoreTags.Contains(tagName))
				{
					WriteEndElement(result, tagName);
				}
			}
			After(result);
			result.Flush();
		}

		protected virtual void Before(TextWriter result)
		{
			if (!string.IsNullOrEmpty(OuterTag))
			{
				WriteElement(result, OuterTag, new StringDictionary(), false);
			}
		}

		protected virtual void After(TextWriter result)
		{
			if (!string.IsNullOrEmpty(OuterTag))
			{
				WriteEndElement(result, OuterTag);
			}
		}

		protected virtual void CaseElement(HtmlReader htmlReader, TextWriter result)
		{
			string tagName = htmlReader.Name.ToLowerInvariant();
			// automatically close unclosed tag, if any
			if (AutoClosedTags.Contains(tagName))
			{
				if (FuzzyPopTag(tagName))
				{
					WriteEndElement(result, tagName);
				}
			}
			// reevaluate tags stack
			PushTag(tagName);
			CheckStack();
			if (_enabled && !IgnoreTags.Contains(tagName))
			{
				WriteElement(result, tagName, htmlReader.Attributes, htmlReader.IsEmptyElement);
			}
			// pop empty tags immediately
			if (htmlReader.IsEmptyElement || EmptyTags.Contains(tagName))
			{
				if (FuzzyPopTag(tagName))
				{
					CheckStack();
				}
			}
		}

		protected virtual void CaseEndElement(HtmlReader htmlReader, TextWriter result)
		{
			string tagName = htmlReader.Name.ToLowerInvariant();
			if (FuzzyPopTag(tagName))
			{
				// only close tag if it was previously open
				if (_enabled && !IgnoreTags.Contains(tagName))
				{
					WriteEndElement(result, tagName);
				}
				CheckStack();
			}
		}

		protected virtual void CaseText(HtmlReader htmlReader, TextWriter result)
		{
			if (_enabled)
			{
				WriteString(result, htmlReader.Value);
			}
		}

		protected virtual void CaseCData(HtmlReader htmlReader, TextWriter result)
		{
			if (_enabled)
			{
				WriteCData(result, htmlReader.Value);
			}
		}

		protected virtual void CheckStack()
		{
			_enabled = true;
			int count = _tags.Count;
			for (int i = count; i > 0; i--)
			{
				string tag = _tags[i - 1];
				if (IncludeTags.Contains(tag))
				{
					_enabled = true;
					return;
				}
				if (ExcludeTags.Contains(tag))
				{
					_enabled = false;
					return;
				}
			}
		}

		protected virtual void WriteElement(TextWriter result, string name, StringDictionary attributes, bool empty)
		{
			result.Write("<");
			result.Write(name);
			foreach (DictionaryEntry entry in attributes)
			{
				var key = ((string) entry.Key).ToLowerInvariant();
				var value = (string) entry.Value;
				if (IsValidAttribute(key, value))
				{
					result.Write(' ');
					result.Write(key);
					if (value != null)
					{
						result.Write('=');
						result.Write('"');
						Encoder.HtmlAttributeEncode(value, result);
						result.Write('"');
					}
				}
			}
			if (empty/* && !EmptyTags.Contains(name)*/)
			{
				result.Write(" /");
			}
			result.Write(">");
		}

		protected virtual bool IsValidAttribute(string key, string value)
		{
			// Suppress event attributes.
			if (EventAttributes.Contains(key))
			{
				return false;
			}
			// Suppress "href='javascript:...'" attributes.
			if (key == "href" && value.IndexOf("javascript", StringComparison.InvariantCultureIgnoreCase) != -1)
			{
				return false;
			}
			if (key == "id" && RemoveIds)
			{
				return false;
			}
			return true;
		}

		protected virtual void WriteEndElement(TextWriter result, string name)
		{
			result.Write("</");
			result.Write(name);
			result.Write(">");
		}

		protected virtual void WriteString(TextWriter result, string value)
		{
			Encoder.HtmlEncode(value, result);
		}

		protected virtual void WriteCData(TextWriter result, string value)
		{
			result.Write("<![CDATA[");
			result.Write(value);
			result.Write("]]>");
		}

		private void PushTag(string item)
		{
			_tags.Add(item);
		}

		protected string PopTag()
		{
			int index = _tags.Count - 1;
			string tag = _tags[index];
			_tags.RemoveAt(index);
			return tag;
		}

		/// <summary>
		/// Finds the specified tag somewhere in the stack and removes it from there.
		/// </summary>
		/// <param name="item">Item to remove.</param>
		/// <returns><c>true</c> if found and removed; <c>false</c> otherwise.</returns>
		protected bool FuzzyPopTag(string item)
		{
			int i = _tags.LastIndexOf(item);
			if (i != -1)
			{
				_tags.RemoveAt(i);
				return true;
			}
			return false;
		}

		protected string TopTag()
		{
			if (_tags.Count > 0)
			{
				return _tags[_tags.Count - 1];
			}
			return null;
		}

		#region Obsolete

		public static string TextToHtml(string text)
		{
			if (text == null)
			{
				return null;
			}

			// thext is already HTML
			if (Regex.Match(text, "<(html|body|br)[^>]*>", RegexOptions.IgnoreCase).Success)
			{
				return text;
			}

			string newText = text.Replace(Environment.NewLine, "<br />");
			newText = Regex.Replace(newText, "(>\\s*<br[^>]*>\\s*<)+", "><", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			return newText;
		}

		#endregion
	}
}