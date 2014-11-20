<%@ Import Namespace="Hd.Portal" %>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_AttachmentComment"
    CodeBehind="AttachmentComment.ascx.cs" %>
<%@ Register TagPrefix="CKEditor" Namespace="CKEditor.NET" Assembly="CKEditor.NET, Version=3.6.6.2, Culture=neutral, PublicKeyToken=e379cdf2f8354999" %>
<label>Attachments</label>
<asp:DataGrid ID="lstAttachment" runat="server" CssClass="generalTable infoTable"
    AutoGenerateColumns="False" ShowHeader="false" GridLines="None" CellPadding="3"
    AllowSorting="False" AllowPaging="false">
    <Columns>
        <asp:TemplateColumn>
            <ItemTemplate>

                <strong>
                    <i class="iconHolder iconAttachment"></i>
                    <asp:HyperLink Target="_blank" ID="lnkAttach" NavigateUrl='<%# Eval("AttachmentID", "~/Attachment.ashx?AttachmentID={0}")%>'
                        runat="server" Text='<%# Eval("OriginalFileName") %>' />
                </strong>
                <br />
                <%# Eval("Description") %>
                <br />
                <%# Eval("CreateDate", "{0:dd-MMM-yyyy}") %> by <%# Eval("Owner.FullName") %>
            </ItemTemplate>
            <ItemStyle CssClass="attachment"></ItemStyle>
        </asp:TemplateColumn>
    </Columns>
</asp:DataGrid>
<div>
    <tp:MultiAttachment ID="attachments" runat="server" ShowSubmitButton="true" OnAttachmentsUploaded="OnAddAttachments" />
</div>
<br />
<div class="commentPanel">
    <label>Comments</label>
    <br />
    <span id="spanAddComment" runat="server"><a id="lnkNewComment" class="btn btn-sm"
        href="javascript:PostNewComment();"
        runat="server">Add</a></span>
    <br />
    <br />
</div>
<asp:UpdatePanel ID="updPanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:DataGrid ID="lstComments" runat="server" AllowPaging="false" ShowHeader="false"
            AutoGenerateColumns="false" GridLines="None">
            <Columns>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <%-- Get Owner regardless of logged in user --%>
                        <div class="<%# (Eval("Owner.UserID").ToString() == Requester.LoggedUserID.ToString())%><%#(Eval("Owner.IsAdministrator").ToString()) %>comment">
                            <asp:Label ID="lblOwner" CssClass="commentOwner" runat="server" Text='<%# Eval("Owner.FullName") %>'></asp:Label>
                            <span><a name='<%# Eval("CommentID") %>'></a>
                            </span>
                            <asp:Label ID="lblDate" runat="server" CssClass="commentDetails" Text='<%# Eval("CreateDate", "{0:dd-MMM-yyyy HH:mm}") %>'></asp:Label>
                            <div id="comment_<%# Eval("CommentID") %>" class="commentText">
                                <%# Eval("Description") %>
                            </div>
                            <asp:PlaceHolder ID="pnlDeleteEdit" runat="server" Visible='<%# (Eval("Owner.UserID").ToString() == Requester.LoggedUserID.ToString() )%>'>
                                <div class="commentTools">
                                    <a href="javascript:EditComment(<%# Eval("CommentID") %>)" class="editLink btn btn-sm">
                                        Edit</a>
                                    <a href="javascript:DeleteComment(<%# Eval("CommentID") %>)" class="editLink btn btn-sm btn-warning">
                                        Delete</a>
                                </div>
                            </asp:PlaceHolder>
                        </div>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:Panel ID="pnlComment" runat="server" Style="width: 600px; display: none;">
    <div class="repad">
        <asp:HiddenField ID="hdnParentID" runat="server" />
        <asp:HiddenField ID="hdnCommentID" runat="server" />

        <CKEditor:CKEditorControl ID="txtComment" BasePath="~/ckeditor/" Text='<%# Bind("Description") %>'
            runat="server"
            Skin="moono">
        </CKEditor:CKEditorControl>


        <br />
        <asp:Button OnClientClick="DisableButtonOnSubmit(this, 'Comment');" UseSubmitBehavior="false"
            ID="btnPostComment" CssClass="editLink btn btn-sm btn-success" runat="server"
            CausesValidation="True"
            Text="Save" OnClick="SaveReply" ValidationGroup="Comment"></asp:Button>&nbsp;
        <a id="Ax1" href="javascript:HideComments()" class='editLink btn btn-sm btn-warning'>
            Cancel</a>

    </div>

</asp:Panel>
<tp:AsyncPostBackHandler ID="handler" runat="server" OnPostBack="DeleteComment">
</tp:AsyncPostBackHandler>
<%--<script type="text/javascript">

        $("#Ax1").on("click", function (e) {
            e.preventDefault();
            alert("FOO");
            $("#ctl00_plcContent_tabControl_ac_pnlComment").css({ display: "none" });
        });

</script>--%>

<script type="text/javascript">
    var FCK;
    function DeleteComment(commentID) {
        <%= Page.ClientScript.GetPostBackEventReference(handler, "id").Replace("'id'", "commentID") %>;
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
    //function FCKEditor_OnInitialized(editor) {
    //    FCK = editor;
    //    editor.Focus();
    //}
    function PostNewComment() {
        ShowPopup();
        ClearComments();
        SetContent('');
        document.getElementById("<%=pnlComment.ClientID%>").scrollIntoView();
        setTimeout(SetFocus, 100);
    }
    function SetFocus() {
        <%--var editor = document.getElementById('<%= txtComment.ClientID %>');
        if (editor.FocusDocument)
            editor.FocusDocument();--%>
        //if (FCK.Focus)
        //    FCK.Focus();
    }
    function ShowPopup() {
        $('#<%= pnlComment.ClientID %>').css({ display: '' });
        $('<%= pnlComment.ClientID %>').center();
    }
    function HideComments() {
        ClearComments();
        $('#<%= pnlComment.ClientID %>').css({ display: 'none' });
    }
    function ClearComments() {
        $get('<%= hdnCommentID.ClientID %>').value = '';
        SetContent('');
    }
    function PostReply(parentId) {
        $get('<%= hdnParentID.ClientID %>').value = parentId;
        ShowPopup();
        ClearComments();
        setTimeout(SetFocus, 100);
    }
    function SetContent(value) {
        //var editor = $('#cke_ctl00_plcContent_tabControl_ac_txtComment').ckeditor().editor;
        // var editor = document.getElementById('<%= txtComment.ClientID %>');
        //CKEDITOR.instances.editor1.setData(value);
    }
    function EditComment(commentID) {
        var div = $get("comment_" + commentID);
        // alert("#comment_" + commentID);
        ShowPopup();
        SetContent(div.innerHTML);
        // alert(div.innerHTML);
        $get('<%= hdnCommentID.ClientID %>').value = commentID;
        $get('<%= hdnParentID.ClientID %>').value = '';
        setTimeout(SetFocus, 100);
    }
</script>
