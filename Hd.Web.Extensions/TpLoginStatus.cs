using System;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hd.Portal;
using Hd.Portal.Components;
using Hd.Web.Extensions.Components;

namespace Hd.Web.Extensions
{
	public class TpLoginStatus : CompositeControl
	{
		// Fields
		private static readonly object EventLoggedOut = new object();
		private static readonly object EventLoggingOut = new object();
		private ImageButton _logInImageButton;
		private LinkButton _logInLinkButton;
		private ImageButton _logOutImageButton;
		private LinkButton _logOutLinkButton;
		private bool _loggedIn;

		private bool LoggedIn
		{
			get { return _loggedIn; }
			set { _loggedIn = value; }
		}

		public virtual string LoginImageUrl
		{
			get
			{
				object obj2 = ViewState["LoginImageUrl"];
				if (obj2 != null)
				{
					return (string) obj2;
				}
				return string.Empty;
			}
			set { ViewState["LoginImageUrl"] = value; }
		}


		public virtual string LoginText
		{
			get
			{
				object obj2 = ViewState["LoginText"];
				if (obj2 != null)
				{
					return (string) obj2;
				}
				return "Login";
			}
			set { ViewState["LoginText"] = value; }
		}

		public virtual LogoutAction LogoutAction
		{
			get
			{
				object obj2 = ViewState["LogoutAction"];
				if (obj2 != null)
				{
					return (LogoutAction) obj2;
				}
				return LogoutAction.Refresh;
			}
			set
			{
				if ((value < LogoutAction.Refresh) || (value > LogoutAction.RedirectToLoginPage))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				ViewState["LogoutAction"] = value;
			}
		}

		public virtual string LogoutImageUrl
		{
			get
			{
				object obj2 = ViewState["LogoutImageUrl"];
				if (obj2 != null)
				{
					return (string) obj2;
				}
				return string.Empty;
			}
			set { ViewState["LogoutImageUrl"] = value; }
		}

		public virtual string LogoutPageUrl
		{
			get
			{
				object obj2 = ViewState["LogoutPageUrl"];
				if (obj2 != null)
				{
					return (string) obj2;
				}
				return string.Empty;
			}
			set { ViewState["LogoutPageUrl"] = value; }
		}

		public virtual string LogoutText
		{
			get
			{
				object obj2 = ViewState["LogoutText"];
				if (obj2 != null)
				{
					return (string) obj2;
				}
				return "Logout";
			}
			set { ViewState["LogoutText"] = value; }
		}		

		protected override HtmlTextWriterTag TagKey
		{
			get { return HtmlTextWriterTag.A; }
		}

		// Events		
		public event EventHandler LoggedOut
		{
			add { base.Events.AddHandler(EventLoggedOut, value); }
			remove { base.Events.RemoveHandler(EventLoggedOut, value); }
		}

		public event LoginCancelEventHandler LoggingOut
		{
			add { base.Events.AddHandler(EventLoggingOut, value); }
			remove { base.Events.RemoveHandler(EventLoggingOut, value); }
		}

		// Methods
		protected override void CreateChildControls()
		{
			Controls.Clear();
			_logInLinkButton = new LinkButton();
			_logInImageButton = new ImageButton();
			_logOutLinkButton = new LinkButton();
			_logOutImageButton = new ImageButton();
			_logInLinkButton.EnableViewState = false;
			_logInImageButton.EnableViewState = false;
			_logOutLinkButton.EnableViewState = false;
			_logOutImageButton.EnableViewState = false;
			_logInLinkButton.EnableTheming = false;
			_logInImageButton.EnableTheming = false;
			_logInLinkButton.CausesValidation = false;
			_logInImageButton.CausesValidation = false;
			_logOutLinkButton.EnableTheming = false;
			_logOutImageButton.EnableTheming = false;
			_logOutLinkButton.CausesValidation = false;
			_logOutImageButton.CausesValidation = false;
			var handler = new CommandEventHandler(LogoutClicked);
			_logOutLinkButton.Command += handler;
			_logOutImageButton.Command += handler;
			handler = new CommandEventHandler(LoginClicked);
			_logInLinkButton.Command += handler;
			_logInImageButton.Command += handler;
			Controls.Add(_logOutLinkButton);
			Controls.Add(_logOutImageButton);
			Controls.Add(_logInLinkButton);
			Controls.Add(_logInImageButton);
		}

