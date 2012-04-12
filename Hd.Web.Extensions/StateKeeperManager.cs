using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Hd.Web.Extensions
{
    public static class StateKeeperManager
    {
        public static IStateKeeper State
        {
            get
            {
                IStateKeeper keeper = !ReferenceEquals(HttpContext.Current, null)
                   ? new SessionStateKeeper() as IStateKeeper
                   : new StateKeeper() as IStateKeeper;

                return keeper;
            }
        }

    }
}
