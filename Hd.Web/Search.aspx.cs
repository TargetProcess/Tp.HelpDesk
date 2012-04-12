using System;
using System.Web.UI;
using Hd.Portal;
using Hd.Portal.Components;
using Hd.Web.Extensions;
using Tp.RequestServiceProxy;
using Tp.Web.Extensions.Components;
using StringUtils=Hd.Portal.Components.StringUtils;


public partial class SearchPage : PersisterBasePage
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
			RequestRequesterDTO dto = new RequestRequesterDTO();
			dto.RequestID = args.RequestID.Value;
			dto.RequesterID = Requester.Logged.ID;
			Hd.Portal.Request.AddRequester(args.RequestID.Value, dto);
		}

	}


	protected override void OnLoad(EventArgs e)
	{
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
}