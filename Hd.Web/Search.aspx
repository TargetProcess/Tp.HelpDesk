<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/main.master" Inherits="SearchPage" Codebehind="Search.aspx.cs" %>
<%@ Import Namespace="Hd.Portal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="tabs" runat="server">
    <div id="tabs">
        <div class="tab" style="float: left" runat="server" id="div2">
            <a href="Default.aspx">Home</a></div>
        <tp:RequestScopeTab ID="RequestScopeTab1" Scope="Private" CssClass="tab" runat="server" Text="My Requests"
            Url="MyRequests.aspx" />
        <tp:RequestScopeTab ID="RequestScopeTab2" IsPublic="true" Scope="Global" CssClass="tab" runat="server" Url="Issues.aspx"
            Text="Issues" />
        <tp:RequestScopeTab ID="RequestScopeTab4" CssClass="tab" IsPublic="true" Scope="Global" runat="server" Url="Ideas.aspx" Text="Ideas" />
        <tp:RequestScopeTab ID="RequestScopeTab3" Scope="Private" CssClass="clearTab" runat="server" Url="Request.aspx"
            Text="Add Request" />
    </div>
</asp:Content>
<asp:Content ID="cnt" ContentPlaceHolderID="plcContent" runat="Server">
    <tp:VoteManager runat="server" ID="voteManager" />
    <asp:UpdatePanel runat="server" ID="u">
        <ContentTemplate>
            <tp:GridController ID="controller" runat="server" GridID="allRequestListing" QueryType="Hd.Portal.AllRequestQuery"
                EntityType="Hd.Portal.Request, Hd.Portal" PagerID="pager" />
            <tp:VoteHolderGridView ID="allRequestListing" CellPadding="3" CellSpacing="0" CssClass="generalTable"
                runat="server" AutoGenerateColumns="False" GridLines="None" AllowSorting="True"
                AllowPaging="false" PageSize="10" DataKeyNames="RequestID" ShowFooter="true">
                <HeaderStyle CssClass="headRow" />
                <FooterStyle CssClass="footerRow" />
                <Columns>
                    <asp:TemplateField ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <tp:Vote VoteImage="~/img/up.gif" HorizontalAlign="Center" CssVotesCount="votesCount"
                                CssVoteLabel="voteLabel" CssClass="votePanel" RequestID='<%# Eval("ID") %>' IsPossibleToVote='<%# VoteHolderGridView.IsPossibleToVote((int)Eval("ID")) %>'
                                runat="server" ID="vote" Count='<%# Eval("RequestersCount") %>'>
                            </tp:Vote>
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="top" />
                        <HeaderStyle Wrap="False" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Name" ItemStyle-CssClass="big" SortExpression="Name">
                        <ItemTemplate>
                            <a class="requestName" id="A2" href='<%# Eval("ID", "~/ViewRequest.aspx?RequestID={0}") %>'
                                runat="server">
                               <%# GetHighlightedText(Eval("Name")) %>
                            </a>
                            <br />
                            <%# GetHighlightedText(Eval("ShortDescription"))%>
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
                </Columns>
                <EmptyDataTemplate>
                    <b>No requests found</b>
                </EmptyDataTemplate>
            </tp:VoteHolderGridView>
            <div style="text-align: right; width: 97%; margin: 0 1%;">
                <tp:Pager ID="pager" runat="server" Width="100%" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
