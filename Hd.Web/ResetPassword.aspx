<%@ Page Language="C#" AutoEventWireup="true" Inherits="ResetPasswordPage" CodeBehind="ResetPassword.aspx.cs" %>

<html>
<head runat="server">
    <title>Change Password</title>
    <link href="Content/bootstrap/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="frm" runat="server">
            <br />
    <br />
    <br />
    <div class="container">
        <div class="row">
            <div class="col-lg-4"></div>
            <div class="col-lg-4 contentbg repad">
                <asp:Panel Visible="True" runat="server" ID="accessDeniedPanel">
                    <h2>Access Denied</h2>
                    <p class="alert alert-danger">
                        You do not have permissions to perform the requested action.
                    </p>
                </asp:Panel>
                <asp:Panel Visible="true" CssClass="repad" runat="server" ID="resetPasswordPanel">
                    <asp:Label runat="server" Visible="false" CssClass="alert alert-warning" ID="errorMessage" />
                    <h2>Change Password<asp:Label runat="server" Text="" ID="requesterName" /></h2>
                    <p>
                        Use this form to change your password. Once changed, your new password will be in
                        effect next time you login.
                    </p>
                    <div class="form-group">
                        <label>
                            New Password
                        </label>
                        <asp:RequiredFieldValidator ID="newPasswordValidator" Display="Dynamic" runat="server"
                            ErrorMessage="Please enter new password" ControlToValidate="newPassword"></asp:RequiredFieldValidator>
                        <asp:TextBox CssClass="form-control" TextMode="Password" runat="server" ID="newPassword"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>
                            Re-enter New Password
                        </label>
                        <asp:TextBox CssClass="form-control" TextMode="Password" runat="server" ID="reenterNewPassword"></asp:TextBox>
                        <asp:Label runat="server" Visible="false" ID="passwordValidation" CssClass="alert alert-danger"
                            Text="*" />
                    </div>
                    <asp:Button CssClass="btn btn-success" runat="server" ID="save" Text="Save" OnClick="save_OnClick" />
                </asp:Panel>
            </div>
        </div>
        <div id="copy">
            Copyright &copy; 2004-2007 <a href="http://www.targetprocess.com" target="_blank">TargetProcess.</a>
            All rights reserved.<br />
        </div>
    </div>
    </form>
</body>
</html>
