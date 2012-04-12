using System;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hd.Portal;
using Hd.Portal.Components.LastActionProcessor;
using Hd.Web.Extensions;
using Hd.Web.Extensions.Components;

public partial class RegisterPage : PersisterBasePage
{
	protected override void OnInit(EventArgs e)
	{
		requesterSource.Updated += requestSource_Updated;
		base.OnInit(e);
	}

	private void requestSource_Updated(object sender, TpObjectDataSourceEventArgs e)
	{
		Requester requester = (Requester)e.BusinessObject;
		
		ActionProcessor.ReplaceLastAction("You were registered successfully");

		LastActionLabel.DoReset = false;
        
		FormsAuthentication.RedirectFromLoginPage(requester.ID.ToString(),false);
        Globals.IsLogOut = false;
	}

    protected void OnUpdated(object sender, TpObjectDataSourceEventArgs e)
    {}

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
    }

    protected void OnValidateEmail(object source, ServerValidateEventArgs args)
    {
        if (string.IsNullOrEmpty(args.Value))
            return;

        args.IsValid = Requester.FindByEmail(args.Value) == null;
    }
}