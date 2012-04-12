using System.Text;
using System.Web.UI.WebControls;

namespace Hd.Web.Extensions
{
    public class Icon : WebControl
    {
        private string _imageName = string.Empty;
        private string _alt = string.Empty;

        public virtual string ImageName
        {
            get { return _imageName; }
            set { _imageName = value; }
        }


        public virtual string Alt
        {
            get { return _alt; }
            set { _alt = value; }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (ImageName == string.Empty)
                return;

            StringBuilder sb = new StringBuilder();
            sb.Append("<img src='")
                .Append(ResolveClientUrl("~/img/" + ImageName))
                .Append("' alt='")
                .Append(Alt)
                .Append("' class='icon' />");

            writer.Write(sb.ToString());
        }
    }
}