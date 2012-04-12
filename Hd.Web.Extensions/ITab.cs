

namespace Hd.Web.Extensions
{
    public interface ITab
    {

        int? TabIndexNumber
        {
            set;
            get;
        }
        bool Selected
        {
            set;
            get;
        }
        bool ChildrenVisible
        {
            get;
            set;
        }
        string TabTitle
        {
            get;
            set;
        }

        void UpdateTabContent();
        void LoadData();
    }
}
