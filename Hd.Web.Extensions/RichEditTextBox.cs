// 
// Copyright (c) 2005-2011 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tp.Web.Extensions.Components;

namespace Hd.Web.Extensions
{
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:RichEditTextBox runat=server></{0}:RichEditTextBox>")]
	public class RichEditTextBox : WebControl, ITextControl
	{
		private readonly IRichEditControl _richEditControl;

		public RichEditTextBox()
		{
			_richEditControl = new FCKEditorRichEditControl();
		}

		public RichMode Mode
		{
			set { _richEditControl.Mode = value; }
		}

		public string EditorClientID
		{
			get
			{
				return _richEditControl.EditorClientID;
			}
		}

		public new Unit Height
		{
			get { return _richEditControl.Height; }
			set { _richEditControl.Height = value; }
		}

		public new Unit Width
		{
			get { return _richEditControl.Width; }
			set { _richEditControl.Width = value; }
		}

		public int MaxLength
		{
			get { return _richEditControl.MaxLength; }
			set { _richEditControl.MaxLength = value; }
		}

		#region ITextControl Members

		//UNODONE:yia
		public string Text
		{
			get
			{
				return Regex.Replace(_richEditControl.Text, @"(<img[^>]*src=['""])(" + VirtualPathUtility.ToAbsolute("~/") + @")([^""']*?['""][^>]*?>)",
												"$1" + "~/" + "$3", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
			}
			set
			{
				_richEditControl.Text = Regex.Replace(Sanitizer.Sanitize(value), @"(<img[^>]*src=['""])(&#126;&#47;|~/)([^""']*?['""][^>]*?>)",
									 "$1" + VirtualPathUtility.ToAbsolute("~/") + "$3", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
			}
		}

		#endregion

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			EnsureChildControls();
		}

		private void SetRichEditControlID()
		{
			var control = _richEditControl as Control;
			if (control != null)
			{
				control.ID = String.Format("{0}_ckeditor", ID);
			}
		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls();
			SetRichEditControlID();
			Controls.Add(_richEditControl as Control);
		}
	}
}
