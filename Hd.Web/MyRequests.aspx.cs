// 
// Copyright (c) 2005-2013 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hd.Portal;
using Hd.Web.Extensions;

public partial class MyRequests : PersisterBasePage
{
	protected void OnDeleteEntity(int? id, EventArgs args)
	{
		Hd.Portal.Request.RemoveRequester(id, Requester.Logged.ID.Value);
		requesterController.RefreshGrid();
		ownerController.RefreshGrid();
	}

	protected string GetPostBackScript(object requestID)
	{
		return ClientScript.GetPostBackEventReference(this, requestID.ToString());
	}

	protected bool IsEditPossible(object obj)
	{
		int? ownerID = (int)DataBinder.Eval(obj,"OwnerID");
		return Requester.Logged.ID == ownerID;
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if(Requester.IsLoggedAsAnonymous)
			Response.Redirect("~/Default.aspx");

		var manager = ScriptManager.GetCurrent(this);
		manager.RegisterAsyncPostBackControl(requesterController);
		manager.RegisterAsyncPostBackControl(ownerController);

		ownerController.Grid.Sorting += OwnerGrid_Sorting;
		requesterController.Grid.Sorting += RequesterGrid_Sorting;
	}

	private void RequesterGrid_Sorting(object sender, GridViewSortEventArgs e)
	{
		ownerController.RefreshGrid();
	}

	private void OwnerGrid_Sorting(object sender, GridViewSortEventArgs e)
	{
		requesterController.RefreshGrid();
	}
}