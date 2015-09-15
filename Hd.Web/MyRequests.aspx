<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    Inherits="MyRequests" Title="Untitled Page" CodeBehind="MyRequests.aspx.cs" %>

<%@ Register Src="~/Controls/Navbar.ascx" TagPrefix="tp" TagName="Navbar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="tabs" runat="server">
    <tp:Navbar runat="server" ID="Navbar" CurrentTab="MyRequests" />
</asp:Content>
<asp:Content ID="cnt" ContentPlaceHolderID="plcContent" runat="Server">
    <tp:VoteManager runat="server" ID="voteManager" />
    <asp:UpdatePanel runat="server" ID="u">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12">
                    <h2>My Requests</h2>
                    <tp:GridController ID="ownerController" runat="server" GridID="rg" QueryType="Hd.Portal.OwnerRequestQuery"
                        EntityType="Hd.Portal.Request, Hd.Portal" PagerID="pager" OnDeleteEntity="OnDeleteEntity" />
                    <asp:GridView ID="rg" CellPadding="0" CellSpacing="0" CssClass="generalTable" runat="server"
                        AutoGenerateColumns="False" GridLines="None" AllowSorting="True" AllowPaging="false"
                        PageSize="10" DataKeyNames="RequestID" ShowFooter="true" RowStyle-Wrap="False"
                        HeaderStyle-Wrap="False">
                        <HeaderStyle CssClass="headRow" />
                        <RowStyle CssClass="itemrow" />
                        <Columns>
                            <asp:TemplateField HeaderText="" SortExpression="size(request.Requesters)">
                                <ItemTemplate>
                                    <div class="row visible-sm visible-xs">
                                        <div class="col-lg-12 repad">
                                            <div class="votestyle votestyle2">
                                                <tp:Vote HorizontalAlign="Center" CssVotesCount="votesCount" CssVoteLabel="voteLabel"
                                                    CssClass="votePanel" RequestID='<%# Eval("ID") %>' IsPossibleToVote="false"
                                                    runat="server" ID="vote" Count='<%# Eval("RequestersCount") %>'>
                                                </tp:Vote>
                                            </div>

                                            <a class="requestName linkOpener" id="A2" href='<%# Eval("ID", "~/ViewRequest.aspx?RequestID={0}") %>'
                                                runat="server" target="popf" title='ID# <%# Eval("ID") %> - <%# Eval("Name") %>'>
                                                <i class="iconHolder icon<%# Eval("RequestType.Name")%>"></i><%# Eval("Name") %>
                                            </a>

                                            <div class="row">
                                                <div class="col-xs-4 excesser"><%# Eval("ProjectName")%></div>
                                                <div class="col-xs-2"><%# Eval("EntityState.Name")%></div>
                                                <div class="col-xs-2"><%# Eval("Priority.Name")%></div>
                                                <div class="col-xs-2">
                                                    <i class="creationdate hidden-xs"></i>
                                                    <tp:AgeLabel ID="a" runat="server" Date='<%# Eval("CreateDate") %>'>
                                                    </tp:AgeLabel>
                                                </div>
                                                <div class="col-xs-2 text-right">
                                                    <asp:HyperLink CssClass="editLink btn btn-sm" Visible='<%#IsEditPossible(Container.DataItem)%>'
                                                        ID="le" runat="server"
                                                        NavigateUrl='<%# Eval("ID", "~/Request.aspx?RequestID={0}") %>'>Edit</asp:HyperLink>
                                                    &nbsp;<a class="editLink btn btn-sm btn-warning" runat="server" href='<%# "javascript:" + ownerController.GetDeletePostbackScript(Eval("ID")) %>'>Detach</a>
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
                                            CssClass="votePanel" RequestID='<%# Eval("ID") %>' IsPossibleToVote="false"
                                            runat="server" ID="vote" Count='<%# Eval("RequestersCount") %>'>
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
                                <HeaderStyle CssClass="typeName hidden-sm hidden-xs" />
                                <ItemStyle CssClass="typeName hidden-sm hidden-xs" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Assignments">
                                <ItemTemplate>
                                    <tp:TeamLabel ID="tl" runat="server" Teams='<%# Eval("Teams") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-1 hidden-sm hidden-xs" />
                                <ItemStyle CssClass="col-md-1 hidden-sm hidden-xs overflow" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HyperLink CssClass="editLink btn btn-sm" Visible='<%#IsEditPossible(Container.DataItem)%>'
                                        ID="le" runat="server"
                                        NavigateUrl='<%# Eval("ID", "~/Request.aspx?RequestID={0}") %>'>Edit</asp:HyperLink>
                                    &nbsp;<a runat="server" class="editLink btn btn-sm btn-warning" href='<%# "javascript:" + ownerController.GetDeletePostbackScript(Eval("ID")) %>'>Detach</a>
                                </ItemTemplate>
                                <ItemStyle CssClass="col-md-1 hidden-sm hidden-xs" />
                                <HeaderStyle CssClass="col-md-1 hidden-sm hidden-xs" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <b>No requests found</b>
                        </EmptyDataTemplate>
                    </asp:GridView>
                    <div style="text-align: right; margin: 0 1%;">
                        <tp:Pager ID="pager" runat="server" Width="100%" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12">
                    <h2>Requests I've Voted For</h2>
                    <tp:GridController ID="requesterController" runat="server" GridID="GridView1" QueryType="Hd.Portal.RequesterRequestQuery"
                        EntityType="Hd.Portal.Request, Hd.Portal" PagerID="requesterPager" OnDeleteEntity="OnDeleteEntity" />
                    <asp:GridView ID="GridView1" CellPadding="0" CellSpacing="0" CssClass="generalTable"
                        runat="server"
                        AutoGenerateColumns="False" GridLines="None" AllowSorting="True" AllowPaging="false"
                        PageSize="10" DataKeyNames="RequestID" ShowFooter="true">
                        <HeaderStyle CssClass="headRow" />
                        <RowStyle CssClass="itemrow" />
                        <FooterStyle CssClass="footerRow" />
                        <PagerStyle CssClass="pager" />
                        <Columns>
                            <asp:TemplateField HeaderText="" SortExpression="size(request.Requesters)">
                                <ItemTemplate>
                                    <div class="row visible-sm visible-xs hidden-lg hidden-md">
                                        <div class="col-lg-12 repad">
                                            <div class="votestyle votestyle2">
                                                <tp:Vote HorizontalAlign="Center" CssVotesCount="votesCount" CssVoteLabel="voteLabel"
                                                    CssClass="votePanel" RequestID='<%# Eval("ID") %>' IsPossibleToVote="false"
                                                    runat="server" ID="vote" Count='<%# Eval("RequestersCount") %>'>
                                                </tp:Vote>
                                            </div>

                                            <a class="requestName linkOpener" id="A2" href='<%# Eval("ID", "~/ViewRequest.aspx?RequestID={0}") %>'
                                                runat="server" target="popf" title='<%# Eval("Name") %>'>
                                                <i class="iconHolder icon<%# Eval("RequestType.Name")%>"></i><%# Eval("Name") %>
                                            </a>

                                            <div class="row">
                                                <div class="col-xs-4 excesser"><%# Eval("ProjectName")%></div>
                                                <div class="col-xs-2"><%# Eval("EntityState.Name")%></div>
                                                <div class="col-xs-2"><%# Eval("Priority.Name")%></div>
                                                <div class="col-xs-2">
                                                    <i class="creationdate hidden-xs"></i>
                                                    <tp:AgeLabel ID="a" runat="server" Date='<%# Eval("CreateDate") %>'>
                                                    </tp:AgeLabel>
                                                </div>
                                                <div class="col-xs-2 text-right">
                                                    <asp:HyperLink CssClass="editLink btn btn-sm" Visible='<%#IsEditPossible(Container.DataItem)%>'
                                                        ID="le" runat="server"
                                                        NavigateUrl='<%# Eval("ID", "~/Request.aspx?RequestID={0}") %>'>Edit</asp:HyperLink>
                                                    &nbsp;<a runat="server" class="editLink btn btn-sm btn-warning" href='<%# "javascript:" + ownerController.GetDeletePostbackScript(Eval("ID")) %>'>Detach</a>
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
                                            CssClass="votePanel" RequestID='<%# Eval("ID") %>' IsPossibleToVote="false"
                                            runat="server" ID="vote" Count='<%# Eval("RequestersCount") %>'>
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
                                <HeaderStyle CssClass="typeName hidden-sm hidden-xs" />
                                <ItemStyle CssClass="typeName hidden-sm hidden-xs" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Assignments">
                                <ItemTemplate>
                                    <tp:TeamLabel ID="tl" runat="server" Teams='<%# Eval("Teams") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="col-md-2 hidden-sm hidden-xs" />
                                <ItemStyle CssClass="col-md-2 hidden-sm hidden-xs overflow" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <a class="editLink btn btn-sm btn-warning" runat="server" href='<%# "javascript:" + ownerController.GetDeletePostbackScript(Eval("ID")) %>'>
                                        Detach</a>
                                </ItemTemplate>
                                <ItemStyle CssClass="col-md-1 hidden-sm hidden-xs" />
                                <HeaderStyle CssClass="col-md-1 hidden-sm hidden-xs" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <b>No requests found</b>
                        </EmptyDataTemplate>
                    </asp:GridView>
                    <div style="text-align: right; margin: 0 1%;">
                        <tp:Pager ID="requesterPager" runat="server" Width="100%" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
