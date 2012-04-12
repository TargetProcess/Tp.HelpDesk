using System;
using System.Collections.Generic;
using System.Text;

namespace Hd.Web.Extensions
{
    public interface IStateKeeper
    {
        Object GetValue(object key);
        void SetValue(object key, object value);
    }
}
