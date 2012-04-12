<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" Inherits="RegisterPage" Title="Register" EnableViewState="false" Codebehind="Register.aspx.cs" %>
<%@ Import Namespace="Hd.Portal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="tabs" runat="server">
	<div id="tabs" style="clear: both">
		<div class="selectedTab" style="float: left" runat="server" id="div2">
			<a  href="Default.aspx" >Home</a></div>
		<tp:RequestScopeTab ID="RequestScopeTab1" CssClass="tab"  Scope="Private"  runat="server" Text="My Requests" Url="MyRequests.aspx"  />
		<tp:RequestScopeTab ID="RequestScopeTab2" CssClass="tab"  Scope="Global" runat="server" Url="Issues.aspx" Text="Issues" />
		<tp:RequestScopeTab ID="RequestScopeTab4" CssClass="tab"  Scope="Global" runat="server" Url="Ideas.aspx" Text="Ideas" />
		<tp:RequestScopeTab ID="RequestScopeTab3" CssClass="clearTab" Scope="Private" runat="server" Url="Request.aspx" Text="Add Request"  />
	</div>
</asp:Content>
<asp:Content ID="cnt" ContentPlaceHolderID="plcContent" runat="Server">
	<asp:FormView ID="requesterDetails" DataSourceID="requesterSource" runat="server"
		DataKeyNames="UserID" DefaultMode="Edit" OnItemUpdated="OnUpdatedItem">
		<EditItemTemplate>
			<div style="padding: 5px 35px; width: 650px">
				<span class="fieldName">Your First Name</span>
				<br />
				<tp:TpTextBox MaxLength="100" ID="txtFirstName" CssClass="input" Width="250" runat="server"
					Text='<%# Bind("FirstName") %>' ToolTipText="Please provide your first name ">
				</tp:TpTextBox>
				<asp:RequiredFieldValidator ID="vldFirstName" Display="Dynamic" runat="server" ErrorMessage="*"
					ControlToValidate="txtFirstName"></asp:RequiredFieldValidator>
				<br />
				<br />
				<span class="fieldName">Your Last Name</span>
				<br />
				<tp:TpTextBox MaxLength="100" ID="txtLastName" CssClass="input" Width="250" runat="server"
					Text='<%# Bind("LastName") %>' ToolTipText="Please provide your last name ">
				</tp:TpTextBox>
				<asp:RequiredFieldValidator ID="vldLastName" Display="Dynamic" runat="server" ErrorMessage="*"
					ControlToValidate="txtLastName"></asp:RequiredFieldValidator>
				<br />
				<br />
				<span class="fieldName">Your Email</span>
				<br />
				<tp:TpTextBox MaxLength="100" ID="txtRequesterEmail" CssClass="input" Width="250"
					runat="server" Text='<%# Bind("Email") %>' ToolTipText="Please provide your email">
				</tp:TpTextBox>
				<asp:RegularExpressionValidator ID="vldEmail" runat="server" ControlToValidate="txtRequesterEmail"
					ErrorMessage="*" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
				<asp:RequiredFieldValidator ID="vldEmailRequired" Display="Dynamic" runat="server"
					ErrorMessage="*" ControlToValidate="txtRequesterEmail" EnableClientScript="true"
					Visible='<%# !Requester.IsLogged %>'></asp:RequiredFieldValidator>
                <asp:CustomValidator ID="vldCustom" runat="server" Text="The requester with such email already exists" OnServerValidate="OnValidateEmail" ControlToValidate='txtRequesterEmail'></asp:CustomValidator>					
				<br />
				<br />
				<span id="Span1" class="fieldName" runat="server">Your Password</span>
				<br />
				<tp:TpTextBox MaxLength="255" ID="txtPassword" TextMode="Password" CssClass="input"
					Width="150" runat="server" Text='<%# Bind("Password") %>' ToolTipText="Please enter the password">
				</tp:TpTextBox>
				<asp:RequiredFieldValidator ID="vldPassword" Display="Dynamic" runat="server" ErrorMessage="*"
					ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
				<br />
				<br />
				<div style="padding: 10px 10px; background: #E5E5E5; border-top: 1px solid #CCC">
						<asp:Button ID="UpdateButton" CssClass="largeButton" runat="server" CausesValidation="True"
							CommandName="Update" Text="Register Me"></asp:Button>
					</div>
		</EditItemTemplate>
	</asp:FormView>
	<tp:TpObjectDataSource ID="requesterSource" runat="server" DataObjectTypeName="Hd.Portal.Requester"
		SelectMethod="RetrieveOrCreate" TypeName="Hd.Portal.Requester" UpdateMethod="Save" OnUpdated="OnUpdated">
		<SelectParameters>
			<asp:QueryStringParameter QueryStringField="RequesterID" DefaultValue="0" Name="RequesterId"
				Type="Int32" />
		</SelectParameters>
	</tp:TpObjectDataSource>
</asp:Content>
