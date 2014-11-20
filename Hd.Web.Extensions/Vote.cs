#region

using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

#endregion

namespace Hd.Web.Extensions
{
    public class VoteEventArgs : EventArgs
    {
        private readonly int? _requestID;

        public VoteEventArgs(int? requestID)
        {
            _requestID = requestID;
        }

        public int? RequestID
        {
            get { return _requestID; }
        }
    }

    public class VoteManager : WebControl, IPostBackEventHandler
    {
        #region Delegates

        public delegate void VoteEventHandler(object sender, VoteEventArgs args);

        #endregion

        #region IPostBackEventHandler Members

        public void RaisePostBackEvent(string eventArgument)
        {
            if (VoteAdded != null)
                VoteAdded(this, new VoteEventArgs(Int32.Parse(eventArgument)));
        }

        #endregion

        public event VoteEventHandler VoteAdded;


        public static VoteManager GetCurrent(Page page)
        {
            if (page == null)
                throw new ArgumentNullException("page");
            return (page.Items[typeof(VoteManager)] as VoteManager);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (GetCurrent(Page) != null)
                throw new InvalidOperationException("More than one VoteManager objects found");

            Page.Items[typeof(VoteManager)] = this;
            ScriptManager.GetCurrent(Page).RegisterAsyncPostBackControl(this);
        }
    }


    public class Vote : Panel
    {
        private readonly HyperLink _voteButton = new HyperLink();
        private readonly Image _voteButtonImage = new Image();
        private readonly Panel _voteButtonPanel = new Panel();
        private readonly Panel _voteLabelPanel = new Panel();
        private readonly Panel _votesCountPanel = new Panel();

        public string VoteImage { get; set; }

        public string CssVotesCount { get; set; }

        public string CssVoteLabel { get; set; }

        public bool IsPossibleToVote { get; set; }

        public long Count { get; set; }

        public int? RequestID { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _voteButton.CssClass = "editLink voter";
            _voteButton.Text = "vote";
            _voteButton.ID = "uxAddVote";
            _votesCountPanel.CssClass = CssVotesCount;
            _voteLabelPanel.CssClass = CssVoteLabel;
            _voteButtonImage.ImageUrl = VoteImage;
        }

        protected override void OnPreRender(EventArgs e)
        {
            _voteButtonPanel.Visible = IsPossibleToVote;
            var manager = VoteManager.GetCurrent(Page);
            var postBackRef = string.Format("javascript:__doPostBack('{0}','{1}');", manager.UniqueID, RequestID);
            _voteButton.Attributes.Add("href", postBackRef);
            base.OnPreRender(e);
        }


        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            _votesCountPanel.Controls.Add(new LiteralControl(Count.ToString()));
            //_voteButtonPanel.Controls.Add(_voteButtonImage);
            Controls.Add(_votesCountPanel);
            if (IsPossibleToVote)
            {
                Controls.Add(new HtmlGenericControl("i"));
                Controls.Add(_voteButton);
            }
            //Controls.Add(_voteLabelPanel);
            Controls.Add(_voteButtonPanel);
        }
    }
}