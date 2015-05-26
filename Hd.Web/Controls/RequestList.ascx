<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequestList.ascx.cs"
    Inherits="Hd.Web.Controls.RequestList" %>
<%@ Import Namespace="Hd.Portal"%>
<tp:VoteManager runat="server" ID="voteManager" />
<tp:VoteHolderGridView ID="requestListing" CellPadding="3" CellSpacing="0" CssClass="generalTable"
    runat="server" AutoGenerateColumns="False" GridLines="None" AllowSorting="True"
    AllowPaging="false" PageSize="10" DataKeyNames="RequestID" ShowFooter="true"
    PagerStyle-CssClass="pager">
    <HeaderStyle CssClass="headRow" />
    <PagerStyle CssClass="pager" />
    <RowStyle CssClass="itemrow" />
    <HeaderStyle CssClass="headRow" />
    <RowStyle CssClass="itemrow" />
    <Columns>
        <asp:TemplateField HeaderText="" SortExpression="size(request.Requesters)">
            <ItemTemplate>
                <div class="row visible-sm visible-xs">
                    <div class="col-lg-12 repad">
                        <div class="votestyle votestyle2">
                            <tp:Vote HorizontalAlign="Center" CssVotesCount="votesCount" CssVoteLabel="voteLabel"
                                CssClass="votePanel" RequestID='<%# Eval("ID") %>' IsPossibleToVote='<%# VoteHolderGridView.IsPossibleToVote((int)Eval("ID")) %>'
                                runat="server" ID="vote1" Count='<%# Eval("RequestersCount") %>'>
                            </tp:Vote>
                        </div>
                        <a class="requestName linkOpener" id="A2" href='<%# Eval("ID", "~/ViewRequest.aspx?RequestID={0}") %>'
                            runat="server" target="popf" title='ID <%# Eval("ID") %> - <%# Eval("Name") %>'>
                            <i class="iconHolder icon<%# Eval("RequestType.Name")%>"></i><%# Eval("Name") %>
                        </a>
                        <div class="row">
                            <div class="col-xs-6 excesser"><%# Eval("ProjectName")%></div>
                            <div class="col-xs-2"><%# Eval("EntityState.Name")%></div>
                            <div class="col-xs-2"><%# Eval("Priority.Name")%></div>
                            <div class="col-xs-2">
                                <i class="creationdate hidden-xs"></i>
                                <tp:AgeLabel ID="a" runat="server" Date='<%# Eval("CreateDate") %>'>
                                </tp:AgeLabel>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
            <HeaderStyle CssClass="col-lg-12 hidden-lg hidden-md" />
            <ItemStyle CssClass="col-lg-12 hidden-lg hidden-md" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Votes" SortExpression="size(request.Requesters)">
            <ItemTemplate>
                <div class="votestyle">
                    <tp:Vote HorizontalAlign="Center" CssVotesCount="votesCount" CssVoteLabel="voteLabel"
                        CssClass="votePanel" RequestID='<%# Eval("ID") %>' IsPossibleToVote='<%# VoteHolderGridView.IsPossibleToVote((int)Eval("ID")) %>'
                    runat="server" ID="vote2" Count='<%# Eval("RequestersCount") %>'>
                </tp:Vote>
                </div>
            </ItemTemplate>
            <ItemStyle CssClass="hidden-sm hidden-xs" />
            <HeaderStyle CssClass="hidden-sm hidden-xs" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="ID" SortExpression="RequestID">
            <ItemTemplate>
                <a class="linkOpener" id="A2" href='<%# Eval("ID", "~/ViewRequest.aspx?RequestID={0}") %>'
                    runat="server" target="popf" title='<%# Eval("Name") %>'>
                    <code class="idtag"><%# Eval("ID") %></code>
                </a>
            </ItemTemplate>
            <ItemStyle CssClass="col-md-1 hidden-sm hidden-xs" />
            <HeaderStyle CssClass="col-md-1 hidden-sm hidden-xs" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Name" SortExpression="Name">
            <ItemTemplate>
                <a class="requestName linkOpener" id="A2" href='<%# Eval("ID", "~/ViewRequest.aspx?RequestID={0}") %>'
                    runat="server" target="popf" title='<%# Eval("Name") %>'>
                    <%# Eval("Name") %>
                </a>
            </ItemTemplate>
            <ItemStyle CssClass="col-md-5 hidden-sm hidden-xs" />
            <HeaderStyle CssClass="col-md-5 hidden-sm hidden-xs" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Product" SortExpression="Project.Name">
            <ItemTemplate>
                <%# Eval("ProjectName")%>
            </ItemTemplate>
            <ItemStyle CssClass="col-md-1 excesser hidden-sm hidden-xs" />
            <HeaderStyle CssClass="col-md-1 hidden-sm hidden-xs" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Priority" SortExpression="Priority.Name">
            <ItemTemplate>
                <%# Eval("Priority.Name")%>
            </ItemTemplate>
            <ItemStyle CssClass="col-md-1 hidden-sm hidden-xs" />
            <HeaderStyle CssClass="col-md-1 hidden-sm hidden-xs" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Status" SortExpression="EntityState.Name">
            <ItemTemplate>
                <%# Eval("EntityState.Name")%>
            </ItemTemplate>
            <HeaderStyle CssClass="col-md-1 hidden-sm hidden-xs" />
            <ItemStyle CssClass="col-md-1 hidden-sm hidden-xs" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Added" SortExpression="CreateDate">
            <ItemTemplate>
                <%--<i class="creationdate"></i>--%>
                <span title='<%# Eval("CreateDate") %>'>
                <tp:AgeLabel ID="a" runat="server" Date='<%# Eval("CreateDate") %>'>
                </tp:AgeLabel>
                </span>
            </ItemTemplate>
            <HeaderStyle CssClass="col-md-1 hidden-sm hidden-xs" />
            <ItemStyle CssClass="col-md-1 hidden-sm hidden-xs overflow" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Type" SortExpression="RequestType.Name">
            <ItemTemplate>
                <i class="iconHolder icon<%# Eval("RequestType.Name")%>"></i>
            </ItemTemplate>
            <HeaderStyle CssClass="col-md-1 hidden-sm hidden-xs" />
            <ItemStyle CssClass="col-md-1 hidden-sm hidden-xs" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Assignments">
            <ItemTemplate>
                <span title='<%# Eval("Teams") %>'>
                <tp:TeamLabel ID="tl" runat="server" Teams='<%# Eval("Teams") %>' />
                </span>
            </ItemTemplate>
            <HeaderStyle CssClass="col-md-2 hidden-sm hidden-xs" />
            <ItemStyle CssClass="col-md-2 hidden-sm hidden-xs overflow" />
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        <b>No requests found</b>
    </EmptyDataTemplate>
</tp:VoteHolderGridView>
