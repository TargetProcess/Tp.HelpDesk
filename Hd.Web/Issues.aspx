<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/main.master"  Inherits="Issues" Codebehind="Issues.aspx.cs" %>
<%@ Register Src="~/Controls/RequestList.ascx" TagName="RequestList" TagPrefix="tp" %>
<%@ Import Namespace="Hd.Portal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="tabs" runat="server">
    <div id="tabs">
        <div class="tab" style="float: left" runat="server" id="div2">
            <a href="Default.aspx">Home</a></div>
        <tp:RequestScopeTab ID="RequestScopeTab1" Scope="Private" CssClass="tab" runat="server" Text="My Requests"
            Url="MyRequests.aspx" />
        <tp:RequestScopeTab ID="RequestScopeTab2" IsPublic="True" Scope="Global" CssClass="selectedTab" runat="server" Url="Issues.aspx"
            Text="Issues" />
		<tp:RequestScopeTab ID="RequestScopeTab4" CssClass="tab" IsPublic="True" Scope="Global" runat="server" Url="Ideas.aspx" Text="Ideas" />
        <tp:RequestScopeTab ID="RequestScopeTab3" Scope="Private" CssClass="clearTab" runat="server" Url="Request.aspx"
            Text="Add Request" />
    </div>
</asp:Content>
<asp:Content ID="cnt" ContentPlaceHolderID="plcContent" runat="Server">
		<asp:UpdatePanel runat="server" ID="u">
		<ContentTemplate>

			<tp:GridController ID="controller" runat="server" GridID="issuesRequestList.requestListing" QueryType="Hd.Portal.IssuesQuery"
				EntityType="Hd.Portal.Request, Hd.Portal" PagerID="pager" />
			<tp:RequestList ID="issuesRequestList" runat="server" />
			<div style="text-align: right; width: 97%; margin: 0 1%;">
				<tp:Pager ID="pager" runat="server" Width="100%" />
			</div>

		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
