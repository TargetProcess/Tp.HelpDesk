<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    Inherits="Default" Title="Untitled Page" EnableViewState="false" CodeBehind="Default.aspx.cs" %>
<%@ Register Src="~/Controls/Navbar.ascx" TagPrefix="tp" TagName="Navbar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="tabs" runat="server">
    <tp:Navbar runat="server" ID="Navbar" />
</asp:Content>
<asp:Content ID="cnt" ContentPlaceHolderID="plcContent" runat="Server">
    <div class="repad">
        <asp:PlaceHolder runat="server" ID="phMessage">
            <p>
                Please <tp:TpLoginStatus ID="ls" runat="server" /> to see your requests or <a href="register.aspx">Register</a>.
            </p>
        </asp:PlaceHolder>
        <asp:Literal runat="server" ID="litHome"></asp:Literal>
    </div>
</asp:Content>