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
				return (HttpContext.Current.Items[DO_RESET] == null) ? true : (bool)HttpContext.Current.Items[DO_RESET];
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			HorizontalAlign = HorizontalAlign.Center;

			Label label = new Label();
			label.ID = "lblLastAction";
			BackColor = Color.Gold;
			Style.Add("padding-left", "100px");
			Style.Add("padding-right", "100px");
			Style.Add("padding-top", "2px");
			Style.Add("padding-bottom", "2px");
			Style.Add("margin-top", "12px");

			if (ActionProcessor.IsError)
				BackColor = Color.LightPink;

			if (ActionProcessor.LastAction != null)
			{
				label.Text = ActionProcessor.LastAction;
				Controls.Add(label);

				ProcessLastEntity();

				if (DoReset)
					ActionProcessor.Clear();

				Style.Remove("display");
			}
			else
			{
				Style.Add("display", "none");
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
					Literal literal = new Literal();
					literal.Text = ".&nbsp;";

					Controls.Add(literal);

					HyperLink lnkView = new HyperLink();

					lnkView.Text = "View Details";

					lnkView.NavigateUrl = "~/ViewRequest.aspx";
					lnkView.NavigateUrl = Globals.AppendQueryParameters(lnkView.NavigateUrl,
					                                                    "RequestID=" + ActionProcessor.LastEntity.ID);
					Controls.Add(lnkView);
				}
			}
		}
	}
}