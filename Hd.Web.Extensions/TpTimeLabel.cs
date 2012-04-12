using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hd.Components;

namespace Hd.Web.Extensions
{
	public class TpTimeLabel : TpLabel
    {
		public TpTimeLabel()
            : base()
        {}

       
        protected override void Render(HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                writer.Write(Formatter.FormatDecimal(new decimal()));
                writer.Write(" h"); 
                return;
            }
        	
			decimal originalValue;
			if (Decimal.TryParse(base.Text, out originalValue))
			{
				String output = Formatter.FormatDecimal(originalValue);
                Text = output;
                Text += " h";
			}
			else 
			{
                Text = "N/A";
			}
        	
            base.Render(writer);
        }
        
      
    }
}