<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" Inherits="RequestPage"
    CodeBehind="Request.aspx.cs" %>


<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>


<%@ Register Src="~/Controls/AttachmentControl.ascx" TagName="Attachment" TagPrefix="tp" %>
<%@ Register Src="~/Controls/ProductDropDown.ascx" TagName="ProductDropDown" TagPrefix="tp" %>
<%@ Register Src="~/Controls/Navbar.ascx" TagPrefix="tp" TagName="Navbar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="tabs" runat="server">
    <tp:Navbar runat="server" ID="Navbar" />
</asp:Content>
<asp:Content ID="cnt" ContentPlaceHolderID="plcContent" runat="Server">
    <div class="row">
        <div class="col-lg-4">
            <asp:FormView ID="requestDetails" runat="server" DataSourceID="requestSource" DataKeyNames="RequestID"
                DefaultMode="Edit">
                <EditItemTemplate>
                    <div class="repad">
                        <h2>
                        <asp:PlaceHolder ID="plcNew" runat="server" Visible='<%# Eval("IsNew") %>'>Add</asp:PlaceHolder>
                        <asp:PlaceHolder ID="plcEdit" runat="server" Visible='<%# !Boolean.Parse(Eval("IsNew").ToString()) %>'>
                            Edit</asp:PlaceHolder>
                        Request</h2>
                        <label>Title (required)</label>
                        <tp:TpTextBox MaxLength="255" ID="txtTitle" CssClass="form-control" runat="server"
                            Text='<%# Bind("Name") %>' ToolTipText="Please provide the short name of the request">
                        </tp:TpTextBox>
                        <asp:RequiredFieldValidator ID="vldTitle" Display="Dynamic" runat="server" ErrorMessage="*"
                            ControlToValidate="txtTitle"></asp:RequiredFieldValidator>
                        <label>Description</label>
                        <%--<asp:TextBox runat="server" CssClass="form-control" ID="txtDescription" Text='<%# Bind("Description") %>'
                    TextMode="MultiLine" Rows="6"></asp:TextBox>--%>


                        <CKEditor:CKEditorControl ID="CKEditor1" BasePath="~/ckeditor/" Text='<%# Bind("Description") %>'
                            runat="server"
                            Skin="moono" Toolbar="Basic">
                        </CKEditor:CKEditorControl>


                        <%--<tp:RichEditTextBox ID="txtDescription" Width="700" Height="300" runat="server" Text='<%# Bind("Description") %>'
					Mode="Simple"></tp:RichEditTextBox>
                <script type="text/javascript">
                    Sys.Application.add_init(function () { Ext.QuickTips.init(); });
                </script>--%>
                        <asp:PlaceHolder Visible='<%# Eval("IsNew") %>' runat="server">
                            <asp:CheckBox runat="server" Text="Is Urgent?" TextAlign="Right"
                                ID="chkUrgent" Checked='<%# Bind("IsUrgent") %>' />
                        </asp:PlaceHolder>
                        <asp:PlaceHolder Visible='<%# !(bool)Eval("IsNew") %>' runat="server">
                            <label>Business Value</label>
                            <tp:TpDropDownList ID="lstPriority" CssClass="form-control" runat="server" DataSourceID="prioritySource"
                                DataTextField="Name"
                                DataValueField="PriorityID" SelectedValue='<%#Bind("PriorityID") %>'>
                            </tp:TpDropDownList>
                        </asp:PlaceHolder>
                        <asp:Panel ID="pnlPrivate" runat="server" Visible='<%# Bind("IsNew") %>'>
                            <asp:CheckBox ID="private" TextAlign="Right" runat="server" Text="Is Private?" Checked='<%# Bind("IsPrivate") %>' />
                        </asp:Panel>
                        <label>Request Type</label>
                        <tp:TpDropDownList ID="lstRequestType" CssClass="form-control" runat="server" DataSourceID="requestTypeSource"
                            DataTextField="Name" DataValueField="ID" SelectedValue='<%#Bind("RequestTypeID") %>'>
                        </tp:TpDropDownList>
                        <tp:ProductDropDown ID="tpProductDropDown" CssClass="form-control" runat="server"
                            ProjectId='<%# Bind("ProjectID") %>' />
                        <tp:Attachment ID="uxAttachment" runat="server" />
                        <asp:Button ID="UpdateButton" CssClass="btn btn-success" runat="server" CausesValidation="True"
                            CommandName="Update" Text="Submit Request"></asp:Button>
                        <a class="btn" href="javascript:history.back(-1)">Cancel</a>
                    </div>
                </EditItemTemplate>
            </asp:FormView>

        </div>
    </div>
    <tp:TpObjectDataSource ID="requestSource" runat="server" DataObjectTypeName="Hd.Portal.Request"
        SelectMethod="RetrieveToEditOrCreate" TypeName="Hd.Portal.Request" UpdateMethod="Save">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="RequestId" DefaultValue="0" Name="RequestId"
                Type="Int32" />
        </SelectParameters>
    </tp:TpObjectDataSource>
    <tp:TpObjectDataSource ID="requestTypeSource" runat="server" DataObjectTypeName="Hd.Portal.RequestType"
        SelectMethod="RetrieveAll" TypeName="Hd.Portal.RequestType">
    </tp:TpObjectDataSource>
    <tp:TpObjectDataSource ID="prioritySource" runat="server" DataObjectTypeName="Hd.Portal.Priority"
        SelectMethod="RetrieveAllForRequest" TypeName="Hd.Portal.Priority">
    </tp:TpObjectDataSource>
</asp:Content>
