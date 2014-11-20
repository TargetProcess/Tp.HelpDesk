<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/main.master" Inherits="Issues"
    CodeBehind="Issues.aspx.cs" %>

<%@ Register Src="~/Controls/RequestList.ascx" TagName="RequestList" TagPrefix="tp" %>
<%@ Register Src="~/Controls/Navbar.ascx" TagPrefix="tp" TagName="Navbar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="tabs" runat="server">
    <tp:Navbar runat="server" ID="Navbar" CurrentTab="Issues" />
</asp:Content>
<asp:Content ID="cnt" ContentPlaceHolderID="plcContent" runat="Server">
    <div class="row">
        <div class="col-xs-12">
            <h2>Issues</h2>
        </div>
    </div>

    <asp:UpdatePanel runat="server" ID="u">
        <ContentTemplate>
            <tp:GridController ID="controller" runat="server" GridID="issuesRequestList.requestListing"
                QueryType="Hd.Portal.IssuesQuery"
                
                EntityType="Hd.Portal.Request, Hd.Portal" PagerID="pager" />
            <tp:RequestList ID="issuesRequestList" runat="server" />
            <div class="pager">
                <tp:Pager ID="pager" runat="server" Width="100%" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
