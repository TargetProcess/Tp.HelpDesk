using System;
using System.Web.UI;

namespace Hd.Web.Extensions
{
    public class AsyncPostBackHandler : Control, IPostBackEventHandler
    {
        #region Delegates

        public delegate void OnPostBackHandler(string argument);

        #endregion

        #region IPostBackEventHandler Members

        public void RaisePostBackEvent(string eventArgument)
        {
            if (PostBack != null)
                PostBack(eventArgument);
        }

        #endregion

        public event OnPostBackHandler PostBack;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ScriptManager manager = ScriptManager.GetCurrent(Page);
            
            if (manager != null)
                manager.RegisterAsyncPostBackControl(this);
        }

        protected override void Render(HtmlTextWriter writer)
        {
        }
    }
}