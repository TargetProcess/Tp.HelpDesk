using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hd.Components;

namespace Hd.Web.Extensions
{
	/// <summary>
	/// TextBox that do the following actions on focus:
	/// 1. Shows ToolTip 
	/// 2. Highlights input field
	/// </summary>
	[ToolboxData("<{0}:TpTextBox runat=server></{0}:TpTextBox>")]
	public class TpTextBox : TextBox
	{
		private const string FocusBlurStyles = "this.style.background = '{0}';this.style.border = '1px solid {1}'";

		public TpTextBox()
		{
			BluringEnabled = true;
			OnFocusScript = string.Empty;
			OnBlurScript = string.Empty;
            //FocusBackground = "#FFFEBA";
            //BlurBackground = "#FFF";
            //FocusBorderColor = "orange";
            //BlurBorderColor = "#28428B";
			CleanTags = true;
		}

		public virtual bool CleanTags { get; set; }
		public string ToolTipText { get; set; }
		public string FocusBackground { get; set; }
		public string BlurBackground { get; set; }
		public string FocusBorderColor { get; set; }
		public string BlurBorderColor { get; set; }
		public string OnEnterPressSubmitControl { get; set; }
		public bool IsNumeric { get; set; }
		public bool IsDecimal { get; set; }
		public string OnBlurScript { get; set; }
		public string OnFocusScript { get; set; }
		public bool BluringEnabled { get; set; }

		protected override void Render(HtmlTextWriter writer)
		{
			FormTipAndFocus(writer);
			IsNumeric = false;
			//fix for mozilla browser. Somehow it fires event that causes postback, 
			//one need to cancel bubble event 
			if (TextMode == TextBoxMode.MultiLine)
				Attributes.Add("onkeypress", "if(event.keyCode==13&&event.stopPropagation!=null)event.stopPropagation();");
			else if (!string.IsNullOrEmpty(OnEnterPressSubmitControl))
			{
				Control submitControl = FindControl(this, OnEnterPressSubmitControl, 5);

				if (submitControl != null)
				{
					Attributes.Add("onkeydown", string.Format("if(event.keyCode==13){{document.getElementById('{0}').click();return false;}}", submitControl.ClientID));
				}
			}
			base.Render(writer);
		}

		public override string Text
		{
			get
			{
				if (IsNumeric)
				{
					if (base.Text == string.Empty)
						return int.MinValue.ToString();
				}

				if (IsDecimal)
				{
					if (base.Text == String.Empty)
						return "0";
					return Formatter.FormatDecimal(base.Text);
				}

				return base.Text;
			}
			set
			{
				decimal result;

				if (IsDecimal && Decimal.TryParse(value, out result))
				{
					base.Text = result.ToString("N");
					return;
				}

				if (CleanTags)
				{
					value = value.Replace("<", "&lt;");
					value = value.Replace(">", "&gt;");
				}

				base.Text = value;
			}
		}

		private void FormTipAndFocus(HtmlTextWriter writer)
		{
            //CssClass = "input";

            //if (BluringEnabled)
            //{
            //    Attributes.Add("onfocus", String.Format(FocusBlurStyles, FocusBackground, FocusBorderColor) + ";" + OnFocusScript);
            //    Attributes.Add("onblur", String.Format(FocusBlurStyles, BlurBackground, BlurBorderColor) + ";" + OnBlurScript);
            //}
		}

		private Control FindControl(Control scope, string id, int deep)
		{
			return scope.FindControl(id) ?? (deep > 0 && (scope.Parent != null) ? this.FindControl(scope.Parent, id, --deep) : null);
		}
	}
}