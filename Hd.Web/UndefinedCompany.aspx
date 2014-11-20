<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/main.master"
    Inherits="UndefinedCompany" CodeBehind="UndefinedCompany.aspx.cs" %>
<%@ Register Src="~/Controls/Navbar.ascx" TagPrefix="tp" TagName="Navbar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="tabs" runat="server">
    <tp:Navbar runat="server" ID="Navbar" CurrentTab="MyRequests" />
</asp:Content>
<asp:Content ID="cnt" ContentPlaceHolderID="plcContent" runat="Server">
    <div style="padding: 10px 10px; margin: 0px 25px; background: #EEE; border: 1px solid #999">
        <p>Your company is undefined, you are not supposed to add any request till TargetProcess
            Admin assigns you to company. </p>
    </div>
</asp:Content>
