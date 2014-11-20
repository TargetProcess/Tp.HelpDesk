using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hd.Portal;
using Hd.Portal.Components.LastActionProcessor;
using Hd.Web.Extensions.Components;
using Tp.EntityStateServiceProxy;


namespace Hd.Web.Extensions
{
    public class LastActionLabel : Panel
    {
        public static readonly string DO_RESET = "DO_RESET_ACTION";

        public static bool DoReset
        {
            set { HttpContext.Current.Items[DO_RESET] = value; }
            get
            {
                return (HttpContext.Current.Items[DO_RESET] == null) || (bool)HttpContext.Current.Items[DO_RESET];
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            HorizontalAlign = HorizontalAlign.Center;
            // writer.Write("<button type=\"button\" class=\"close\" aria-hidden=\"true\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>");

            Label label = new Label { ID = "lblLastAction" };

            if (ActionProcessor.IsError)
                label.Text = "ERROR: ";

            if (ActionProcessor.LastAction != null)
            {
                label.Text += ActionProcessor.LastAction;
                Controls.Add(label);

                ProcessLastEntity();

                if (DoReset)
                    ActionProcessor.Clear();

                Style.Remove("display");
            }
            else
            {
                Style.Add("display", "none");
                // writer.Write("<script type=\"text/javascript\">$(document).ready(function () {$(\".alert\").alert('close');});</script>");
            }

            base.Render(writer);
        }

        private void ProcessLastEntity()
        {
            if (ActionProcessor.LastEntity is Request && ActionProcessor.LastActionTypeEnum != ActionTypeEnum.None)
            {
                if (ActionProcessor.LastActionTypeEnum == ActionTypeEnum.Add
                    || ActionProcessor.LastActionTypeEnum == ActionTypeEnum.Update)
                {
                    Literal literal = new Literal { Text = ".&nbsp;" };

                    Controls.Add(literal);

                    HyperLink lnkView = new HyperLink
                                        {
                                            Text = "View Details", 
                                            NavigateUrl = "~/ViewRequest.aspx"
                                        };

                    lnkView.NavigateUrl = Globals.AppendQueryParameters(lnkView.NavigateUrl,
                                                                        "RequestID=" + ActionProcessor.LastEntity.ID);
                    Controls.Add(lnkView);
                }
            }
        }
    }
}