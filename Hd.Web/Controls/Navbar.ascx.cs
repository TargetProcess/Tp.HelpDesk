using System;
using System.Web.UI;

namespace Hd.Web.Controls
{
    public partial class Navbar : UserControl
    {
        public enum SelectedTab
        {
            MyRequests = 1,
            Issues = 2,
            Ideas = 3
        }
        protected override void OnPreRender(EventArgs e)
        {

            switch (CurrentTab)
            {
                case SelectedTab.MyRequests:
                    RequestScopeTab1.CssClass = "selectedTab";
                    break;
                case SelectedTab.Issues:
                    RequestScopeTab2.CssClass = "selectedTab";
                    break;
                case SelectedTab.Ideas:
                    RequestScopeTab4.CssClass = "selectedTab";
                    break;
            }
        }

        public SelectedTab CurrentTab { get; set; }
    }
}