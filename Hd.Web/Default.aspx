<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
	Inherits="Default" Title="Untitled Page" EnableViewState="false" Codebehind="Default.aspx.cs" %>
<%@ Import Namespace="Hd.Portal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="tabs" runat="server">
	<div id="tabs">
		<div class="selectedTab" style="float: left" runat="server" id="div2">
			<a href="Default.aspx">Home</a></div>
		<tp:RequestScopeTab CssClass="tab" Scope="Private" runat="server" Text="My Requests" Url="MyRequests.aspx"  />
		<tp:RequestScopeTab CssClass="tab" Scope="Global" IsPublic="true" runat="server" Url="Issues.aspx" Text="Issues" />
		<tp:RequestScopeTab CssClass="tab" Scope="Global" IsPublic="true" runat="server" Url="Ideas.aspx" Text="Ideas" />
		<tp:RequestScopeTab CssClass="clearTab" Scope="Private" runat="server" Url="Request.aspx" Text="Add Request"  />
	</div>
</asp:Content>
<asp:Content ID="cnt" ContentPlaceHolderID="plcContent" runat="Server">
	<div style="padding: 10px 10px; margin: 0px 25px; background: #EEE; border: 1px solid #999">
		<asp:PlaceHolder runat="server" ID="phMessage">
			<p>
				Please
				<tp:TpLoginStatus ID="ls" runat="server" />
				to see your requests or <a href="register.aspx" >Register</a>. </p>
		</asp:PlaceHolder>
		<p>
			You may:</p>
		<ul style="line-height: 1.6">
			<li>Post requests/ideas/issues</li>
			<li>Vote for requests</li>
			<li>View your requests with statuses</li>
			<li>Discuss requests via comments threads</li>
			<li>Attach files to requests</li>
			<li>View related bugs and user stories</li>
		</ul>
	</div>
</asp:Content>
