using System;
using System.ComponentModel;
using System.Web.UI;

namespace Hd.Web.Extensions
{
    public class DynamicTab : UpdatePanel, ITab
    {
        private bool _selected = false;
        private bool _NotEqual = false;
        private string _tabTitle = string.Empty;
        private int? _tabIndexNumber;
        private string _value;

        #region Properties

        [Browsable(false)]
        public int? TabIndexNumber
        {
            set { _tabIndexNumber = value; }
            get { return _tabIndexNumber; }
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public bool NotEqual
        {
            get { return _NotEqual; }
            set { _NotEqual = value; }
        }



        public string TabTitle
        {
            set { _tabTitle = value; }
            get { return _tabTitle; }
        }

        public bool Selected
        {
            set { _selected = value; }
            get { return _selected; }
        }


        public bool ChildrenVisible
        {
            set
            {
                foreach (Control control  in ContentTemplateContainer.Controls)
                    control.Visible = value;
            }
            get
            {
                foreach (Control control  in ContentTemplateContainer.Controls)
                {    
                    if (!control.Visible)
                        return false;
                }

                return true;
            }
        }

        #endregion

        #region Methods

        public void LoadData()
        {
            Control[] lazyControls = ControlFinder.FindInterfaceImplementers(this, typeof (ITabControl));
            
            foreach (ITabControl lazyLoadableObject in lazyControls)
                lazyLoadableObject.LoadContent(((TabControl) NamingContainer).Argument);
        }

        public void UpdateTabContent()
        {
            LoadData();

            if (UpdateMode == UpdatePanelUpdateMode.Conditional)
                Update();
        }

        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            if (_tabIndexNumber == ((TabControl) NamingContainer).SelectedIndex)
                ChildrenVisible = true;

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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ChildrenVisible = false;
        }
    }
}