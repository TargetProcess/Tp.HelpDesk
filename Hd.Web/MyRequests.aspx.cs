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
        //Hd.Portal.Request.Retrieve(43).
		Hd.Portal.Request.RemoveRequester(id, Requester.Logged.ID.Value);
		requesterController.RefreshGrid();
		ownerController.RefreshGrid();
	}

	protected String GetPostBackScript(Object requestID)
	{
		return ClientScript.GetPostBackEventReference(this, requestID.ToString());
	}

	protected Boolean IsEditPossible(Object obj)
	{
		Int32? ownerID = (Int32)DataBinder.Eval(obj,"OwnerID");
		return Requester.Logged.ID == ownerID;
	}

	protected void Page_Load(Object sender, EventArgs e)
	{
		if(Requester.IsLoggedAsAnonymous)
			Response.Redirect("~/Default.aspx");

		var manager = ScriptManager.GetCurrent(this);
		manager.RegisterAsyncPostBackControl(requesterController);
		manager.RegisterAsyncPostBackControl(ownerController);

		var currentProject = Session["currentproject"] ?? String.Empty;
		requesterController.FilterProject = currentProject.ToString();
		ownerController.FilterProject = currentProject.ToString();
        ownerController.InitializeFilter();
		ownerController.Grid.Sorting += OwnerGrid_Sorting;
		requesterController.Grid.Sorting += RequesterGrid_Sorting;
	}

	private void RequesterGrid_Sorting(Object sender, GridViewSortEventArgs e)
	{
		ownerController.RefreshGrid();
	}

	private void OwnerGrid_Sorting(Object sender, GridViewSortEventArgs e)
	{
		requesterController.RefreshGrid();
	}
}