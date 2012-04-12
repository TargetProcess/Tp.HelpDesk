using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using System.ComponentModel;
using System.Drawing.Design;

namespace Hd.Web.Extensions
{
    [ToolboxData("<{0}:TpObjectDataSource runat=\"server\"></{0}:TpObjectDataSource>")]
    [PersistChildren(false), ParseChildren(true), DefaultEvent("Selecting"), DefaultProperty("TypeName")]
    public class TpObjectDataSource : DataSourceControl
    {
        private TpObjectDataSourceView defaultView;
        protected override DataSourceView GetView(string viewName)
        {
            return GetView();
        }
        private TpObjectDataSourceView GetView()
        {
            if (defaultView == null)
            {
                defaultView = new TpObjectDataSourceView(this, "Default");
            }
            return defaultView;
        }

        public event EventHandler<TpObjectDataSourceEventArgs> Selecting;
        public event EventHandler<TpObjectDataSourceEventArgs> Selected;
        public event EventHandler<TpObjectDataSourceEventArgs> Updated;
        
        internal void OnSelected(TpObjectDataSourceEventArgs e)
        {
            if (Selected != null)
                Selected(this, e);
        }

        internal void OnSelecting(TpObjectDataSourceEventArgs e)
        {
            if (Selecting != null)
                Selecting(this, e);
        }

        internal void OnUpdated(TpObjectDataSourceEventArgs args)
        {
            if (Updated != null)
                Updated(this, args);
        }
        
        public event EventHandler<TpObjectDataSourceEventArgs> Updating;
        internal void OnUpdating(TpObjectDataSourceEventArgs e)
        {
            if (Updating != null)
            {
                Updating(this, e);
            }
        }

        public event EventHandler<TpObjectDataSourceEventArgs> Inserting;
        internal void OnInserting(TpObjectDataSourceEventArgs e)
        {
            if (Inserting != null)
            {
                Inserting(this, e);
            }
        }
        [DefaultValue((string)null)]
        public string TypeName
        {
            get { return GetView().Source.TypeName; }
            set { GetView().Source.TypeName = value; }
        }
        [DefaultValue((string)null)]
        public string SelectMethod
        {
            get { return GetView().Source.SelectMethod; }
            set { GetView().Source.SelectMethod = value; }
        }
        [DefaultValue((string)null)]
        public string UpdateMethod
        {
            get { return GetView().Source.UpdateMethod; }
            set { GetView().Source.UpdateMethod = value; }
        }
        [DefaultValue((string)null)]
        public string InsertMethod
        {
            get { return GetView().Source.InsertMethod; }
            set { GetView().Source.InsertMethod = value; }
        }
        [DefaultValue((string)null)]
        public string DeleteMethod
        {
            get { return GetView().Source.DeleteMethod; }
            set { GetView().Source.DeleteMethod = value; }
        }
        [DefaultValue((string)null)]
        public string DataObjectTypeName
        {
            get { return GetView().Source.DataObjectTypeName; }
            set { GetView().Source.DataObjectTypeName = value; }
        }
        [DefaultValue(false)]
        public bool EnablePaging
        {
            get { return GetView().EnablePaging; }
            set { GetView().EnablePaging = value; }
        }
        [DefaultValue(false)]
        public bool InternalSort
        {
            get { return GetView().InternalSort; }
            set { GetView().InternalSort = value;  }
        }
        [Category("Data"), PersistenceMode(PersistenceMode.InnerProperty), DefaultValue((string)null), Editor("System.Web.UI.Design.WebControls.ParameterCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)), MergableProperty(false)]
        public ParameterCollection SelectParameters
        {
            get
            {
                return this.GetView().SelectParameters;
            }
        }        
     }
}
