<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AttachmentControl.ascx.cs"
    Inherits="Hd.Web.Controls.AttachmentControl" %>
<br />
<br />
<div class="contentbg repad">
    <label>
        Attachments
    </label>
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
                    <div>
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
    <div>
        <tp:MultiAttachment ID="attachments" runat="server" ShowSubmitButton="false" OnAttachmentsUploaded="OnAddAttachments" />
    </div>
</div>
<br />
<br />
