// 
// Copyright (c) 2005-2011 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using Hd.Portal;
using Hd.Portal.Components;
using Hd.Web.Extensions;
using Hd.Web.Extensions.Components;

public partial class TpLogin : PersisterBasePage
{
	private bool _isAutoLogin;

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
			FormsAuthentication.RedirectFromLoginPage( Requester.ANONYMOUS_USER_ID.ToString(), false );			
	}


	private void TryAutoLogin()
	{
		HttpCookie loginCookie = Request.Cookies.Get(Globals.LOGIN_COOKIE);
		HttpCookie passwordCookie = Request.Cookies.Get(Globals.PASSWORD_COOKIE);

		if (IsPostBack)
			return;

		if (loginCookie != null)
			UserName.Text = loginCookie.Value;

		if (Globals.IsLogOut || Requester.IsLoggedAsAnonymous)
		{
			Globals.IsLogOut = false;
			return;
		}

		if (loginCookie != null && passwordCookie != null && passwordCookie.Value != string.Empty
		    && Requester.Validate(loginCookie.Value, passwordCookie.Value))
		{
			_isAutoLogin = true;
			Requester user = Requester.FindByEmail(loginCookie.Value);
			int? userID = user.ID;
			PerformLogin(userID);
			return;
		}

		TryLoginAnonymously();
	}

	private void PerformLogin(int? userID)
	{
		DataPortal.Instance.ResetCachedValue(typeof (Requester), userID);

		FormsAuthentication.RedirectFromLoginPage(userID.Value.ToString(), false);

		Globals.IsLogOut = false;

		if (!_isAutoLogin)
			CreateCookieForLogin();
	}

	private void CreateCookieForLogin()
	{
		CreateCookie(Globals.LOGIN_COOKIE, UserName.Text);
		CreateCookie(Globals.PASSWORD_COOKIE, RememberMe.Checked ? Password.Text : string.Empty);
	}

	private void CreateCookie(string cookieName, string cookieValue)
	{
		var cookie = new HttpCookie(cookieName, cookieValue) {Expires = DateTime.Now.AddYears(1)};
		Response.Cookies.Add(cookie);
	}

	protected void OnLogin(object sender, CommandEventArgs e)
	{
		switch (e.CommandName)
	{
			case "LoginAsGuest":
				Response.Redirect("~/");
				break;
			case "Login":
				if( Requester.Validate( UserName.Text, Password.Text ) )
		{
					Requester user = Requester.FindByEmail( UserName.Text );
			int? userId = user.ID;
					PerformLogin( userId );
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
