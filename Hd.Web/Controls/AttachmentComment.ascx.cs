// 
// Copyright (c) 2005-2013 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using System.Collections.Generic;
using System.Web.UI;
using Hd.Portal;
using Hd.Portal.Components.LastActionProcessor;
using Hd.Web.Extensions;

public partial class Controls_AttachmentComment : UserControl, ITabControl
{
	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);

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
		const string hql = "from Comment as c where c.General.GeneralID = ? and Description <> 'DELETED'";
		List<Comment> list = DataPortal.Instance.Retrieve<Comment>(hql, argument);
		lstComments.DataSource = list;
		lstComments.DataBind();
	}

	private void LoadAttachments(object argument)
	{
		const string hql = "from Attachment as a where a.General.GeneralID = ?";
		List<Attachment> list = DataPortal.Instance.Retrieve<Attachment>(hql, argument);
		lstAttachment.DataSource = list;
		lstAttachment.DataBind();
	}

	protected void SaveReply(object sender, EventArgs eventArgs)
	{
		int? commentId = hdnCommentID.Value == string.Empty ? null : (int?)Int32.Parse(hdnCommentID.Value);
		int? parentId = hdnParentID.Value == string.Empty ? null : (int?)Int32.Parse(hdnParentID.Value);

		Comment comment = Comment.RetrieveOrCreate(commentId);
		comment.GeneralID = GeneralID;

		if (parentId.HasValue)
			comment.ParentID = parentId;

		comment.Description = txtComment.Text;

		if (CanEditComment(comment))
		{
			comment.Save();
		}

		LoadComments(GeneralID);
		LoadAttachments(GeneralID);

		updPanel.Update();
	}

	protected void DeleteComment(string argument)
	{
		Comment comment;
		var exists = Comment.TryRetrieve(Int32.Parse(argument), out comment);

		if (exists && CanEditComment(comment))
		{
			ActionProcessor.LastAction = "The comment was deleted";
			Comment.Delete(Int32.Parse(argument));
			LoadContent(GeneralID);
			updPanel.Update();
		}
	}

	private bool CanEditComment(Comment comment)
	{
		return Requester.LoggedUserID == comment.OwnerID;
	}
}