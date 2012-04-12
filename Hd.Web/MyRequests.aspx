<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
	Inherits="MyRequests" Title="Untitled Page" Codebehind="MyRequests.aspx.cs" %>

<%@ Import Namespace="Hd.Portal" %>
<asp:Content ContentPlaceHolderID="tabs" runat="server">
	<div id="tabs">
		<div class="tab" style="float: left" runat="server" id="div2">
			<a href="Default.aspx" >Home</a></div>
		<tp:RequestScopeTab ID="RequestScopeTab1" Scope="Private" CssClass="selectedTab" runat="server" Text="My Requests"
			Url="MyRequests.aspx" />
		<tp:RequestScopeTab ID="RequestScopeTab2" Scope="Global" CssClass="tab" runat="server" Url="Issues.aspx"
			Text="Issues" />
		<tp:RequestScopeTab ID="RequestScopeTab4" CssClass="tab"  Scope="Global" runat="server" Url="Ideas.aspx" Text="Ideas" />
		<tp:RequestScopeTab ID="RequestScopeTab3" Scope="Private" CssClass="clearTab" runat="server" Url="Request.aspx"
			Text="Add Request" />
	</div>
</asp:Content>
<asp:Content ID="cnt" ContentPlaceHolderID="plcContent" runat="Server">
	<tp:VoteManager runat="server" ID="voteManager" />
	<asp:UpdatePanel runat="server" ID="u">
		<ContentTemplate>
			<table width="100%">
			<tr valign="top">
				<td width="50%"><h2 style="padding-left:10px">My Requests</h2></td>
				<td width="50%"><h2 style="padding-left:10px">Requests Voted by me</h2></td>
			</tr>
			<tr valign="top">
			<td width="50%">
				<tp:GridController ID="ownerController" runat="server" GridID="rg" QueryType="Hd.Portal.OwnerRequestQuery"
					EntityType="Hd.Portal.Request, Hd.Portal" PagerID="pager" OnDeleteEntity="OnDeleteEntity" />
				<asp:GridView ID="rg" CellPadding="3" CellSpacing="0" CssClass="generalTable" runat="server"
					AutoGenerateColumns="False" GridLines="None" AllowSorting="True" AllowPaging="false"
					PageSize="10" DataKeyNames="RequestID" ShowFooter="true">
					<HeaderStyle CssClass="headRow" />
					<FooterStyle CssClass="footerRow" />
					<Columns>
						<asp:TemplateField HeaderText="Votes" SortExpression="size(request.Requesters)">
							<ItemTemplate>
								<tp:Vote HorizontalAlign="Center" CssVotesCount="votesCount" CssVoteLabel="voteLabel"
									CssClass="votePanel" RequestID='<%# Eval("ID") %>' IsPossibleToVote="false"
									runat="server" ID="vote" Count='<%# Eval("RequestersCount") %>'>
								</tp:Vote>
							</ItemTemplate>
							<HeaderStyle Wrap="False" />
							<ItemStyle VerticalAlign="top" />
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Name" ItemStyle-CssClass="big" SortExpression="Name">
							<ItemTemplate>
								<a class="requestName" id="A2" href='<%# Eval("ID", "~/ViewRequest.aspx?RequestID={0}") %>'
									runat="server">
									<%# Eval("Name") %>
								</a>
								<br />
								<%# Eval("ShortDescription") %>
							</ItemTemplate>
							<ItemStyle VerticalAlign="top" />
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Priority" SortExpression="Priority.Name">
							<ItemTemplate>
								<%# Eval("Priority.Name")%>
							</ItemTemplate>
							<HeaderStyle Wrap="False" />
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Status" SortExpression="EntityState.Name">
							<ItemTemplate>
								<%# Eval("EntityState.Name")%>
							</ItemTemplate>
							<HeaderStyle Wrap="False" />
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Added" SortExpression="CreateDate">
							<ItemTemplate>
								<tp:AgeLabel ID="a" runat="server" Date='<%# Eval("CreateDate") %>'>
								</tp:AgeLabel>
							</ItemTemplate>
							<HeaderStyle Wrap="False" />
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Type" SortExpression="RequestType.Name">
							<ItemTemplate>
								<%# Eval("RequestType.Name")%>
							</ItemTemplate>
							<HeaderStyle Wrap="False" />
						</asp:TemplateField>
						<asp:TemplateField ItemStyle-Wrap="false" HeaderText="Assignments">
							<ItemTemplate>
								<tp:TeamLabel ID="tl" runat="server" Teams='<%# Eval("Teams") %>' />
							</ItemTemplate>
							<HeaderStyle Wrap="False" />
						</asp:TemplateField>
						<asp:TemplateField ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right">
							<ItemTemplate>
								<asp:HyperLink Visible='<%#IsEditPossible(Container.DataItem)%>'  ID="le" runat="server" NavigateUrl='<%# Eval("ID", "~/Request.aspx?RequestID={0}") %>'>Edit</asp:HyperLink>
								&nbsp;<a runat="server" href='<%# "javascript:" + this.ownerController.GetDeletePostbackScript(Eval("ID")) %>'>Detach</a>
							</ItemTemplate>
							<HeaderStyle Wrap="False" />
						</asp:TemplateField>
					</Columns>
					<EmptyDataTemplate>
						<b>No requests found</b>
					</EmptyDataTemplate>
				</asp:GridView>
				<div style="text-align: right; margin: 0 1%;">
					<tp:Pager ID="pager" runat="server" Width="100%" />
				</div>
			</td>
			<td width="50%">
				<tp:GridController ID="requesterController" runat="server" GridID="GridView1" QueryType="Hd.Portal.RequesterRequestQuery"
					EntityType="Hd.Portal.Request, Hd.Portal" PagerID="requesterPager" OnDeleteEntity="OnDeleteEntity" />
				<asp:GridView ID="GridView1" CellPadding="3" CellSpacing="0" CssClass="generalTable" runat="server"
					AutoGenerateColumns="False" GridLines="None" AllowSorting="True" AllowPaging="false"
					PageSize="10" DataKeyNames="RequestID" ShowFooter="true">
					<HeaderStyle CssClass="headRow" />
					<FooterStyle CssClass="footerRow" />
					<Columns>
						<asp:TemplateField HeaderText="Votes" SortExpression="size(request.Requesters)">
							<ItemTemplate>
								<tp:Vote HorizontalAlign="Center" CssVotesCount="votesCount" CssVoteLabel="voteLabel"
									CssClass="votePanel" RequestID='<%# Eval("ID") %>' IsPossibleToVote="false"
									runat="server" ID="vote" Count='<%# Eval("RequestersCount") %>'>
								</tp:Vote>
							</ItemTemplate>
							<HeaderStyle Wrap="False" />
							<ItemStyle VerticalAlign="top" />
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Name" ItemStyle-CssClass="big" SortExpression="Name">
							<ItemTemplate>
								<a class="requestName" id="A2" href='<%# Eval("ID", "~/ViewRequest.aspx?RequestID={0}") %>'
									runat="server">
									<%# Eval("Name") %>
								</a>
								<br />
								<%# Eval("ShortDescription") %>
							</ItemTemplate>
							<ItemStyle VerticalAlign="top" />
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Priority" SortExpression="Priority.Name">
							<ItemTemplate>
								<%# Eval("Priority.Name")%>
							</ItemTemplate>
							<HeaderStyle Wrap="False" />
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Status" SortExpression="EntityState.Name">
							<ItemTemplate>
								<%# Eval("EntityState.Name")%>
							</ItemTemplate>
							<HeaderStyle Wrap="False" />
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Added" SortExpression="CreateDate">
							<ItemTemplate>
								<tp:AgeLabel ID="a" runat="server" Date='<%# Eval("CreateDate") %>'>
								</tp:AgeLabel>
							</ItemTemplate>
							<HeaderStyle Wrap="False" />
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Type" SortExpression="RequestType.Name">
							<ItemTemplate>
								<%# Eval("RequestType.Name")%>
							</ItemTemplate>
							<HeaderStyle Wrap="False" />
						</asp:TemplateField>
						<asp:TemplateField ItemStyle-Wrap="false" HeaderText="Assignments">
							<ItemTemplate>
								<tp:TeamLabel ID="tl" runat="server" Teams='<%# Eval("Teams") %>' />
							</ItemTemplate>
							<HeaderStyle Wrap="False" />
						</asp:TemplateField>
						<asp:TemplateField ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right">
							<ItemTemplate>
								<asp:HyperLink Visible='<%#IsEditPossible(Container.DataItem)%>'  ID="le" runat="server" NavigateUrl='<%# Eval("ID", "~/Request.aspx?RequestID={0}") %>'>Edit</asp:HyperLink>
								&nbsp;<a id="A1" runat="server" href='<%# "javascript:" + this.requesterController.GetDeletePostbackScript(Eval("ID")) %>'>Detach</a>
							</ItemTemplate>
							<HeaderStyle Wrap="False" />
						</asp:TemplateField>
					</Columns>
					<EmptyDataTemplate>
						<b>No requests found</b>
					</EmptyDataTemplate>
				</asp:GridView>
				<div style="text-align: right; margin: 0 1%;">
					<tp:Pager ID="requesterPager" runat="server" Width="100%" />
				</div>
			</td>
			</tr>
			</table>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
