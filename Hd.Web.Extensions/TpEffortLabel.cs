using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hd.Components;

namespace Hd.Web.Extensions
{
    /// <summary>
    /// Formats effort values. May contain logic to display proper effort unit
    /// </summary>
    public class TpEffortLabel : TpLabel
    {
        private bool _UseBaseRender = false;

        public bool UseBaseRender
        {
            get { return _UseBaseRender; }
            set { _UseBaseRender = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                writer.Write(Formatter.FormatDecimal(new decimal()));
                return;
            }

            decimal originalValue;
            if (Decimal.TryParse(base.Text, out originalValue))
            {
                String output = Formatter.FormatDecimal(originalValue);
                Text = output + " h";
            }
            else
            {
                Text = "N/A";
            }

            if (!UseBaseRender)
                writer.Write(Text); // for html size reducing, will not render <span>
            else 
                base.Render(writer);
        }
    }
}