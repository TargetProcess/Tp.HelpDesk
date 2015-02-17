<%@ Page Language="C#" AutoEventWireup="true" Inherits="TpLogin" CodeBehind="Login.aspx.cs" %>

<%@ Import Namespace="Hd.Portal.Components" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="Content/bootstrap/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="frm" runat="server">
    <asp:ScriptManager ID="sm" runat="server" ScriptMode="Release" />
    <br />
    <br />
    <br />
    <div class="container">
        <div class="row">
            <div class="col-lg-4"></div>
            <div class="col-lg-4 contentbg repad" id="logoHolder" runat="server">
                <div class="row whitebg">
                    <div class="repad">
                        <div class="col-lg-6">
                            <img src="logo.svg" class="img-responsive" />
                            <br />
                        </div>
                    </div>
                </div>
                <div class="repad">
                    <asp:UpdatePanel runat="server" ID="mailPanel">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="loginPanel">
                                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
                                <h2><%= Settings.Title %></h2>
                                <br />
                                <div class="form-group">
                                    <asp:TextBox ID="UserName" CssClass="form-control" runat="server" placeholder="Email"></asp:TextBox>
                                    <asp:RequiredFieldValidator ValidationGroup="login" ID="UserNameRequired" runat="server"
                                        ControlToValidate="UserName" Text="Email required" Display="Dynamic" CssClass="valerror"
                                        ForeColor=""></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group">
                                    <asp:TextBox ID="Password" CssClass="form-control" runat="server" TextMode="Password"
                                        placeholder="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ValidationGroup="login" ID="PasswordRequired" CssClass="valerror"
                                        runat="server"
                                        ControlToValidate="Password" Text="Password required" Display="Dynamic" ForeColor=""></asp:RequiredFieldValidator>
                                    <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time"></asp:CheckBox>
                                </div>
                                <div class="row">
                                    <div class="col-xs-3"></div>
                                    <div class="col-xs-3">
                                        <asp:Button ID="btnLogin" ValidationGroup="login" CssClass="btn btn-success btn-sm"
                                            CommandName="Login"
                                            runat="server" Text="Enter" OnCommand="OnLogin"></asp:Button>
                                    </div>
                                    <div class="col-xs-3">
                                        <asp:Button ID="btnLoginAsGuest" CssClass="btn btn-sm" CommandName="LoginAsGuest"
                                            runat="server" Text="Guest" OnCommand="OnLogin" />
                                    </div>
                                </div>
                                <br />
                                <br />
                                <div class="well">
                                    <asp:Panel runat="server" ID="Registration">
                                        Don't have a Help Desk account? <a href="register.aspx" class="">Sign up.</a>
                                        <br />
                                        <br />
                                    </asp:Panel>
                                    <asp:LinkButton OnClick="forgotPassword_OnForgotPassword" runat="server" ID="forgotPassword">Forgot password?</asp:LinkButton>
                                </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel Visible="false" runat="server" ID="forgotPasswordPanel">
                                <h2>Reset Password</h2>
                                <p>
                                    Please enter your email address and we will send you an email with instructions
                                    for resetting your password.
                                </p>
                                <asp:Label Style="display: block; color: Red;" ID="errorMessage" Visible="false"
                                    runat="server" />
                                <asp:RequiredFieldValidator ID="emptyEmailValidator" Display="Dynamic" runat="server"
                                    ErrorMessage="Please enter email" ControlToValidate="emailToSendPassword"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="correctEmailValidator" runat="server" ControlToValidate="emailToSendPassword"
                                    ErrorMessage="Please enter valid email" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                <asp:TextBox CssClass="form-control" placeholder="Email" ID="emailToSendPassword"
                                    runat="server" />
                                <br />
                                <asp:Button CssClass="btn btn-sm btn-warning" OnClick="OnSendPasswordButtonClick"
                                    Text="Send"
                                    runat="server" ID="sendPassword" />
                            </asp:Panel>
                            <asp:Panel Visible="false" runat="server" ID="passwordSentPanel">
                                <h2>Reset Password</h2>
                                <p>
                                    <b>User Password Link Sent</b>
                                    <br />
                                    You have requested a link be sent to your email address  which will enable you to
                                    reset your password. Please allow up to 15 minutes for it to arrive.
                                </p>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div id="copy">
        Copyright &copy; 2004-2015 <a href="http://www.targetprocess.com" target="_blank">TargetProcess.</a>
        All rights reserved.
    </div>
    <div class="waitDialog">
        <div class="text-center">
            <div id="circleG">
                <div id="circleG_1" class="circleG">
                </div>
                <div id="circleG_2" class="circleG">
                </div>
                <div id="circleG_3" class="circleG">
                </div>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript" src="Scripts/jquery-1.9.0.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            // bind your jQuery events here initially
            allFunctions();
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            // re-bind your jQuery events here
            allFunctions();
        });
        function allFunctions() {
            function doWait() {
                $(".waitDialog").center();
            }
            // hide the waitDialog if already visible
            $(".waitDialog").css("display", "none");
            // If something is invalid, do NOT show the waitDialog
            $('form').submit(function () {
                doValidate();
            });
            $(document).on('keypress', function (e) {
                doValidate();
            });
            function doValidate() {
                if (typeof Page_Validators != 'undefined') {
                    $.each(Page_Validators, function () {
                        if (!this.isvalid) {
                            $(this).parent().addClass('has-error');
                            $(".waitDialog").css("display", "none");
                        } else {
                            $(this).parent().removeClass('has-error');
                        }
                    });
                }
            }
            $("#btnLogin").on('click', function (e) {
                $(".waitDialog").center();
            });
            jQuery.fn.center = function () {
                this.css("display", "block")
                this.css("position", "absolute");
                this.css("top", Math.max(0, (($(window).height() - $(this).outerHeight()) / 2) +
                                                            $(window).scrollTop() - 150) + "px");
                this.css("left", Math.max(0, (($(window).width() - $(this).outerWidth()) / 2) +
                                                            $(window).scrollLeft()) + "px");
                return this;
            }
        }
    </script>
</body>
</html>
