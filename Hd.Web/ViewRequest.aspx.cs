// 
// Copyright (c) 2005-2013 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System.Collections;
using System.Web.Security;
using Hd.Portal;
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

				if (IsNotSavedRequest(entity) || !PermissionManager.HaveRightToViewRequest(entity))
				{
					ActionProcessor.ReplaceLastAction("Request not found");
					Response.Redirect(Requester.IsLogged ? "~/MyRequests.aspx" : "~/Default.aspx");
				}
			}
		}
	}

	private static bool IsNotSavedRequest(Request request)
	{
		return request == null || request.ID <= 0 || !request.ID.HasValue;
	}
}
