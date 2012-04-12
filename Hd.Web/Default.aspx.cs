using System;
using Hd.Portal;
using Hd.Web.Extensions;

public partial class Default : PersisterBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		phMessage.Visible = !Requester.IsLogged;
	}
}