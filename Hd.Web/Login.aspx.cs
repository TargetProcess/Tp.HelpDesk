// 
// Copyright (c) 2005-2013 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using System.Globalization;
using System.Web.Security;
using System.Web.UI.WebControls;
using Hd.Portal;
using Hd.Portal.Components;
using Hd.Web.Extensions;
using Hd.Web.Extensions.Components;

public partial class TpLogin : PersisterBasePage
{
	public override bool IsLoginPage
	{
		get { return true; }
	}

	protected override void OnInit( EventArgs e )
	{
		base.OnInit(e);
		btnLoginAsGuest.Visible = Settings.IsPublicMode;
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		UserName.Focus();
		Response.Expires = 0;
		TryAutoLogin();
	}

	private void TryLoginAnonymously()
	{
		if(Settings.IsPublicMode)
			FormsAuthentication.RedirectFromLoginPage(Requester.ANONYMOUS_USER_ID.ToString(CultureInfo.InvariantCulture), false);
	}

	private void TryAutoLogin()
	{
		if (IsPostBack)
			return;

		if (Globals.IsLogOut || Requester.IsLoggedAsAnonymous)
		{
			Globals.IsLogOut = false;
			return;
		}

		TryLoginAnonymously();
	}

	private void PerformLogin(Requester requester)
	{
		DataPortal.Instance.ResetCachedValue(typeof(Requester), requester.ID);

		FormsAuthentication.RedirectFromLoginPage(requester.ID.GetValueOrDefault().ToString(CultureInfo.InvariantCulture), RememberMe.Checked);

		Globals.IsLogOut = false;
	}

	protected void OnLogin(object sender, CommandEventArgs e)
	{
		switch (e.CommandName)
		{
			case "LoginAsGuest":
				Response.Redirect("~/");
				break;
			case "Login":
				if(Requester.Validate(UserName.Text, Password.Text))
				{
					Requester user = Requester.FindByEmail( UserName.Text );
					PerformLogin(user);
				}
				else
					FailureText.Text = "Login failed. Most likely you have entered incorrect login or password.";
				break;
		}
	}

	private void ShowPasswordSentPanel()
	{
		loginPanel.Visible = false;
		forgotPasswordPanel.Visible = false;
		passwordSentPanel.Visible = true;
	}

	protected void OnSendPasswordButtonClick(object sender, EventArgs e)
	{
		HideLoginPanel();
		try
		{
			bool result = Requester.ForgotPassword(emailToSendPassword.Text);
			if (result)
				ShowPasswordSentPanel();
			ShowErrorMessage(string.Format("The User with email '{0}' does not exist.", emailToSendPassword.Text));
		}
		catch (Exception ex)
		{
			ShowErrorMessage(ex.Message);
		}
	}

	private void ShowErrorMessage(string message)
	{
		errorMessage.Text = message;
		errorMessage.Visible = true;
	}

	private void HideLoginPanel()
	{
		loginPanel.Visible = false;
		forgotPasswordPanel.Visible = true;
	}

	protected void forgotPassword_OnForgotPassword(object sender, EventArgs e)
	{
		HideLoginPanel();
	}
}
