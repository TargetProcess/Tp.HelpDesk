<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Navbar.ascx.cs" Inherits="Hd.Web.Controls.Navbar" %>
<div id="tabs">
    <div class="darktop">
        <a class="clearTab" href="Request.aspx"><i class="iconHolder iconAddRequest add"></i> ADD</a>
    </div>
    <div class="tab" runat="server" id="div2">
        <i class="iconHome iconHolder"></i><a href="Default.aspx">Home</a>
    </div>
    <tp:RequestScopeTab ID="RequestScopeTab1" Scope="Private" CssClass="tab" runat="server"
        Text="My Requests"
        Url="~/MyRequests.aspx" />
    <tp:RequestScopeTab ID="RequestScopeTab2" IsPublic="True" Scope="Global" CssClass="tab"
        runat="server" Url="~/Issues.aspx"
        Text="Issues" />
    <tp:RequestScopeTab ID="RequestScopeTab4" CssClass="tab" IsPublic="True" Scope="Global"
        runat="server" Url="~/Ideas.aspx" Text="Ideas" />
</div>
