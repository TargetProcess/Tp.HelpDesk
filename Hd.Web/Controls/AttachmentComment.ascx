<%@ Import Namespace="Hd.Portal" %>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_AttachmentComment"
	CodeBehind="AttachmentComment.ascx.cs" %>
<div class="grayTitle" style="width: 300px">
	<div class="blueLeftSep">
		Attachments
	</div>
</div>
<asp:DataGrid ID="lstAttachment" runat="server" CssClass="generalTable infoTable"
	AutoGenerateColumns="False" ShowHeader="false" GridLines="None" CellPadding="3"
	AllowSorting="False" AllowPaging="false">
	<Columns>
		<asp:TemplateColumn>
			<ItemTemplate>
				<div style="white-space: nowrap">
					<asp:HyperLink Target="_blank" ID="lnkAttach" NavigateUrl='<%# Eval("AttachmentID", "~/Attachment.ashx?AttachmentID={0}")%>'
						runat="server" Text='<%# Eval("OriginalFileName") %>' />
					&nbsp;&nbsp;
				</div>
				<%# Eval("Description") %>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
			<ItemTemplate>
				<div style="font-size: 10px; white-space: nowrap">
					posted
					<%# Eval("CreateDate", "{0:dd-MMM-yyyy HH:mm}") %>
				</div>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
			<ItemTemplate>
				by
				<%# Eval("Owner.FullName") %>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:DataGrid>
<div style="padding-left: 20px">
	<tp:MultiAttachment ID="attachments" runat="server" ShowSubmitButton="true" OnAttachmentsUploaded="OnAddAttachments" />
</div>
<br />
<div class="grayTitle" style="width: 300px">
	<div class="blueLeftSep">
		Comments <span id="spanAddComment" runat="server">[&nbsp;<a id="lnkNewComment" href="javascript:PostNewComment();"
			runat="server">add</a>&nbsp;]</span>
	</div>
</div>
<asp:UpdatePanel ID="updPanel" runat="server" UpdateMode="Conditional">
	<ContentTemplate>
		<div style="padding-left: 20px; margin-top: 10px">
			<asp:DataGrid ID="lstComments" runat="server" AllowPaging="false" ShowHeader="false"
				AutoGenerateColumns="false" GridLines="None">
				<Columns>
					<asp:TemplateColumn>
						<ItemTemplate>
							<img id="i" runat="server" src="~/img/comment.gif" alt="comment" style="float: left;
								margin-right: 10px" />
							<div id="comment_<%# Eval("CommentID") %>">
								<%# Eval("Description") %>
							</div>
							<div style="font-size: 10px; white-space: nowrap; margin-bottom: 4px; color: #666">
								Comment #<asp:Label ID="lblCommentID" runat="server" Text='<%# Eval("CommentID") %>'></asp:Label>
								<span style="color: #333; font-weight: bold"><a name='<%# Eval("CommentID") %>'></a>
								</span>posted by
								<asp:Label ID="lblOwner" runat="server" Text='<%# Eval("Owner.FullName") %>'></asp:Label>
								at
								<asp:Label ID="lblDate" runat="server" Text='<%# Eval("CreateDate", "{0:dd-MMM-yyyy HH:mm}") %>'></asp:Label>
								<asp:PlaceHolder ID="pnlDeleteEdit" runat="server" Visible='<%# (Eval("Owner.UserID").ToString() == Requester.LoggedUserID.ToString() )%>'>
									| <a href="javascript:EditComment(<%# Eval("CommentID") %>)">Edit</a> | <a href="javascript:DeleteComment(<%# Eval("CommentID") %>)">
										Delete</a> </asp:PlaceHolder>
							</div>
							<br />
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
			</asp:DataGrid>
		</div>
	</ContentTemplate>
</asp:UpdatePanel>
<asp:Panel ID="pnlComment" runat="server" Style="width: 600px; display: none;">
	<table cellpadding="5">
		<tr>
			<td style="text-align: center">
				<asp:HiddenField ID="hdnParentID" runat="server" />
				<asp:HiddenField ID="hdnCommentID" runat="server" />
				<tp:RichEditTextBox ID="txtComment" Mode="Simple" Width="600" Height="250" runat="server"
					Text=""></tp:RichEditTextBox>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Button OnClientClick="DisableButtonOnSubmit(this, 'Comment');" UseSubmitBehavior="false"
					ID="btnPostComment" CssClass="button" runat="server" CausesValidation="True"
					Text="Save" OnClick="SaveReply" ValidationGroup="Comment"></asp:Button>&nbsp;
				<a id="A1" href="javascript:HideComments();" runat="server" class='buttonSecondary'>
					Cancel</a>
			</td>
		</tr>
	</table>
</asp:Panel>
<tp:AsyncPostBackHandler ID="handler" runat="server" OnPostBack="DeleteComment">
</tp:AsyncPostBackHandler>
<script language="javascript" type="text/javascript">
	var FCK;

	function DeleteComment(commentID)
    {
        <%= Page.ClientScript.GetPostBackEventReference(this.handler, "id").Replace("'id'", "commentID") %>;
    }

	function DisableButtonOnSubmit(button, validationGroup) {
		
		if (typeof (Page_ClientValidate) == 'function') {
			if (Page_ClientValidate(validationGroup) == false) {
				return false;
			}
		}
		button.disabled = true;
		button.value = 'Submitting...';
	}

	function FCKEditor_OnInitialized(editor) {
		FCK = editor;
		editor.Focus();
	}

	function PostNewComment() {
		ShowPopup();
		ClearComments();
		document.getElementById("<%=this.pnlComment.ClientID%>").scrollIntoView();
		setTimeout(SetFocus, 100);
	}

	function SetFocus() {
		var editor = document.getElementById('<%= txtComment.EditorClientID %>');

		if (editor.FocusDocument)
			editor.FocusDocument();

		if (FCK.Focus)
			FCK.Focus();
	}

	function ShowPopup() {
		$get('<%= this.pnlComment.ClientID %>').style.display = '';
	}

	function HideComments() {
		ClearComments();
		$get('<%= this.pnlComment.ClientID %>').style.display = 'none';
	}

	function ClearComments() {
		$get('<%= this.hdnCommentID.ClientID %>').value = '';
		SetContent('');
	}

	function PostReply(parentId) {
		$get('<%= this.hdnParentID.ClientID %>').value = parentId;
		ShowPopup();
		ClearComments();
		setTimeout(SetFocus, 100);
	}

	function SetContent(value) {
		var editor = document.getElementById('<%= txtComment.EditorClientID %>');

		if (FCK.SetHTML)
			FCK.SetHTML(value);
		else
			editor.valueToSet = value;
	}

	function EditComment(commentID) {
		var div = $get("comment_" + commentID);
		ShowPopup();
		SetContent(div.innerHTML);
		$get('<%= this.hdnCommentID.ClientID %>').value = commentID;
		$get('<%= this.hdnParentID.ClientID %>').value = '';
		setTimeout(SetFocus, 100);
	}
</script>
