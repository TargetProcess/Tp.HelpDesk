// 
// Copyright (c) 2005-2013 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using Hd.Portal;
using Hd.Web.Extensions;
using Hd.Web.Extensions.Components;
using Tp.RequestServiceProxy;
using Tp.Web.Extensions.Components;
using StringUtils=Hd.Portal.Components.StringUtils;

public partial class SearchPage : PersisterBasePage, IVoteHolderGridViewContainer
{
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		voteManager.VoteAdded += voteManager_VoteAdded;
		//if (Settings.Scope == RequestScope.Private)
		//    Response.Redirect("~/MyRequests.aspx");
	}

	private void voteManager_VoteAdded(object sender, VoteEventArgs args)
	{
		if( Requester.IsLogged && !Hd.Portal.Request.IsRequesterAttached( args.RequestID.Value, Requester.LoggedUserID.Value ) )
		{
			var dto = new RequestRequesterDTO {RequestID = args.RequestID.Value, RequesterID = Requester.Logged.ID};
			Hd.Portal.Request.AddRequester(args.RequestID.Value, dto);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
        // Redirect to ViewRequest if ID
        String SearchString = Request.QueryString[0];
        if (SearchString == string.Empty)
            return;

        try
        {
            int requestID = int.Parse(SearchString);
            string url = "~/ViewRequest.aspx";
            string param = "RequestID=" + requestID;

            url = Globals.AppendQueryParameters(url, param);
            Response.Redirect(Globals.ResolveClientUrl(url));
        }
        catch (Exception)
        {
            // NOT an integer
        }
		base.OnLoad(e);
		controller.RefreshGrid();
	}

	public object GetHighlightedText(object value)
	{
		if (value == null)
		{
			return null;
		}

		string valueToHighlight = value.ToString();

		string pattern = StringUtils.TrimToNull(Request.QueryString["SearchString"]);

		return WordsHighlighter.Highlight(valueToHighlight, pattern);
	}

	public VoteHolderGridView VoteHolderGridView {
		get { return allRequestListing; }
	}
}
