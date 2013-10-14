// 
// Copyright (c) 2005-2013 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using Hd.Portal;
using Hd.Portal.Components;
using Hd.Portal.Components.LastActionProcessor;
using log4net;

namespace Hd.Web.Extensions
{
	public class PersisterBasePage : Page
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(PersisterBasePage));

		public virtual bool IsRequiredToSetTitle
		{
			get { return true; }
		}

		public virtual bool IsLoginPage
		{
			get
			{
				return false;
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			if (!Requester.IsLogged && !Requester.IsLoggedAsAnonymous && !IsLoginPage && Settings.IsPublicMode)
			{
				FormsAuthentication.RedirectToLoginPage();
			}

			Response.Expires = -1;
			Response.Cache.SetNoStore();

			if (IsRequiredToSetTitle)
				Title = Settings.Title;

			Error += HandleError;

			base.OnLoad(e);
		}

		public void HandleError(object sender, EventArgs args)
		{
			log.Error("The error is occured", Server.GetLastError());
			ActionProcessor.LastAction = Server.GetLastError().Message;
			ActionProcessor.GetHolder().IsError = true;
			Application["LastError"] = Server.GetLastError();
		}

		public override Control FindControl(string id)
		{
			WebPartManager manager = WebPartManager.GetCurrentWebPartManager(Page);
			if (manager != null)
			{
				Control control = manager.FindControl(id);
				if (control != null)
				{
					return control;
				}
			}
			return base.FindControl(id);
		}
	}
}
