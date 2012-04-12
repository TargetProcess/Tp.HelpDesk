using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Hd.Web.Extensions
{
    public class SessionStateKeeper : IStateKeeper
    {
        public void CheckAccessibility()
        {
            if (ReferenceEquals(HttpContext.Current, null))
                throw new ApplicationException("The component could not be used for non web applications");
        }

        public object GetValue(object key)
        {
            CheckAccessibility();
            return HttpContext.Current.Session[key.ToString()];
        }

        public void SetValue(object key, object value)
        {
            CheckAccessibility();
            HttpContext.Current.Session[key.ToString()] = value;
        }
    }
}
