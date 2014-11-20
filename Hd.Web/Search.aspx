<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/main.master" Inherits="SearchPage"
    CodeBehind="Search.aspx.cs" %>

<%@ Register Src="~/Controls/Navbar.ascx" TagPrefix="tp" TagName="Navbar" %>

<%@ Import Namespace="Hd.Portal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="tabs" runat="server">
    <tp:Navbar runat="server" ID="Navbar" />
</asp:Content>
<asp:Content ID="cnt" ContentPlaceHolderID="plcContent" runat="Server">
    <tp:VoteManager runat="server" ID="voteManager" />
    <asp:UpdatePanel runat="server" ID="u">
        <ContentTemplate>
            <tp:GridController ID="controller" runat="server" GridID="allRequestListing" QueryType="Hd.Portal.AllRequestQuery"
                EntityType="Hd.Portal.Request, Hd.Portal" PagerID="pager" />
            <asp:GridView ID="allRequestListing" CellPadding="3" CellSpacing="0" CssClass="generalTable"
                runat="server" AutoGenerateColumns="False" GridLines="None" AllowSorting="True"
                AllowPaging="false" PageSize="10" DataKeyNames="RequestID" ShowFooter="true">
                <HeaderStyle CssClass="headRow" />

                <RowStyle CssClass="itemrow" />
                <Columns>
                    <asp:TemplateField HeaderText="" SortExpression="size(request.Requesters)">
                        <ItemTemplate>
                            <tp:Vote HorizontalAlign="Center" CssVotesCount="votesCount"
                                CssVoteLabel="voteLabel" CssClass="votePanel" RequestID='<%# Eval("ID") %>' IsPossibleToVote='<%# Requester.IsLogged ? !Hd.Portal.Request.IsRequesterAttached((int)Eval("ID"),  Requester.LoggedUserID.Value ) : false %>'
                                runat="server" ID="vote" Count='<%# Eval("RequestersCount") %>'>
                            </tp:Vote>
                        </ItemTemplate>
                        <ItemStyle CssClass="votestyle" />
                        <HeaderStyle Wrap="False" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Name" ItemStyle-CssClass="big" SortExpression="Name">
                        <ItemTemplate>
                            <%-- Open in Iframe/Popup --%>
                            <a class="requestName linkOpener" id="A2" href='<%# Eval("ID", "~/ViewRequest.aspx?RequestID={0}") %>'
                                target="popf"
                                runat="server">
                                <%# Eval("Name") %>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col-lg-2" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Product"  SortExpression="Project.Name">
                        <ItemTemplate>
                            <%# Eval("ProjectName")%>
                        </ItemTemplate>
                        <HeaderStyle Wrap="False" />
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
                            <i class="creationdate"></i>
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
            </asp:GridView>
            <div style="text-align: right; width: 97%; margin: 0 1%;">
                <tp:Pager ID="pager" runat="server" Width="100%" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
