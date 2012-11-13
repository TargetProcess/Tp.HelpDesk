<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" Inherits="RequestPage"
	CodeBehind="Request.aspx.cs" %>

<%@ Register Src="~/Controls/AttachmentControl.ascx" TagName="Attachment" TagPrefix="tp" %>
<%@ Register Src="~/Controls/ProductDropDown.ascx" TagName="ProductDropDown" TagPrefix="tp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="tabs" runat="server">
	<div id="tabs">
		<div class="selectedTab" style="float: left" runat="server" id="div2">
			<a href="Default.aspx">Home</a></div>
		<tp:RequestScopeTab ID="RequestScopeTab1" Scope="Private" CssClass="tab" runat="server"
			Text="My Requests" Url="MyRequests.aspx" />
		<tp:RequestScopeTab ID="RequestScopeTab2" Scope="Global" CssClass="tab" runat="server"
			Url="Issues.aspx" Text="Issues" />
		<tp:RequestScopeTab ID="RequestScopeTab4" CssClass="tab" Scope="Global" runat="server"
			Url="Ideas.aspx" Text="Ideas" />
		<tp:RequestScopeTab ID="RequestScopeTab3" Scope="Private" CssClass="clearTab" runat="server"
			Url="Request.aspx" Text="Add Request" />
	</div>
</asp:Content>
<asp:Content ID="cnt" ContentPlaceHolderID="plcContent" runat="Server">
	<asp:FormView ID="requestDetails" runat="server" DataSourceID="requestSource" DataKeyNames="RequestID"
		DefaultMode="Edit">
		<EditItemTemplate>
			<div style="padding: 5px 35px; width: 650px">
				<span style="font: bold 16px Arial">
					<asp:PlaceHolder ID="plcNew" runat="server" Visible='<%# Eval("IsNew") %>'>Add</asp:PlaceHolder>
					<asp:PlaceHolder ID="plcEdit" runat="server" Visible='<%# !Boolean.Parse(Eval("IsNew").ToString()) %>'>
						Edit</asp:PlaceHolder>
					Request</span>
				<br />
				<br />
				<span class="fieldName">Title (required)</span><br />
				<tp:TpTextBox MaxLength="255" ID="txtTitle" CssClass="input" Width="550" runat="server"
					Text='<%# Bind("Name") %>' ToolTipText="Please provide the short name of the request">
				</tp:TpTextBox>
				<asp:RequiredFieldValidator ID="vldTitle" Display="Dynamic" runat="server" ErrorMessage="*"
					ControlToValidate="txtTitle"></asp:RequiredFieldValidator>
				<br />
				<br />
				<span class="fieldName">Description</span>
				<br />
				<tp:RichEditTextBox ID="txtDescription" Width="700" Height="300" runat="server" Text='<%# Bind("Description") %>'
					Mode="Simple"></tp:RichEditTextBox>
				<script language="javascript" type="text/javascript">
					Sys.Application.add_init(
						function () {
							Ext.QuickTips.init(); // enable tooltips
						}
					);
				</script>
				<br />
				<br />
				<asp:PlaceHolder Visible='<%# Eval("IsNew") %>' runat="server">
					<asp:CheckBox runat="server" TextAlign="Right" ID="chkUrgent" Text="Is Urgent?" Checked='<%# Bind("IsUrgent") %>' /><br />
				</asp:PlaceHolder>
				<asp:PlaceHolder Visible='<%# !(bool)Eval("IsNew") %>' runat="server">
					<span class="fieldName">Business Value</span><br />
					<tp:TpDropDownList ID="lstPriority" runat="server" DataSourceID="prioritySource" DataTextField="Name"
						DataValueField="PriorityID" SelectedValue='<%#Bind("PriorityID") %>'>
					</tp:TpDropDownList>
				</asp:PlaceHolder>
				<asp:Panel ID="pnlPrivate" runat="server" Visible='<%# Bind("IsNew") %>'>
					<br />
					<br />
					<asp:CheckBox ID="private" TextAlign="Right" runat="server" Text="Is Private?" Checked='<%# Bind("IsPrivate") %>' />
				</asp:Panel>
				<br />
				<br />
				<span class="fieldName">Request Type</span><br />
				<tp:TpDropDownList ID="lstRequestType" runat="server" DataSourceID="requestTypeSource"
					DataTextField="Name" DataValueField="ID" SelectedValue='<%#Bind("RequestTypeID") %>'>
				</tp:TpDropDownList>
				<tp:ProductDropDown ID="tpProductDropDown" runat="server" ProjectId='<%# Bind("ProjectID") %>' />
				<br />
				<br />
				<tp:Attachment ID="uxAttachment" runat="server" />
				<br />
				<br />
				<div style="padding: 10px 10px; background: #E5E5E5; border-top: 1px solid #CCC">
					<asp:Button ID="UpdateButton" CssClass="largeButton" runat="server" CausesValidation="True"
						CommandName="Update" Text="Submit Request"></asp:Button>
					<a class="buttonSecondary" href="javascript:history.back(-1)">Cancel</a>
				</div>
			</div>
		</EditItemTemplate>
	</asp:FormView>
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
