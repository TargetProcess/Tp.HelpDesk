<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/main.master"
	Inherits="UndefinedCompany" Codebehind="UndefinedCompany.aspx.cs" %>

<asp:content id="Content1" contentplaceholderid="tabs" runat="server">
	<div id="tabs" style="clear: both">
		<div class="selectedTab" style="float: left" runat="server" id="div2">
			<a href="Default.aspx">Home</a></div>
		<tp:RequestScopeTab ID="RequestScopeTab1" CssClass="tab"  runat="server" Text="My Requests" Url="MyRequests.aspx"  />
		<tp:RequestScopeTab ID="RequestScopeTab2" CssClass="tab" runat="server" Url="Issues.aspx" Text="Issues" />
		<tp:RequestScopeTab ID="RequestScopeTab4" CssClass="tab" runat="server" Url="Ideas.aspx" Text="Ideas" />
		<tp:RequestScopeTab ID="RequestScopeTab3" CssClass="clearTab" runat="server" Url="Request.aspx" Text="Add Request"  />
	</div>
</asp:content>
<asp:content id="cnt" contentplaceholderid="plcContent" runat="Server">
  <div style="padding: 10px 10px; margin: 0px 25px; background: #EEE; border: 1px solid #999">
     <p>Your company is undefined, you are not supposed to add any request till TargetProcess Admin assigns you to company. </p>
   </div>
</asp:content>
