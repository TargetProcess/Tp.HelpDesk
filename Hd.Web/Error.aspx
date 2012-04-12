<%@ Page Language="C#" AutoEventWireup="true" Inherits="Error" Codebehind="Error.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body style="background: #BDD9FF">
    <form id="form1" runat="server">
        <div style="padding: 20px">
            <h2>
                Whoops!</h2>
            <div style="font: 16px Arial">
                There was something incorrect with system behavior.
                <br />
                <br />
                <b>Most likely Help Desk Portal can't connect to TargetProcess application</b>.<br />
                Please check the following parameters in Web.config:<br />
                <ul>
                    <li>TargetProcessPath </li>
                    <li>AdminLogin </li>
                    <li>AdminPassword </li>
                </ul>
                If problem is still there, <a href="mailto:support@targetprocess.com?subject=error in TargetProcess application&body=">
                    let us know about the problem using your email client</a>.
            </div>
        </div>
        <br />
        <div style="color: #444;padding-left: 5px;">
            <asp:Literal ID="ltError" runat="server"></asp:Literal>
        </div>
        <br />
    </form>
</body>
</html>
