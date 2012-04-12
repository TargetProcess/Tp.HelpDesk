<%@ Page Language="C#" AutoEventWireup="true" Inherits="TpLogin" Codebehind="Login.aspx.cs" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Login</title>
</head>
<body>
	<form id="frm" runat="server">
	<asp:ScriptManager ID="sm" runat="server" ScriptMode="Release" />
	<table width="100%" style="height: 100%; border: 1px solid #000">
		<tr>
			<td align="center" valign="middle">
				<table width="420" cellpadding="0" cellspacing="0">
					<tr>
						<td colspan="5" align="center" class="loginForm">
							<asp:UpdatePanel runat="server" ID="mailPanel">
								<ContentTemplate>
									<asp:Panel runat="server" ID="loginPanel">
										<table id="loginPanel" cellpadding="0" cellspacing="0" style="margin: 20px">
											<tr>
												<td colspan="2" class="loginTitle" style="height: 38px">
													<span class="loginTitle">Help Desk Portal / Authorize&nbsp;</span>
												</td>
											</tr>
											<tr>
												<td colspan="2" style="color: #507cb6">
													<asp:Literal ID="FailureText" runat="server"></asp:Literal>
												</td>
											</tr>
											<tr>
												<td>
													<br />
													Email
												</td>
												<td>
													<br />
													<asp:TextBox ID="UserName" CssClass="inputLarge" runat="server"></asp:TextBox>
													<asp:RequiredFieldValidator ValidationGroup="login" ID="UserNameRequired" runat="server"
														ControlToValidate="UserName" Text="*"></asp:RequiredFieldValidator>
												</td>
											</tr>
											<tr>
												<td>
													Password
												</td>
												<td>
													<asp:TextBox ID="Password" CssClass="inputLarge" runat="server" TextMode="Password"></asp:TextBox>
													<asp:RequiredFieldValidator ValidationGroup="login" ID="PasswordRequired" runat="server"
														ControlToValidate="Password" Text="*"></asp:RequiredFieldValidator>
												</td>
											</tr>
											<tr>
												<td>
													&nbsp;
												</td>
												<td>
													<asp:LinkButton OnClick="forgotPassword_OnForgotPassword" runat="server" ID="forgotPassword">Forgot your password?</asp:LinkButton>
												</td>
											</tr>
											<tr>
												<td>
												</td>
												<td>
													<asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time"></asp:CheckBox>
												</td>
											</tr>
											<tr>
												<td>
												</td>
												<td>
													<asp:Button ID="btnLogin" ValidationGroup="login" CssClass="inputLarge" CommandName="Login"
														runat="server" Text="Enter" OnCommand="OnLogin"></asp:Button>
													<asp:Button ID="btnLoginAsGuest" CssClass="inputLarge"  CommandName="LoginAsGuest" runat="server" Text="Guest" OnCommand="OnLogin" />
												</td>
											</tr>
											<tr>
												<td align="left" colspan="2">
													Please login to see your requests or <a href="register.aspx">Register</a>.
												</td>
											</tr>
										</table>
									</asp:Panel>
									<asp:Panel Visible="false" runat="server" ID="forgotPasswordPanel">
										<table class="loginForm">
											<tr>
												<td colspan="2" class="loginTitle" style="height: 38px">
													<span class="loginTitle">Reset Password</span>
												</td>
											</tr>
											<tr>
												<td colspan="2">
													Please enter your email address and we will send you an email with instructions for resetting your password.
												</td>
											</tr>
											<tr>
												<td style="white-space: nowrap">
													Email
												</td>
												<td>
													<asp:Label Style="display:block;color: Red;" ID="errorMessage" Visible="false" runat="server" />
													<asp:RequiredFieldValidator ID="emptyEmailValidator" Display="Dynamic" runat="server"
														ErrorMessage="Please enter email" ControlToValidate="emailToSendPassword"></asp:RequiredFieldValidator>
													<asp:RegularExpressionValidator ID="correctEmailValidator" runat="server" ControlToValidate="emailToSendPassword"
														ErrorMessage="Please enter valid email" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
													<asp:TextBox CssClass="inputLarge" ID="emailToSendPassword" runat="server" />
												</td>
											</tr>
											<tr>
												<td>
													&nbsp;
												</td>
												<td>
													<asp:Button CssClass="inputLarge" OnClick="OnSendPasswordButtonClick" Text="Send"
														runat="server" ID="sendPassword" />
												</td>
											</tr>
										</table>
									</asp:Panel>
									<asp:Panel Visible="false" runat="server" ID="passwordSentPanel">
										<table class="loginForm">
											<tr>
												<td colspan="2" class="loginTitle" style="height: 38px">
													<span class="loginTitle">Reset Password</span>
												</td>
											</tr>
											<tr>
												<td colspan="2">
<b>User Password Link Sent</b>
<br/>
You have requested a link be sent to your email address  which will enable you to reset your password. Please allow up to 15 minutes for it to arrive. 
	
												</td>
											</tr>
											</table>
									</asp:Panel>
								</ContentTemplate>
							</asp:UpdatePanel>
						</td>
					</tr>
					<tr>
						<td colspan="5" class="copy">
							Copyright &copy; 2004-2012 <a href="http://www.targetprocess.com" target="_blank">TargetProcess.</a>
							All rights reserved.<br />
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	</form>
</body>
</html>
