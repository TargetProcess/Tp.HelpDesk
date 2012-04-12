//  
// Copyright (c) 2005-2009 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using Hd.Portal;
using Hd.Web.Controls;
using Hd.Web.Extensions;

public partial class RequestPage : PersisterBasePage
{
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		requestSource.Updated += requestSource_Updated;
	}

	private void requestSource_Updated(object sender, TpObjectDataSourceEventArgs e)
	{
		var request = e.BusinessObject as Request;
		var uxAttachment = requestDetails.FindControl("uxAttachment") as AttachmentControl;
		request.AddAttachments(uxAttachment.Attachments);
		Response.Redirect("~/MyRequests.aspx");
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (Requester.IsLoggedAsAnonymous)
			Response.Redirect("~/Default.aspx");
	}
}