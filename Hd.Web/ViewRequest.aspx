<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
	Inherits="ViewRequest" Title="Untitled Page" Codebehind="ViewRequest.aspx.cs" %>

<%@ Register Src="~/Controls/RelatedEntities.ascx" TagName="RelatedEntities" TagPrefix="tp" %>
<%@ Register Src="~/Controls/AttachmentComment.ascx" TagName="AttachmentComment" TagPrefix="tp" %>
<%@ Register Namespace="Hd.Web.Controls" TagPrefix="tp" Assembly="Hd.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="tabs" runat="server">
	<div id="tabs">
		<div class="selectedTab" style="float: left" runat="server" id="div2">
			<a  href="Default.aspx" >Home</a></div>
		<tp:RequestScopeTab ID="RequestScopeTab1" Scope="Private" CssClass="tab" runat="server" Text="My Requests"
			Url="MyRequests.aspx" />
		<tp:RequestScopeTab ID="RequestScopeTab2" IsPublic="true" Scope="Global" CssClass="tab" runat="server" Url="Issues.aspx"
			Text="Issues" />
		<tp:RequestScopeTab ID="RequestScopeTab4" IsPublic="true" Scope="Global" CssClass="tab" runat="server" Url="Ideas.aspx"
			Text="Ideas" />
		<tp:RequestScopeTab ID="RequestScopeTab3" Scope="Private" CssClass="clearTab" runat="server" Url="Request.aspx"
			Text="Add Request" />
	</div>
</asp:Content>
<asp:Content ID="cnt" ContentPlaceHolderID="plcContent" runat="Server">
	<div id="main" style="width: 100%">
		<table cellspacing="2" cellpadding="2" style="width: 100%">
			<tr>
				<td style="width: 1%">
					&nbsp;</td>
				<td colspan='3'>
					<asp:FormView ID="title" runat="server" DataSourceID="RequestSource" DataKeyNames="RequestID"
						DefaultMode="ReadOnly" Width="100%">
						<ItemTemplate>
							<span style="font: bold 16px Arial">#<%# Eval("ID") %>.&nbsp;<%# Eval("Name") %>
							</span>
						</ItemTemplate>
					</asp:FormView>
				</td>
			</tr>
			<tr valign="top">
				<td style="width: 1%">
					&nbsp;</td>
				<td style="width: 63%">
					<tp:TabControl ID="tabControl" runat="server" TabCssClass="tabInner" SelectedTabCssClass="tabInnerSelected"
						TabContentCssClass="tabInnerContent" TabPanelCssClass="tabInnerPanel" QueryStringField="RequestID">
						<tp:StaticTab ID="general" runat="server" TabTitle="General">
							<asp:FormView ID="requestDetails" runat="server" DataSourceID="RequestSource" DataKeyNames="RequestID"
								DefaultMode="ReadOnly" Width="100%">
								<ItemTemplate>
									<tp:DescriptionLabel ID="lblDescription" runat="server" Text='<%# Eval("Description") %>' /><br />
									<br />
								</ItemTemplate>
							</asp:FormView>
							<tp:AttachmentComment ID="ac" runat="server"   />
						</tp:StaticTab>
						<tp:DynamicTab ID="entities" runat="server" TabTitle="Related Entities" UpdateMode="Conditional">
							<ContentTemplate>
								<tp:RelatedEntities ID="re" runat="server" />
							</ContentTemplate>
						</tp:DynamicTab>
					</tp:TabControl>
				</td>
				<td>
					<td style="width: 2%">
						&nbsp;</td>
					<td style="width: 35%">
						<div class="grayTitle">
							<div class="blueLeftSep">
								Additional Info</div>
						</div>
						<asp:FormView ID="requestView" runat="server" DataSourceID="RequestSource" DataKeyNames="RequestID"
							DefaultMode="ReadOnly" Width="100%">
							<ItemTemplate>
								<table cellspacing="0" class="generalTable infoTable" cellpadding="4" style="width: 100%">
									<tr>
										<td>
											State</td>
										<td>
											<%# Eval("EntityState.Name") %>
											<td>
									</tr>
									<tr>
										<td>
											Request Type</td>
										<td>
											<%# Eval("RequestType.Name") %>
										</td>
									</tr>
									<tr>
										<td>
											Votes</td>
										<td>
											<%# Eval("RequestersCount")%>
										</td>
									</tr>
									<tr>
										<td>
											Source</td>
										<td>
											<%# Eval("SourceType")%>
										</td>
									</tr>
									<tr>
										<td>
											Assignments
										</td>
										<td>
											<tp:TeamLabel ID="tl" runat="server" Teams='<%# Eval("Teams") %>' />
										</td>
									</tr>
									<tr>
										<td>
											Owner
										</td>
										<td>
											<%# Eval("Owner.FullName")%>
										</td>
									</tr>
									<tr>
										<td>
											Last Editor
										</td>
										<td>
											<%# Eval("LastEditor.FullName")%>
										</td>
									</tr>
									<tr>
										<td>
											Creation Date
										</td>
										<td>
											<%# Eval("CreateDate", "{0:dd-MMM-yy}")%>
										</td>
									</tr>
									<tr>
										<td>
											Completion Date
										</td>
										<td>
											<%# Eval("EndDate", "{0:dd-MMM-yy}")%>
										</td>
									</tr>
								</table>
							</ItemTemplate>
						</asp:FormView>
					</td>
			</tr>
		</table>
	</div>
	<tp:TpObjectDataSource ID="RequestSource" runat="server" DataObjectTypeName="Hd.Portal.Request"
		SelectMethod="RetrieveOrCreate" OnSelected="requestSource_OnSelected" TypeName="Hd.Portal.Request"
		UpdateMethod="Save">
		<SelectParameters>
			<asp:QueryStringParameter QueryStringField="RequestId" DefaultValue="0" Name="RequestId"
				Type="Int32" />
		</SelectParameters>
	</tp:TpObjectDataSource>
</asp:Content>
