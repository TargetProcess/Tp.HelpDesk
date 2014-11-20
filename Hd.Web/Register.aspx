<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" Inherits="RegisterPage"
    Title="Register" EnableViewState="false" CodeBehind="Register.aspx.cs" %>
<%@ Register Src="~/Controls/Navbar.ascx" TagPrefix="tp" TagName="Navbar" %>

<%@ Import Namespace="Hd.Portal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="tabs" runat="server">
    <tp:Navbar runat="server" ID="Navbar" />
</asp:Content>
<asp:Content ID="cnt" ContentPlaceHolderID="plcContent" runat="Server">
    <asp:FormView ID="requesterDetails" DataSourceID="requesterSource" runat="server"
        DataKeyNames="UserID" DefaultMode="Edit" OnItemUpdated="OnUpdatedItem">
        <EditItemTemplate>
            <div style="padding: 5px 35px; width: 650px">
                <label>Your First Name</label>
                <br />
                <tp:TpTextBox MaxLength="100" ID="txtFirstName" CssClass="form-control" Width="250"
                    runat="server"
                    Text='<%# Bind("FirstName") %>' ToolTipText="Please provide your first name ">
                </tp:TpTextBox>
                <asp:RequiredFieldValidator ID="vldFirstName" Display="Dynamic" runat="server" ErrorMessage="*"
                    ControlToValidate="txtFirstName"></asp:RequiredFieldValidator>
                <label>Your Last Name</label>
                <br />
                <tp:TpTextBox MaxLength="100" ID="txtLastName" CssClass="form-control" Width="250"
                    runat="server"
                    Text='<%# Bind("LastName") %>' ToolTipText="Please provide your last name ">
                </tp:TpTextBox>
                <asp:RequiredFieldValidator ID="vldLastName" Display="Dynamic" runat="server" ErrorMessage="*"
                    ControlToValidate="txtLastName"></asp:RequiredFieldValidator>
                <label>Your Email</label>
                <br />
                <tp:TpTextBox MaxLength="100" ID="txtRequesterEmail" CssClass="form-control" Width="250"
                    runat="server" Text='<%# Bind("Email") %>' ToolTipText="Please provide your email">
                </tp:TpTextBox>
                <asp:RegularExpressionValidator ID="vldEmail" runat="server" ControlToValidate="txtRequesterEmail"
                    ErrorMessage="*" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="vldEmailRequired" Display="Dynamic" runat="server"
                    ErrorMessage="*" ControlToValidate="txtRequesterEmail" EnableClientScript="true"
                    Visible='<%# !Requester.IsLogged %>'></asp:RequiredFieldValidator>
                <label id="Span1" runat="server">Your Password</label>
                <br />
                <tp:TpTextBox MaxLength="255" ID="txtPassword" TextMode="Password" CssClass="form-control"
                    Width="250" runat="server" Text='<%# Bind("Password") %>' ToolTipText="Please enter the password">
                </tp:TpTextBox>
                <asp:RequiredFieldValidator ID="vldPassword" Display="Dynamic" runat="server" ErrorMessage="*"
                    ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
                <br />
                <asp:CustomValidator ID="vldCustom" runat="server" Text="The requester with such email already exists"
                    OnServerValidate="OnValidateEmail" ControlToValidate='txtRequesterEmail'></asp:CustomValidator>
                <br />
                <div class="repad">
                    <asp:Button ID="UpdateButton" CssClass="btn btn-success btn-sm" runat="server" CausesValidation="True"
                        CommandName="Update" Text="Register Me"></asp:Button>
                </div>
        </EditItemTemplate>
    </asp:FormView>
    <tp:TpObjectDataSource ID="requesterSource" runat="server" DataObjectTypeName="Hd.Portal.Requester"
        SelectMethod="RetrieveOrCreate" TypeName="Hd.Portal.Requester" UpdateMethod="Save"
        OnUpdated="OnUpdated">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="RequesterID" DefaultValue="0" Name="RequesterId"
                Type="Int32" />
        </SelectParameters>
    </tp:TpObjectDataSource>
</asp:Content>
