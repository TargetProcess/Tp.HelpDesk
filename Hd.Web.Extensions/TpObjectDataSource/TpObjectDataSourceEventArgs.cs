using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.ComponentModel;

namespace Hd.Web.Extensions
{
    public class TpObjectDataSourceEventArgs : CancelEventArgs
    {
        private object businessObject;
        public object BusinessObject
        {
            get { return businessObject; }
            set { businessObject = value; }
        }

		public IOrderedDictionary SelectParams { get; set; }
    }
}
