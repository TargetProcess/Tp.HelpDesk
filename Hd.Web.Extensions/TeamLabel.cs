using System.Collections.Generic;
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
                {
                    str += "<span class='assignments' "
                        + "title='" + team.UserName + " (" + team.UserName + ")'>"
                        + team.UserName + " (" + team.ActorName + ")</span>";
                }
                _text = str;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(_text);
        }
    }
}
