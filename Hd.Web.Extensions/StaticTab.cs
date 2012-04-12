using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Hd.Web.Extensions
{
    public class StaticTab : Panel, ITab
    {
        private string _tabTitle = string.Empty;
        private bool _selected = false;
        private int? _tabIndexNumber;

        #region Properties

        [Browsable(false)]
        public int? TabIndexNumber
        {
            get { return _tabIndexNumber; }
            set { _tabIndexNumber = value; }
        }

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        public bool ChildrenVisible
        {
            get { return Visible; }
            set { Visible = value; }
        }

        public string TabTitle
        {
            get { return _tabTitle; }
            set { _tabTitle = value; }
        }

        public bool Available
        {
            get { return true; }
        }

        #endregion

        #region Methods

        public void UpdateTabContent()
        {}

        public void LoadData()
        {
            Control[] lazyControls = ControlFinder.FindInterfaceImplementers(this, typeof(ITabControl));

            foreach (ITabControl lazyLoadableObject in lazyControls)
                lazyLoadableObject.LoadContent(((TabControl) NamingContainer).Argument);

        }

        #endregion

        #region Events
        protected override void OnPreRender(EventArgs e)
        {
            if (!Page.IsPostBack)
                LoadData();

            base.OnPreRender(e);
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            if (_tabIndexNumber == ((TabControl) NamingContainer).SelectedIndex)
                writer.AddAttribute("style", "display:block");
            else
                writer.AddAttribute("style", "display:none");

            writer.AddAttribute("tabIndexNumber", _tabIndexNumber.ToString());
            
            base.RenderControl(writer);
        }

        #endregion
    }
}