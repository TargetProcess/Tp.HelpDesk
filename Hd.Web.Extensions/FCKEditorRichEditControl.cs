// 
// Copyright (c) 2005-2014 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System.Web;
using CKEditor.NET;

namespace Hd.Web.Extensions
{
	public class FCKEditorRichEditControl : CKEditorControl, IRichEditControl
	{
		public FCKEditorRichEditControl()
		{
			BasePath = VirtualPathUtility.ToAbsolute("~/fckeditor/");
			config.enterMode = EnterMode.BR;

			HttpContext.Current.Session["FCKeditor:UserFilesPath"] = VirtualPathUtility.ToAbsolute("~/upload/");

			Mode = RichMode.Full;
		}

		public string EditorClientID
		{
			get { return base.ClientID; }
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

				config.customConfig = VirtualPathUtility.ToAbsolute(ConfigurationPath); // +"?" + DateTime.Now.Ticks; ;
			}
		}

		public string ConfigurationPath { get; set; }
	}
}