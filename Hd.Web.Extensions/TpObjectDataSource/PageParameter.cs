using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI;
using System.Reflection;

namespace Hd.Web.Extensions
{
    public class PageParameter : Parameter
    {
        protected override object Evaluate(HttpContext context, Control control)
        {
            if ((context == null) || (string.Empty.Equals(PropertyName)))
            {
                return null;
            }
            IHttpHandler handler = context.Handler;
            Type type = handler.GetType();
            PropertyInfo property = type.GetProperty(PropertyName);
            if (property == null)
            {
                return null;
            }
            return property.GetValue(handler, null);
        }
        protected override Parameter Clone()
        {
            PageParameter parameter = new PageParameter();
            parameter.PropertyName = this.PropertyName;
            return parameter;
        }
        public string PropertyName
        {
            get
            {
                return (string)ViewState["PropertyName"] ?? string.Empty;
            }
            set 
            {
                if (PropertyName != value)
                {
                    ViewState["PropertyName"] = value; 
                    base.OnParameterChanged();
                }                
            }
        }
    }
}
