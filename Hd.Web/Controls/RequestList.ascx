<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequestList.ascx.cs" Inherits="Hd.Web.Controls.RequestList" %>
<%@ Import Namespace="Hd.Portal"%>

<tp:VoteManager runat="server" ID="voteManager" />
<tp:VoteHolderGridView ID="requestListing" CellPadding="3" CellSpacing="0" CssClass="generalTable"
    runat="server" AutoGenerateColumns="False" GridLines="None" AllowSorting="True"
    AllowPaging="false" PageSize="10" DataKeyNames="RequestID" ShowFooter="true">
    <HeaderStyle CssClass="headRow" />
    <FooterStyle CssClass="footerRow" />
    <Columns>
        <asp:TemplateField HeaderText="Votes" SortExpression="size(request.Requesters)">
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