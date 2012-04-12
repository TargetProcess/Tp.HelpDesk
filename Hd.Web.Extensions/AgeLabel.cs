using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace Hd.Web.Extensions
{
    public class AgeLabel : Control
    {
        public DateTime? Date
        {
            set { ViewState["Date"] = value; }
            get { return ViewState["Date"] as DateTime?; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!Date.HasValue)
                writer.Write("N/A");

            TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan date = new TimeSpan(Date.Value.Ticks);

            TimeSpan result = timeSpan - date;

            double months = Math.Round((result.TotalDays/30), 0);

            if (months > 1)
            {
                writer.Write(months + " months ago");
                return;
            }

            if (result.TotalDays > 1)
            {
                writer.Write(Math.Round(result.TotalDays, 0) + " days ago");
                return;
            }

            if (result.TotalHours > 1)
            {
                writer.Write(Math.Round(result.TotalHours, 0) + " hours ago");
                return;
            }

            if (result.TotalMinutes > 1)
            {
                writer.Write(Math.Round(result.TotalMinutes, 0) + " minutes ago");
                return;
            }

            if (result.TotalSeconds > 1)
            {
                writer.Write(Math.Round(result.TotalSeconds, 0) + " seconds ago");
                return;
            }
            
            writer.Write("Just added");
        }
    }
}