		private void LoginClicked(object Source, CommandEventArgs e)
		{
			this.Page.Response.Redirect(FormsAuthentication.LoginUrl);			
		}

		private void LogoutClicked(object Source, CommandEventArgs e)
		{
			var args = new LoginCancelEventArgs();
			OnLoggingOut(args);
			if (!args.Cancel)
			{
				FormsAuthentication.SignOut();
				Page.Response.Clear();
				Page.Response.StatusCode = 200;
				OnLoggedOut(EventArgs.Empty);
				switch (LogoutAction)
				{
					case LogoutAction.Refresh:
						if ((Page.Form == null) || !string.Equals(Page.Form.Method, "get", StringComparison.OrdinalIgnoreCase))
						{
							Page.Response.Redirect(Page.Request.Url.PathAndQuery, false);
							return;
						}
						Page.Response.Redirect(Page.Request.Path, false);
						return;

					case LogoutAction.Redirect:
						{
							string logoutPageUrl = LogoutPageUrl;
							if (string.IsNullOrEmpty(logoutPageUrl))
							{
								logoutPageUrl = FormsAuthentication.LoginUrl;
							}
							else
							{
								logoutPageUrl = base.ResolveClientUrl(logoutPageUrl);
							}
							Page.Response.Redirect(logoutPageUrl, false);
							return;
						}
					case LogoutAction.RedirectToLoginPage:
						Page.Response.Redirect(FormsAuthentication.LoginUrl, false);
						return;
				}
			}
		}

		protected virtual void OnLoggedOut(EventArgs e)
		{
			var handler = (EventHandler) base.Events[EventLoggedOut];
			if (handler != null)
			{
				handler(this, e);
			}
		}

		protected virtual void OnLoggingOut(LoginCancelEventArgs e)
		{
			var handler = (LoginCancelEventHandler) base.Events[EventLoggingOut];
			if (handler != null)
			{
				handler(this, e);
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);			

			LoggedIn = Settings.IsPublicMode 
						? Page.Request.IsAuthenticated && ( HttpContext.Current.User.Identity.Name != Requester.ANONYMOUS_USER_ID.ToString() ) 
						: Page.Request.IsAuthenticated;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			RenderContents(writer);
		}

		protected override void RenderContents(HtmlTextWriter writer)
		{
			if (Page != null)
			{
				Page.VerifyRenderingInServerForm(this);
			}
			SetChildProperties();
			if (!string.IsNullOrEmpty(ID))
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
			}
			base.RenderContents(writer);
		}

		private void SetChildProperties()
		{
			EnsureChildControls();
			_logInLinkButton.Visible = false;
			_logInImageButton.Visible = false;
			_logOutLinkButton.Visible = false;
			_logOutImageButton.Visible = false;
			WebControl control = null;
			if (LoggedIn)
			{
				string logoutImageUrl = LogoutImageUrl;
				if (logoutImageUrl.Length > 0)
				{
					_logOutImageButton.AlternateText = LogoutText;
					_logOutImageButton.ImageUrl = logoutImageUrl;
					control = _logOutImageButton;
				}
				else
				{
					_logOutLinkButton.Text = LogoutText;
					control = _logOutLinkButton;
				}
			}
			else
			{
				string loginImageUrl = LoginImageUrl;
				if (loginImageUrl.Length > 0)
				{
					_logInImageButton.AlternateText = LoginText;
					_logInImageButton.ImageUrl = loginImageUrl;
					control = _logInImageButton;
				}
				else
				{
					_logInLinkButton.Text = LoginText;
					control = _logInLinkButton;
				}
			}
			control.CopyBaseAttributes(this);
			control.ApplyStyle(base.ControlStyle);
			control.Visible = true;
		}
	}
}
