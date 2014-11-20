using System;
using System.Linq;
using Hd.Web.REST;

namespace Hd.Web
{
    public partial class ChangeProject : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Int32 i = Convert.ToInt32(Request.QueryString["id"]);

            Session["currentproject"] = i == -1 ? "-1" : ((ProjectCollection)Session["projects"]).Projects.First(p => p.Id == i).Name;
            Response.Redirect(Server.HtmlDecode(Request.QueryString["returnurl"]));
        }
    }
}