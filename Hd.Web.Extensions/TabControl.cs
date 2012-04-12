using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hd.Web.Extensions
{
    [ParseChildren(true, "Tabs")]
    public class TabControl : CompositeControl
    {
        private List<ITab> _tabs = new List<ITab>();
        private TabEventProcessor _tabEventProcessor = new TabEventProcessor();
        private string _tabCssClass = string.Empty;
        private string _selectedTabCssClass = string.Empty;
        private string _tabPanelCssClass = string.Empty;
        private string _tabContentCssClass = string.Empty;
        private string _queryStringField = string.Empty;
        private object SelectedTabEventKey = new object();

        #region Properties

        public List<ITab> Tabs
        {
            get { return _tabs; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            if (!this.Page.IsPostBack)
                SelectTab(SelectedIndex);
        }

        public string TabCssClass
        {
            set { _tabCssClass = value; }
            get { return _tabCssClass; }
        }

        public string SelectedTabCssClass
        {
            set { _selectedTabCssClass = value; }
            get { return _selectedTabCssClass; }
        }

        public string TabPanelCssClass
        {
            set { _tabPanelCssClass = value; }
            get { return _tabPanelCssClass; }
        }

        public string TabContentCssClass
        {
            get { return _tabContentCssClass; }
            set { _tabContentCssClass = value; }
        }

        public string QueryStringField
        {
            set { _queryStringField = value; }
            get { return _queryStringField; }
        }

        public int SelectedIndex
        {
            set { Page.Session[ClientID] = value; }
            get
            {
                if (Page.Session[ClientID] == null)
                {
                    for (int index = 0; index < _tabs.Count; index++)
                    {
                        if (_tabs[index].Selected)
                        {
                            Page.Session[ClientID] = index;
                            return index;
                        }
                    }
                    Page.Session[ClientID] = 0;
                    return 0;
                }
                else
                    return (int) Page.Session[ClientID];
            }
        }

        public int? Argument
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Context.Request.Params[_queryStringField]);
                }
                catch
                {
                    return null;
                }
            }
        }

        #endregion


        protected override void OnPreRender(EventArgs e)
        {
            JavaScriptRegistrator.RegisterEmbeddedScript(Page, GetType(), "TabControl");
            base.OnPreRender(e);
        }

        protected void TabEventProcessor_TabSelected(object sender, TabClickedEventArgs e)
        {
            SelectedIndex = e.TabIndexNumber;
        }

        protected void TabEventProcessor_TabLoading(object sender, TabClickedEventArgs e)
        {
            int index = e.TabIndexNumber;
            SelectTab(index);
        }

        private void SelectTab(int index)
        {
            SelectedIndex = index;
            _tabs[SelectedIndex].UpdateTabContent();
        }

        protected override void OnInit(EventArgs e)
        {
            EnsureChildControls();
            _tabEventProcessor.TabSelected += new TabEventHandler(TabEventProcessor_TabSelected);
            _tabEventProcessor.TabLoading += new TabEventHandler(TabEventProcessor_TabLoading);
            base.OnInit(e);
        }

        protected override void CreateChildControls()
        {
            //add event processor in order to fire events
            _tabEventProcessor.ID = this.ID + "_tep";
            Controls.Add(_tabEventProcessor);


            //adding tabs
            BulletedList tabList = new BulletedList();
            tabList.Attributes.Add("class", _tabPanelCssClass);
            
            List<ITab> tabsContentList = new List<ITab>();

            for (int index = 0; index < _tabs.Count; index++)
                AddTab(index, tabList, tabsContentList);

            Panel tabsHolder = new Panel();
            tabsHolder.Controls.Add(tabList);
            Controls.Add(tabsHolder);

            //adding tabs content
            Panel tabsContentHolder = new Panel();
            tabsContentHolder.Attributes.Add("class", _tabContentCssClass);
            
            for (int index = 0; index < tabsContentList.Count; index++)
                tabsContentHolder.Controls.Add(tabsContentList[index] as Control);

            Controls.Add(tabsContentHolder);


            base.CreateChildControls();
        }

        private void AddTab(int index, BulletedList tabList, List<ITab> tabsContentList)
        {
            ListItem listItem = new ListItem(_tabs[index].TabTitle);
            listItem.Attributes.Add("tabIndexNumber", index.ToString());
            listItem.Attributes.Add("onclick",
                                    "showTabIndex('" + ClientID + "','" + _tabEventProcessor.UniqueID + "'," +
                                    index.ToString() + ",'" +
                                    _selectedTabCssClass + "','" + _tabCssClass + "')");
            tabList.Items.Add(listItem);

            if (SelectedIndex == index)
                listItem.Attributes.Add("class", _selectedTabCssClass);
            else
                listItem.Attributes.Add("class", _tabCssClass);

            _tabs[index].TabIndexNumber = index;

            tabsContentList.Add(_tabs[index]);
        }
    }
}