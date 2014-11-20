<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    Inherits="ViewRequest" Title="Untitled Page" CodeBehind="ViewRequest.aspx.cs" %>

<%@ Register Src="~/Controls/RelatedEntities.ascx" TagName="RelatedEntities" TagPrefix="tp" %>
<%@ Register Src="~/Controls/AttachmentComment.ascx" TagName="AttachmentComment"
    TagPrefix="tp" %>
<%@ Register Namespace="Hd.Web.Controls" TagPrefix="tp" Assembly="Hd.Web" %>
<%@ Register Src="~/Controls/Navbar.ascx" TagPrefix="tp" TagName="Navbar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="tabs" runat="server">
    <tp:Navbar runat="server" ID="Navbar" />
</asp:Content>
<asp:Content ID="cnt" ContentPlaceHolderID="plcContent" runat="Server">
    <div id="main" style="width: 100%">
        <div class="row">
            <div class="col-lg-12">
                <asp:FormView ID="title" runat="server" DataSourceID="RequestSource" DataKeyNames="RequestID"
                    DefaultMode="ReadOnly" RenderOuterTable="False">
                    <ItemTemplate>
                        <h2>#<%# Eval("ID") %>.&nbsp;<%# Eval("Name") %>
                        </h2>
                    </ItemTemplate>
                </asp:FormView>
            </div>
            <div class="col-lg-8">
                <tp:TabControl ID="tabControl" runat="server" TabCssClass="tabInner" SelectedTabCssClass="tabInnerSelected"
                    TabContentCssClass="tabInnerContent" TabPanelCssClass="tabInnerPanel" QueryStringField="RequestID">
                    <tp:StaticTab ID="general" runat="server" TabTitle="General">

                        <asp:FormView ID="requestDetails" runat="server" DataSourceID="RequestSource" DataKeyNames="RequestID"
                            DefaultMode="ReadOnly" RenderOuterTable="False">
                            <ItemTemplate>
                                <div class="repad">
                                    <tp:DescriptionLabel CssClass="requestText" ID="lblDescription" runat="server" Text='<%# Eval("Description") %>' />
                                </div>
                            </ItemTemplate>
                        </asp:FormView>
                        <tp:AttachmentComment ID="ac" runat="server" />

                    </tp:StaticTab>
                    <tp:StaticTab ID="entities" runat="server" TabTitle="Related Entities">


                        <tp:RelatedEntities ID="re" runat="server" />

                    </tp:StaticTab>
                </tp:TabControl>
            </div>
            <div class="col-lg-4">
                <div class="repad">
                    <div class="repad additionalinfo">
                        <h4>Additional Info</h4>
                        <asp:FormView ID="requestView" runat="server" DataSourceID="RequestSource" DataKeyNames="RequestID"
                            DefaultMode="ReadOnly" RenderOuterTable="False">
                            <ItemTemplate>
                                <div class="row">
                                    <div class="col-xs-3">
                                        State
                                    </div>
                                    <div class="col-xs-9">
                                        <%# Eval("EntityState.Name") %>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-3">
                                        Request Type
                                    </div>
                                    <div class="col-xs-9">
                                        <%# Eval("RequestType.Name") %>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-3">
                                        Votes
                                    </div>
                                    <div class="col-xs-9">
                                        <span class="votesPanel votesCount">
                                            <%# Eval("RequestersCount")%> votes
                                        </span>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-3">
                                        Source
                                    </div>
                                    <div class="col-xs-9">
                                        <%# Eval("SourceType")%>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-3">
                                        Assignments
                                    </div>
                                    <div class="col-xs-9">
                                        <tp:TeamLabel ID="tl" runat="server" Teams='<%# Eval("Teams") %>' />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-3">
                                        Owner
                                    </div>
                                    <div class="col-xs-9">
                                        <%# Eval("Owner.FullName")%>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-3">
                                        Last Editor
                                    </div>
                                    <div class="col-xs-9">
                                        <%# Eval("LastEditor.FullName")%>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-3">
                                        Creation Date
                                    </div>
                                    <div class="col-xs-9">
                                        <%# Eval("CreateDate", "{0:dd-MMM-yy}")%>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-3">
                                        Completion Date
                                    </div>
                                    <div class="col-xs-9">
                                        <%# Eval("EndDate", "{0:dd-MMM-yy}")%>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:FormView>
                    </div>
                </div>
            </div>
        </div>
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
