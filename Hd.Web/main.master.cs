// 
// Copyright (c) 2005-2013 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using Hd.Portal;
using Hd.Web.Extensions.Components;
using Hd.Web.REST;

public partial class main : MasterPage
{
    /// <summary>   Event handler. Called by Page for load events. </summary>
    /// <remarks>   Dax Pandhi, 11/17/2014. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    protected void Page_Load(object sender, EventArgs e)
    {
        String theme = GetTheme();

        HtmlLink css = new HtmlLink { Href = "content/" + theme + ".min.css" };
        css.Attributes["rel"] = "stylesheet";
        css.Attributes["type"] = "text/css";
        css.Attributes["media"] = "all";
        Page.Header.Controls.Add(css);

        if (theme == "dark")
        {
            lnkDarkTheme.CssClass = "themelink ";
            lnkDarkTheme.CssClass += (theme == "light" ? "dark" : "light") + "theme";
            lnkDarkTheme.Text = theme == "light" ? "Dark" : "Light";
        }

        lnkDarkTheme.Click += lnkDarkTheme_Click;

        Requester requester = Requester.Logged;

        if (!ReferenceEquals(requester, null))
            lblLoginName.Text = requester.Email;

        if (Requester.IsLoggedAsAnonymous)
        {
            lblLoginName.Text = "Guest";
            settingsLink.NavigateUrl = "login.aspx";
        }

        spanUsername.Visible = !String.IsNullOrEmpty(lblLoginName.Text.Trim());
        tblSearch.Visible = Requester.IsLogged || Requester.IsLoggedAsAnonymous;

        if (!IsPostBack)
            CreateProjectLinks();

        // POSTBACK ONLY below
        if (!IsPostBack)
            return;
        pnlLastAction.Visible = false;

        if (txtSearch.Text != string.Empty)
        {
            PerformSearch();
        }
    }

    /// <summary>   Creates the links for switching projects </summary>
    /// <remarks>   Dax Pandhi, 11/17/2014. </remarks>
    private void CreateProjectLinks()
    {
        if (Session["projects"] == null)
        {
            Session["projects"] = DownloadProjects();
        }
        ProjectCollection projects = (ProjectCollection)Session["projects"];
        StringBuilder sb = new StringBuilder();

        sb.AppendLine(String.Format(
                "<li><a class='projectlink' href='changeproject.aspx?id={0}&returnurl={1}'>{2}</a></li>",
                "-1",
                Server.HtmlEncode(Request.Url.AbsoluteUri),
                "All Projects"));
        
        foreach (Project p in projects.Projects.Where(p => p.IsProduct && p.IsActive))
            sb.AppendLine(String.Format(
                "<li><a class='projectlink' href='changeproject.aspx?id={0}&returnurl={1}'>{2}</a></li>",
                Server.HtmlEncode(p.Id.ToString()),
                Server.HtmlEncode(Request.Url.AbsoluteUri),
                p.Name));

        litProjects.Text = sb + "</ul></div>";

        try
        {
            // Get current project name
            String currentProjectName_original = Session["currentproject"].ToString();
            String currentProjectName = "";
            if (currentProjectName_original.Length > 19)
                currentProjectName = currentProjectName_original.Substring(0, 16) + "...";
            else
                currentProjectName = currentProjectName_original;

            currentProjectName = currentProjectName == "-1" ? "Select Project" : currentProjectName;
            currentProjectName_original = currentProjectName_original == "-1"
                                              ? "Select Project" : currentProjectName_original;

            // If there is no current project, show generic text
            litCurrentProject.Text = "<div class=\"btn-group projectMenu\"><button type=\"button\" title='" + currentProjectName_original + "' class=\"btn btn-default btn-sm dropdown-toggle\" data-toggle=\"dropdown\" aria-expanded=\"false\">"
                + currentProjectName + "<span class=\"caret\"></span></button><ul class=\"dropdown-menu\" role=\"menu\">";
        }
        catch (Exception)
        {
            // Something went wrong, so just show generic text
            litCurrentProject.Text = "<div class=\"btn-group projectMenu\"><button type=\"button\" class=\"btn btn-default btn-sm dropdown-toggle\" data-toggle=\"dropdown\" aria-expanded=\"false\">Select Project<span class=\"caret\"></span></button><ul class=\"dropdown-menu\" role=\"menu\">";
        }

    }

    /// <summary>   Downloads list of projects from the TargetProcess using REST API. </summary>
    /// <remarks>   Dax Pandhi, 11/17/2014. </remarks>
    /// <returns>   A ProjectCollection. </returns>
    private static ProjectCollection DownloadProjects()
    {
        WebClient client = new WebClient
        {
            Credentials =
                new NetworkCredential(ConfigurationManager.AppSettings["AdminLogin"],
                                      ConfigurationManager.AppSettings["AdminPassword"])
        };
        string xml = client.DownloadString(ConfigurationManager.AppSettings["TargetProcessPath"]
            + "/api/v1/Projects/?include=[ID,Name,IsActive,IsProduct]&take=500");

        var serializer = new XmlSerializer(typeof(ProjectCollection));
        var userStoryCollection = (ProjectCollection)serializer.Deserialize(new StringReader(xml));
        return userStoryCollection;
    }

    private string GetTheme()
    {
        // Get theme from WebSettings
        string theme = ConfigurationManager.AppSettings["Theme"] == "Dark" ? "dark" : "light";

        // Check if user is allowed to switch themes
        Boolean allowThemeSwitching = ConfigurationManager.AppSettings["AllowThemeSwitching"] == "true";

        // Hide theme switching if turned off
        lnkDarkTheme.Visible = allowThemeSwitching;

        if (!allowThemeSwitching)
            return theme;
        // Try to get cookie
        try
        {
            theme = Request.Cookies["tphelpdesktheme"].Value;
        }
        catch (Exception)
        {
            // No cookie, or error so ignore and move on
        }
        return theme;

    }

    void lnkDarkTheme_Click(object sender, EventArgs e)
    {
        // Toggle the theme
        Response.SetCookie(new HttpCookie("tphelpdesktheme", GetTheme() == "light" ? "dark" : "light"));
        Response.Redirect(Request.RawUrl);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        PerformSearch();
    }

    private void PerformSearch()
    {
        if (txtSearch.Text == string.Empty)
            return;

        string url = "~/Search.aspx";
        string param = "SearchString=" + txtSearch.Text;
        try
        {
            int requestID = int.Parse(txtSearch.Text);
            url = "~/ViewRequest.aspx";
            param = "RequestID=" + requestID;
        }
        catch (Exception)
        {
            // NOT an integer
        }

        url = Globals.AppendQueryParameters(url, param);
        Response.Redirect(Globals.ResolveClientUrl(url));
    }

    protected void loginStatus_LoggingOut(object sender, LoginCancelEventArgs e)
    {
        Session.Clear();
        Globals.IsLogOut = false;
        FormsAuthentication.RedirectToLoginPage();
    }

}
