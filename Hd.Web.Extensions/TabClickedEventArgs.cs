using System;


namespace Hd.Web.Extensions
{
    public class TabClickedEventArgs : EventArgs
    {
        private int _tabIndexNumber;
        public TabClickedEventArgs(int tabIndexNumber)
        {
            _tabIndexNumber = tabIndexNumber;
        }
        public int TabIndexNumber
        {
            get
            {
                return _tabIndexNumber;
            }

        }


    }
}