// 
// Copyright (c) 2005-2011 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System.Web;
using FredCK.FCKeditorV2;

namespace Hd.Web.Extensions
{
	public class FCKEditorRichEditControl : FCKeditor, IRichEditControl
	{
		public FCKEditorRichEditControl()
		{
			BasePath = VirtualPathUtility.ToAbsolute("~/fckeditor/");
			Config["EnterMode"] = "br";

			HttpContext.Current.Session["FCKeditor:UserFilesPath"] = VirtualPathUtility.ToAbsolute("~/upload/");

			Mode = RichMode.Full;
		}

		public string EditorClientID
		{
			get { return base.ClientID; }
		}

		public int MaxLength
		{
			get { return 8000; }
			set { }
		}

		public RichMode Mode
		{
			set
			{
				switch (value)
				{
					case RichMode.Full:
						ConfigurationPath = "~/fckeditor/Configuration/TpFull.js";
						break;
					case RichMode.Simple:
						ConfigurationPath = "~/fckeditor/Configuration/TpSimple.js";
						break;
					case RichMode.SimpleWithTable:
						ConfigurationPath = "~/fckeditor/Configuration/TpSimpleWithTable.js";
						break;
					default:
						ConfigurationPath = "~/fckeditor/Configuration/TpSimple.js";
						break;
				}

				Config["CustomConfigurationsPath"] = VirtualPathUtility.ToAbsolute(ConfigurationPath); // +"?" + DateTime.Now.Ticks; ;
			}
		}

		public string Text
		{
			get { return Value; }
			set { Value = value; }
		}

		public string ConfigurationPath { get; set; }
	}
}