using System;
using System.Configuration;
using System.Web.UI.HtmlControls;
using Hd.Portal;
using Hd.Portal.Entities.PasswordRecovery;
using Hd.Web.Extensions;

public partial class ResetPasswordPage : PersisterBasePage
{
    private Requester requester;

    protected void Page_Load(object sender, EventArgs e)
    {

        // Get theme from WebSettings
        string theme = ConfigurationManager.AppSettings["Theme"] == "Dark" ? "dark" : "light";

        // Check if user is allowed to switch themes
        Boolean allowThemeSwitching = ConfigurationManager.AppSettings["AllowThemeSwitching"] == "true";

        if (allowThemeSwitching)
        {
            // Try to get cookie
            try
            {
                theme = Request.Cookies["tphelpdesktheme"].Value;
            }
            catch (Exception)
            {
                // No cookie, or error so ignore and move on
            }
        }

        HtmlLink css = new HtmlLink { Href = "content/" + theme + ".min.css" };
        css.Attributes["rel"] = "stylesheet";
        css.Attributes["type"] = "text/css";
        css.Attributes["media"] = "all";

        Page.Header.Controls.Add(css);

        string activationKey = Request["ActivationKey"];
        if (string.IsNullOrEmpty(activationKey))
        {
            ShowAccessDenied();
            return;
        }

        requester = PasswordRecovery.RetrieveByActivationKey(activationKey);

        if (requester == null)
        {
            ShowAccessDenied();
            return;
        }

        requesterName.Text = string.Format("{0} {1}", requester.FirstName, requester.LastName);
    }

    private void ShowAccessDenied()
    {
        resetPasswordPanel.Visible = false;
        accessDeniedPanel.Visible = true;
    }

    private bool ConfirmPassword()
    {
        if (newPassword.Text == reenterNewPassword.Text)
            return true;
        passwordValidation.Visible = true;
        return false;
    }

    protected void save_OnClick(object sender, EventArgs e)
    {
        if (!ConfirmPassword())
            return;

        requester.Password = newPassword.Text;
        Requester.Save(requester);
        Response.Redirect("Default.aspx");
    }
}