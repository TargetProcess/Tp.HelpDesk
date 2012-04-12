using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hd.Portal;

namespace Hd.Web.Extensions
{
    public class TeamLabel : WebControl
    {
        private string _text = string.Empty;

        public List<Team> Teams
        {
            set
            {
                string str = string.Empty;

                foreach (Team team in value)
                    str += "<b>" + team.ActorName + ":</b> " + team.UserName + "<br/>";

                this._text = str;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(_text);
        }
    }
}
