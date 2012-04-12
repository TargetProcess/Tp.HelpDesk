<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductDropDown.ascx.cs" Inherits="Hd.Web.Controls.ProductDropDown" %>

<asp:Panel ID="pnlProductList" runat="server">
	<br />
	<br />
	<span class="fieldName">Product (required)</span><br />
	<tp:TpDropDownList ID="lstProductList" runat="server" DataSourceID="productsSource"
		DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true" SelectedValue='<%# Eval("ProjectID") %>'>
		<asp:ListItem Value="-1">- Select Product -</asp:ListItem>
	</tp:TpDropDownList>
	<asp:RegularExpressionValidator ID="vldProduct" runat="server" ControlToValidate="lstProductList"
		ErrorMessage="*" Display="Dynamic" ValidationExpression="\d+"></asp:RegularExpressionValidator>
</asp:Panel>

<tp:TpObjectDataSource ID="productsSource" runat="server" DataObjectTypeName="Hd.Portal.Request"
	SelectMethod="RetrieveProducts" TypeName="Hd.Portal.Request">
</tp:TpObjectDataSource>