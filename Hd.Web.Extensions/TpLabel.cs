using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace Hd.Web.Extensions
{
    public class TpLabel : Label
    {
        private string _summaryField = null;

        public string SummaryField
        {
            get { return _summaryField; }
            set { _summaryField = value; }
        }
    }
}
