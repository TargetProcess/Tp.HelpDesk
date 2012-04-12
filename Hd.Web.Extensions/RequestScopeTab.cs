using System;
using System.Web.UI.WebControls;
using Hd.Portal;
using Hd.Portal.Components;

namespace Hd.Web.Extensions
{
	public class RequestScopeTab : Panel
	{
		private string _text;		
		private string _url;
		private RequestScope _scope;

		public RequestScope Scope
		{
			get { return _scope; }
			set { _scope = value; }
		}

		public string Url
		{
			get { return _url; }
			set { _url = value; }
		}

		public string Text
		{
			get { return _text; }
			set { _text = value; }
		}

		public bool IsPublic { get; set; }	

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			Visible = false;
			if( !Requester.IsLogged && !(Requester.IsLoggedAsAnonymous) )
				return;
			if (Settings.Scope == RequestScope.Private)
			{
				if(Scope == RequestScope.Private )
					Visible = IsPublic ? true : Requester.IsLogged;
				else
					Visible = false;
 			}
			else
			{
				Visible = IsPublic ? true : Requester.IsLogged;
			}
		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls();
			Controls.Clear();
			HyperLink hyperLink = new HyperLink();
			hyperLink.Text = _text;
			hyperLink.NavigateUrl = _url;
			Controls.Add(hyperLink);
		}
	}
}