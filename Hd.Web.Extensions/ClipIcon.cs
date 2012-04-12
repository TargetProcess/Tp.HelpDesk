using System;
using Hd.Web.Extensions;

namespace Tp.Web.Extensions
{
    public class ClipIcon : Icon
    {
        public ClipIcon()
        {
            Load += new EventHandler(ClipIcon_Load);
        }

        private void ClipIcon_Load(object sender, EventArgs e)
        {
            ImageName = "clip.gif";
        }
    }
}