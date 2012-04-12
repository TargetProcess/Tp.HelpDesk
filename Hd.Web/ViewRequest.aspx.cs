using System.Collections;
using System.Web.Security;
using System.Linq;
using Hd.Portal;
using Hd.Portal.Components;
using Hd.Portal.Components.LastActionProcessor;
using Hd.Web.Extensions;
using Hd.Web.Extensions.Components;

public partial class ViewRequest : PersisterBasePage
{
	protected void requestSource_OnSelected(object sender, TpObjectDataSourceEventArgs e)
	{
		if (e.BusinessObject != null)
		{
			IEnumerator enumerator = ((IEnumerable) e.BusinessObject).GetEnumerator();
			if (enumerator.MoveNext())
			{
				var entity = enumerator.Current as Request;
				
				//If no entity found using current query and user is logged as Anonymous
				if (!entity.ID.HasValue && Requester.IsLoggedAsAnonymous)
				{
					
					if(e.SelectParams.Contains("RequestId"))
					{
						var requestId = e.SelectParams["RequestId"] as int?;
						if (requestId != null)
						{
							//If such entity exist - redirects to login page for try to access this request
							if (Hd.Portal.Request.Retrieve(requestId,true) != null)
							{
								Globals.IsLogOut = true;
								FormsAuthentication.RedirectToLoginPage();//Globals.CurrentQueryString);	
								return;
							}
								
						}
					}
				}

				if (IsNotSavedRequest(entity) || !HaveRightToViewRequest(entity))
				{
					ActionProcessor.ReplaceLastAction("Request not found");
					Response.Redirect(Requester.IsLogged ? "~/MyRequests.aspx" : "~/Default.aspx");
				}
			}
		}
	}

	private static bool HaveRightToViewRequest(Request request)
	{
		if (request.OwnerID == Requester.LoggedUserID)
		{
			return true;
		}
		if (request.Requesters.Any(x => x.RequesterID == Requester.LoggedUserID))
		{
			return true;
		}
		if (request.IsPrivate == true)
		{
			return false;
		}
		if (Settings.Scope == RequestScope.Private)
		{
			return false;
		}

		return true;
	}

	private static bool IsNotSavedRequest(Request request)
	{
		return request == null || request.ID <= 0 || !request.ID.HasValue;
	}
}
