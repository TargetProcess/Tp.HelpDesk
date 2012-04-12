using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Hd.Portal;
using Hd.Portal.Components.LastActionProcessor;
using Hd.Web.Extensions;

public partial class UserProfile : PersisterBasePage
{

	protected override void OnLoad(EventArgs e)
	{
		if (Requester.IsLoggedAsAnonymous)
			Response.Redirect("~/Default.aspx");
		base.OnLoad(e);
	}
	protected void OnValidateEmail(object source, ServerValidateEventArgs args)
	{
	 	if (string.IsNullOrEmpty(args.Value))
			return;

		args.IsValid = Requester.Logged.CanChangeEmailTo(args.Value); 
	}


	

	protected void OnUpdatedItem(object sender, FormViewUpdatedEventArgs e)
	{
		if (e.Exception != null)
		{
			e.ExceptionHandled = true;
			e.KeepInEditMode = true;
			ActionProcessor.IsError = true;

			Exception exception = e.Exception;

			while (exception.InnerException != null)
				exception = exception.InnerException;

			ActionProcessor.ReplaceLastAction(exception.Message);
		}
		Response.Redirect(Page.Request.Url.AbsolutePath);

	}

	protected void requesterSource_SourceObject(object sender, TpObjectDataSourceEventArgs e)
	{
		if (Requester.Logged != null)
			e.BusinessObject = Requester.Logged;
		else
			e.Cancel = true;

	}

 
}
