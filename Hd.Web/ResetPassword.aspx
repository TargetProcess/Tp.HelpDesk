<%@ Page Language="C#" AutoEventWireup="true" Inherits="ResetPasswordPage" Codebehind="ResetPassword.aspx.cs" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Change Password</title>
</head>
<body>
	<form id="frm" runat="server">
	<table width="100%" style="height: 100%; border: 1px solid #000">
		<tr>
			<td align="center" valign="middle">
				<table width="420" cellpadding="0" cellspacing="0">
					<tr>
						<td class="loginForm">
							<asp:Panel Visible="false" runat="server" ID="accessDeniedPanel">
								<table style="width: 100%">
									<tr>
										<td class="loginTitle" style="height: 38px">
											<span class="loginTitle">Access Denied</span>
										</td>
									</tr>
									<tr>
										<td style="height: 100px">
											You do not have permissions to perform the requested action.
										</td>
									</tr>
								</table>
							</asp:Panel>
							<asp:Panel Visible="true" runat="server" ID="resetPasswordPanel">
								<table style="width: 100%">
									<tr>
										<td>
											<asp:Label runat="server" Visible="false" Style="color: red" ID="errorMessage" />
										</td>
									</tr>
									<tr>
										<td colspan="2" class="loginTitle" style="height: 38px">
											<span class="loginTitle">Change Password
											<asp:Label runat="server" Text="" ID="requesterName" /></span>
										</td>
									</tr>
									<tr>
										<td colspan="2">
											Use this form to change your password. Once changed, your new password will be in
											effect next time you login.<br />
											<br />
										</td>
									</tr>
									<tr>
										<td>
											New Password
										</td>
										<td>
											<asp:RequiredFieldValidator ID="newPasswordValidator" Display="Dynamic" runat="server"
												ErrorMessage="Please enter new password" ControlToValidate="newPassword"></asp:RequiredFieldValidator>
											<asp:TextBox CssClass="inputLarge" TextMode="Password" runat="server" ID="newPassword"></asp:TextBox>
										</td>
									</tr>
									<tr>
										<td style="white-space: nowrap">
											Re-enter New Password
										</td>
										<td>
											<span style="white-space: nowrap">
												<asp:TextBox CssClass="inputLarge" TextMode="Password" runat="server" ID="reenterNewPassword"></asp:TextBox>
												<asp:Label runat="server" Visible="false" ID="passwordValidation" Style="color: red"
													Text="*" /></span>
										</td>
									</tr>
									<tr>
										<td colspan="2">
											<asp:Button CssClass="inputLarge" runat="server" ID="save" Text="Save" OnClick="save_OnClick" />
										</td>
									</tr>
								</table>
							</asp:Panel>
						</td>
					</tr>
					<tr>
						<td>
							<div class="copy">
								Copyright &copy; 2004-2007 <a href="http://www.targetprocess.com" target="_blank">TargetProcess.</a>
								All rights reserved.<br />
							</div>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	</form>
</body>
</html>
