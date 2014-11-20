<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/main.master" CodeBehind="Ideas.aspx.cs"
    Inherits="Hd.Web.Ideas" %>

<%@ Register Src="~/Controls/RequestList.ascx" TagName="RequestList" TagPrefix="tp" %>
<%@ Register Src="~/Controls/Navbar.ascx" TagPrefix="tp" TagName="Navbar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="tabs" runat="server">
    <tp:Navbar runat="server" ID="Navbar" CurrentTab="Ideas" />
</asp:Content>
<asp:Content ID="cnt" ContentPlaceHolderID="plcContent" runat="Server">
    <h2>Issues</h2>
    <asp:UpdatePanel runat="server" ID="u">
        <ContentTemplate>
            <tp:GridController ID="controller" runat="server" GridID="ideasRequestList.requestListing"
                QueryType="Hd.Portal.IdeasQuery"
                EntityType="Hd.Portal.Request, Hd.Portal" PagerID="pager" />
            <tp:RequestList ID="ideasRequestList" runat="server" />
            <div style="text-align: right; width: 97%; margin: 0 1%;">
                <tp:Pager ID="pager" runat="server" Width="100%" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
