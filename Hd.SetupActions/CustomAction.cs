//  
// Copyright (c) 2005-2009 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Xml;

namespace Hd.SetupActions
{
	[RunInstaller(true)]
	public partial class CustomAction : Installer
	{
		public CustomAction()
		{
			InitializeComponent();
		}

		public override void Install(IDictionary stateSaver)
		{
			base.Install(stateSaver);

			// /url="[URL]" /title="[TITLE]" /login="[LOGIN]" /password="[PASSWORD]" /targetdir="[TARGETDIR]\"
			string url = Context.Parameters["url"];
			string title = Context.Parameters["title"];
			string login = Context.Parameters["login"];
			string password = Context.Parameters["password"];
			string targetdir = Context.Parameters["targetdir"];

			List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

			values.Add(new KeyValuePair<string, string>("TargetProcessPath", url));
			values.Add(new KeyValuePair<string, string>("Title", title));
			values.Add(new KeyValuePair<string, string>("AdminLogin", login));
			values.Add(new KeyValuePair<string, string>("AdminPassword", password));

			WebConfigInstaller.SetAppSettingsValue(targetdir, values);
		}

		private class WebConfigInstaller
		{
			public static void SetAppSettingsValue(string targetDir, List<KeyValuePair<string, string>> values)
			{
				XmlDocument webConfig = new XmlDocument();
				string webConfigPath = Path.Combine(targetDir, "Web.config");
				webConfig.Load(webConfigPath);

				XmlNode apSettingsNode = webConfig.SelectSingleNode("configuration/appSettings");
				foreach (KeyValuePair<string, string> keyValuePair in values)
				{
					XmlNode xmlNode = apSettingsNode.SelectSingleNode(string.Format("add[@key='{0}']", keyValuePair.Key));
					if (xmlNode == null)
					{
						XmlElement xmlElement = apSettingsNode.OwnerDocument.CreateElement("add");
						xmlElement.SetAttribute("key", keyValuePair.Key);
						apSettingsNode.AppendChild(xmlElement);
						xmlNode = xmlElement;
					}
					(xmlNode as XmlElement).SetAttribute("value", keyValuePair.Value);
				}
				webConfig.Save(webConfigPath);
			}
		}
	}
}