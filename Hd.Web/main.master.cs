// 
// Copyright (c) 2005-2011 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hd.Portal;
using Hd.Web.Extensions.Components;

public partial class main : MasterPage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		ScriptManager manager = ScriptManager.GetCurrent(Page);
		if (manager != null)
		{
			manager.Scripts.Add(new ScriptReference("~/JavaScript/ext-base.js"));
			manager.Scripts.Add(new ScriptReference("~/JavaScript/ext-core.js"));
			manager.Scripts.Add(new ScriptReference("~/JavaScript/ext-all.js"));
		}

		var stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat(" var appHostAndPath = '{0}';", Globals.ApplicationHostAndPath);
		Page.ClientScript.RegisterStartupScript(GetType(), "extGif",
			string.Format("Ext.QuickTips.init(); Ext.BLANK_IMAGE_URL = '{0}/App_Themes/Main/Ext/resources/images/default/s.gif';",
				Globals.ApplicationHostAndPath), true);

		Requester requester = Requester.Logged;

		if (!ReferenceEquals(requester, null))
			lblLoginName.Text = requester.Email;

		if (Requester.IsLoggedAsAnonymous)
		{
			lblLoginName.Text = "Guest";
			settingsLink.NavigateUrl = "";
		}

		spanUsername.Visible = !String.IsNullOrEmpty(lblLoginName.Text.Trim());

		tblSearch.Visible = Requester.IsLogged || Requester.IsLoggedAsAnonymous;
	}

	protected void btnSearch_Click(object sender, EventArgs e)
	{
		if (txtSearch.Text == string.Empty)
		{
			return;
		}

		string url = "~/Search.aspx";
		string param = "SearchString=" + txtSearch.Text;
		url = Globals.AppendQueryParameters(url, param);
		Response.Redirect(Globals.ResolveClientUrl(url));
	}

	protected void loginStatus_LoggingOut(object sender, LoginCancelEventArgs e)
	{
		if(Response.Cookies[Globals.PASSWORD_COOKIE] != null)
		{
			Response.Cookies.Set(new HttpCookie(Globals.PASSWORD_COOKIE, ""));
		}		
		Session.Clear();
		Globals.IsLogOut = false;
		FormsAuthentication.RedirectToLoginPage();
	}
}
