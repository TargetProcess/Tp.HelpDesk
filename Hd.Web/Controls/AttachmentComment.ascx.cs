using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI;
using Hd.Portal;
using Hd.Portal.Components.LastActionProcessor;
using Hd.Web.Extensions;

public partial class Controls_AttachmentComment : UserControl, ITabControl
{

	protected override void OnLoad( EventArgs e )
	{
		base.OnLoad( e );

		attachments.Visible = Requester.IsLogged;
		spanAddComment.Visible = Requester.IsLogged;
		//lnkNewComment.Visible = Requester.IsLogged;
	}

    protected void OnAddAttachments(object sender)
    {
        Request request = Hd.Portal.Request.Retrieve(GeneralID);
        request.AddAttachments(attachments.Attachments);
		ActionProcessor.LastAction = "The attachment(s) were added";
		Response.Redirect(Request.Url.AbsoluteUri);
    }

    private int GeneralID
    {
        get { return Int32.Parse(Request.QueryString["RequestID"]); }
    }

    public void LoadContent(object argument)
    {
        LoadAttachments(argument);
        LoadComments(argument);
    }

    private void LoadComments(object argument)
    {
        string hql = "from Comment as c where c.General.GeneralID = ? and Description <> 'DELETED'";
        List<Comment> list = DataPortal.Instance.Retrieve<Comment>(hql, argument);
        lstComments.DataSource = list;
        lstComments.DataBind();
    }

    private void LoadAttachments(object argument)
    {
        string hql = "from Attachment as a where a.General.GeneralID = ?";
        List<Attachment> list = DataPortal.Instance.Retrieve<Attachment>(hql, argument);
        lstAttachment.DataSource = list;
        lstAttachment.DataBind();
    }

    protected void SaveReply(object sender, EventArgs eventArgs)
    {
        int? commentID = hdnCommentID.Value == string.Empty ? null : (int?)Int32.Parse(hdnCommentID.Value);
        int? parentID = hdnParentID.Value == string.Empty ? null : (int?)Int32.Parse(hdnParentID.Value);

        Comment comment = Comment.RetrieveOrCreate(commentID);
        comment.GeneralID = GeneralID;

        if (parentID.HasValue)
            comment.ParentID = parentID;

        comment.Description = txtComment.Text;
        comment.Save();

        LoadComments(GeneralID);
		LoadAttachments(GeneralID);

        updPanel.Update();
    }


    protected void DeleteComment(string argument)
    {
        ActionProcessor.LastAction = "The comment was deleted";
        Comment.Delete(Int32.Parse(argument));
        LoadContent(GeneralID);
        updPanel.Update();
    }
}

