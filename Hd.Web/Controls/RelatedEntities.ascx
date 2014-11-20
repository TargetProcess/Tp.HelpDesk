<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_RelatedEntities"
    CodeBehind="RelatedEntities.ascx.cs" %>
<asp:GridView ID="grid" runat="server" AutoGenerateColumns="False" AllowSorting="True"
    CellPadding="0" CellSpacing="0" CssClass="innerTable generalTable" GridLines="None"
    DataKeyNames="ID" RowStyle-Wrap="False"
    HeaderStyle-Wrap="False"
    AllowPaging="false">

    <HeaderStyle CssClass="headRow" />
    <RowStyle CssClass="itemrow itemrowOther" />
    <HeaderStyle CssClass="headRow" />
    <FooterStyle CssClass="footerRow" />
    <Columns>
        <asp:TemplateField HeaderText="#ID" ItemStyle-Width="1%">
            <ItemTemplate>
                <%# Eval("ID") %>
            </ItemTemplate>
            <HeaderStyle Wrap="false" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Type" ItemStyle-Width="1%" ItemStyle-VerticalAlign="Middle">
            <ItemTemplate>
                <tp:Icon ID="icon" runat="server" ImageName='<%# Eval("EntityTypeID", "{0}.gif") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Name">
            <ItemTemplate>
                <%# Eval("Name") %>
            </ItemTemplate>
            <HeaderStyle Wrap="false" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Status">
            <ItemTemplate>
                <%# Eval("StateName") %>
            </ItemTemplate>
            <HeaderStyle Wrap="false" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Release">
            <ItemTemplate>
                <%# Eval("ReleaseName") %>
            </ItemTemplate>
            <HeaderStyle Wrap="false" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Release End Date">
            <ItemTemplate>
                <%# Eval("ReleaseEndDate", "{0:dd-MMM-yyyy}")%>
            </ItemTemplate>
            <HeaderStyle Wrap="false" />
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        There are no related entities found.
    </EmptyDataTemplate>
</asp:GridView>
