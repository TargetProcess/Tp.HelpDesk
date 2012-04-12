using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Reflection;
using System.Web;

namespace Hd.Web.Extensions
{
    public class TemplateControlParameter : Parameter
    {
        protected override object Evaluate(HttpContext context, Control control)
        {
            if ((control == null) || (string.Empty.Equals(PropertyName)))
            {
                return null;
            }
            TemplateControl templateControl = control.TemplateControl;
            Type type = templateControl.GetType();
            PropertyInfo property = type.GetProperty(PropertyName);
            if (property == null)
            {
                return null;
            }
            return property.GetValue(templateControl, null);
        }
        protected override Parameter Clone()
        {
            TemplateControlParameter parameter = new TemplateControlParameter();
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
