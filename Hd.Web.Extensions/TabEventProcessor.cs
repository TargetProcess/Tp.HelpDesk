using System;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Hd.Web.Extensions
{
    public delegate void TabEventHandler(object sender, TabClickedEventArgs e);
    public class TabEventProcessor : WebControl, IPostBackEventHandler
    {

        public event TabEventHandler TabSelected;
        public event TabEventHandler TabLoading;
        protected override void OnInit(EventArgs e)
        {
            ScriptManager manager = ScriptManager.GetCurrent(Page);
            manager.RegisterAsyncPostBackControl(this);
            Attributes.Add("style","display:none");
            base.OnInit(e);
        }

        public void RaisePostBackEvent(string eventArgument)
        {

            string action;
            int tabIndex;
            try
            {
                string[] args = eventArgument.Split(new Char[] { ':' });
                action = args[0].ToUpper();
                tabIndex = Convert.ToInt32(args[1]);
            }
            catch
            {
                throw new ArgumentException("Invalid argument: " + eventArgument + ".");
            }

            switch (action)
            {
                case "L":
                    if (TabLoading != null)
                        TabLoading(this, new TabClickedEventArgs(tabIndex));
                    break;
                case "S": 
                    if (TabSelected != null)
                        TabSelected(this, new TabClickedEventArgs(tabIndex));
                    break;
                default:
                    throw new ArgumentException("Invalid argument: " + eventArgument + ".");
            }


        }


    }
}