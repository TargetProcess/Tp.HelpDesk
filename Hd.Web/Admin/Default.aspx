<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Hd.Web.Admin.Default" %>

<%@ Register TagPrefix="CKEditor" Namespace="CKEditor.NET" Assembly="CKEditor.NET, Version=3.6.6.2, Culture=neutral, PublicKeyToken=e379cdf2f8354999" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TargetProcess HelpDesk Control Panel</title>
    <link href="../Content/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/Light.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="container cp">
        <div class="row">
            <div class="col-lg-12">
                <h3>HelpDesk Settings</h3>
                <div class="row">
                    <div class="col-xs-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">Interface Settings</div>
                            <div class="panel-body">
                                Title<br />
                                <asp:TextBox runat="server" CssClass="form-control input-sm" ID="txtTitle" Text="TargetProcess Help Desk"></asp:TextBox>
                                <br />
                                Logo URL (Default: logo.svg)<br />
                                <asp:TextBox runat="server" CssClass="form-control input-sm" ID="txtLogo" Text="logo.svg"></asp:TextBox>
                                <br />
                                Default Theme:<br />
                                <asp:DropDownList CssClass="form-control" runat="server" ID="ddlTheme">
                                    <asp:ListItem Text="Light" />
                                    <asp:ListItem Text="Dark" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">Preferences</div>
                            <div class="panel-body">
                                Mode
                        <asp:DropDownList CssClass="form-control" runat="server" ID="ddlPublic">
                            <asp:ListItem Text="Public" />
                            <asp:ListItem Text="Private" />
                        </asp:DropDownList>
                                <br />
                                Show Logo
                        <asp:DropDownList CssClass="form-control" runat="server" ID="ddlLogo">
                            <asp:ListItem Text="Yes" />
                            <asp:ListItem Text="No" />
                        </asp:DropDownList>
                                <br />
                                Allow the user to switch themes
                        <asp:DropDownList CssClass="form-control" runat="server" ID="ddlThemeSwitch">
                            <asp:ListItem Text="Yes" />
                            <asp:ListItem Text="No" />
                        </asp:DropDownList>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading">Home Page Content</div>
                    <div class="panel-body">
                        <CKEditor:CKEditorControl ID="CKEditor1" BasePath="~/ckeditor/" runat="server"
                            Skin="moono" Height="400px"></CKEditor:CKEditorControl>
                        <small class="text-primary">Bootstrap 3 tags allowed, however we recommend keeping it
                            lightweight.</small>
                    </div>
                </div>
                <asp:Button CssClass="btn btn-success" runat="server" ID="btnSaveHomePage"
                    Text="Save" OnClick="btnSaveHomePage_Click" />
                <asp:Button CssClass="btn btn-warning" runat="server"
                    ID="btnResetHomePage" Text="Reset text to Default"
                    OnClick="btnResetHomePage_Click" />
            </div>
        </div>
    </div>
    </form>
    <script src="../Scripts/jquery-1.9.0.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
</body>
</html>
