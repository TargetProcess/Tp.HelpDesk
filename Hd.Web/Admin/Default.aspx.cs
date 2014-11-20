using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hd.Web.Admin
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;
            try
            {
                try { CKEditor1.Text = File.ReadAllText(Server.MapPath("~/App_Data/home.txt")); }
                catch (Exception) { }
            }
            catch (Exception)
            {
                CKEditor1.Text = "[Content could not be loaded. Try again or contact the administrator.]";
            }
        }

        protected void btnSaveHomePage_Click(object sender, EventArgs e)
        {
            File.WriteAllText(Server.MapPath("~/App_Data/home.txt"), CKEditor1.Text);
        }

        protected void btnResetHomePage_Click(object sender, EventArgs e)
        {

            const string defaultText = "<p>You may:</p><ul><li>Post requests/ideas/issues</li><li>Vote for requests</li><li>View your requests with statuses</li><li>Discuss requests via comments threads</li><li>Attach files to requests</li><li>View related bugs and user stories</li></ul>";
            CKEditor1.Text = defaultText;
            File.WriteAllText(Server.MapPath("~/App_Data/home.txt"), defaultText);
        }
    }
}