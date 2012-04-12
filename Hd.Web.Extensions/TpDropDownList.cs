using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hd.Web.Extensions
{
    [ToolboxData("<{0}:TpDropDownList runat=server></{0}:TpDropDownList>")]
    public class TpDropDownList : DropDownList
    {
        private string argument;
        private bool isNumeric = true;
        private string focusBackground = "#FFFEBA";
        private string blurBackground = "#FFF";
        private string focusBorderColor = "orange";
        private string blurBorderColor = "#28428B";
        private bool processTerms = false;
        private bool processTermsForFirst = false;
        private bool makeHumanReadable = false;


        public bool IsNumeric
        {
            get { return isNumeric; }
            set { isNumeric = value; }
        }


        public override void DataBind()
        {
            try
            {
                base.DataBind();

            }
            catch (Exception)
            {
                SelectedIndex = 0; // reset values
            }
        }


        /// <summary>
        /// Override SelectedValue to resolve null values binding
        /// </summary>
        public override string SelectedValue
        {
            get
            {
                if (!IsNumeric)
                    return base.SelectedValue;

                if (base.SelectedValue == "" || base.SelectedValue == null)
                    return int.MinValue.ToString();

                return base.SelectedValue;
            }
            set
            {
                if (value == null || value == string.Empty)
                    return;

                base.SelectedValue = value;
            }
        }

        public int? SelectedNumericValue
        {
            get
            {
                int value;
                bool isParsed = Int32.TryParse(SelectedValue, out value);
                return isParsed ? (int?) value : null;
            }
        }

        public int? SelectedNumericArgument
        {
            get
            {
                int value;
                bool isParsed = Int32.TryParse(argument, out value);
                return isParsed ? (int?) value : null;
            }
        }


        [Bindable(true)]
        public string Argument
        {
            get { return argument; }
            set { argument = value; }
        }


        protected override object SaveViewState()
        {
            ViewState["Argument"] = argument;
            return base.SaveViewState();
        }

        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);
            argument = ViewState["Argument"] as string;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            Attributes.Add("onfocus", String.Format(FocusBlurStyles, focusBackground, focusBorderColor));

            Attributes.Add("onblur", String.Format(FocusBlurStyles, blurBackground, blurBorderColor));
            base.Render(writer);
        }

        private string FocusBlurStyles
        {
            get { return "this.style.background = '{0}';this.style.border = '1px solid {1}'"; }
        }

        public bool ProcessTerms
        {
            get { return processTerms; }
            set { processTerms = value; }
        }

        public bool MakeHumanReadable
        {
            get { return makeHumanReadable; }
            set { makeHumanReadable = value; }
        }

        public bool ProcessTermsForFirst
        {
            get { return processTermsForFirst; }
            set { processTermsForFirst = value; }
        }
    }
}