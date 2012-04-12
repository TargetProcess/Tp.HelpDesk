using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hd.Portal;
using Tp.Web.Extensions;

namespace Hd.Web.Extensions
{
    public class MultiAttachment : Control
    {
        private List<FileAttachment> attachments = new List<FileAttachment>();
        private int _maxCountOfAttachments = 5;
        private bool _showSubmitButton = false;
        private Button _submitButton = null;
        private bool _controlsCreated = false;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CreateChildControls();

            if (ShowSubmitButton)
            {
                _submitButton.Click += new EventHandler(button_Click);
                _submitButton.Visible = true;
            }
            else
            {
                _submitButton.Visible = false;
            }
        }

        public List<FileAttachment> Attachments
        {
            get
            {
                ProcessAttachments();
                return attachments;
            }
            set { attachments = value; }
        }

        public int MaxCountOfAttachments
        {
            get { return _maxCountOfAttachments; }
            set { _maxCountOfAttachments = value; }
        }

        private void ProcessAttachments()
        {
            for (int i = 0; i < MaxCountOfAttachments; i++)
            {
                var upload = FindControl("fileUpload" + i) as FileUpload;
                var description = FindControl("fileDescription" + i) as TextBox;
                if (upload != null && upload.FileName != string.Empty && upload.FileBytes.Length != 0)
                {
                    var attachment = new FileAttachment{OriginalName = upload.FileName, FileStream = upload.FileContent};
                	attachment.Description = description.Text != "type file description" ? description.Text : string.Empty;
                    attachments.Add(attachment);
                }
            }
        }

        protected override void CreateChildControls()
        {
            if (_controlsCreated) return;

            string fileUploadIds = "var fileUploads = new Array();var fileUploadNames = new Array();";
            string fileDescriptions = "var fileDescriptions = new Array();var fileDescriptionsText = new Array();";

            Controls.Add(new LiteralControl("<br />"));
            for (int i = 0; i < MaxCountOfAttachments; i ++)
            {
                var fileUpload = new FileUpload {ID = "fileUpload" + i};
            	fileUpload.Style.Add("display", "none");
                Controls.Add(fileUpload);
                fileUploadIds +=
                    string.Format("fileUploads[{0}] = '{1}';fileUploadNames[{0}] = '{2}';", i, fileUpload.ClientID,
                                  fileUpload.UniqueID);

                var attachmentDescription = new TextBox {ID = "fileDescription" + i};
            	attachmentDescription.Style.Add("display", "none");
                Controls.Add(attachmentDescription);
                fileDescriptions +=
                    string.Format("fileDescriptions[{0}] = '{1}';fileDescriptionsText[{0}] = '{2}';", i,
                                  attachmentDescription.ClientID, attachmentDescription.UniqueID);
            }

            var span = new Literal {Text = "<span id='attachments_content'></span>"};
        	Controls.Add(span);

            var table = new Table();
            var actionRow = new TableRow();

            var cellClip = new TableCell {Width = Unit.Percentage(1), VerticalAlign = VerticalAlign.Middle};
        	var clipIcon = new ClipIcon();
            cellClip.Controls.Add(clipIcon);

            actionRow.Cells.Add(cellClip);

            var cellAddAttachment = new TableCell
                                    	{
                                    		Width = Unit.Percentage(99),
                                    		HorizontalAlign = HorizontalAlign.Left,
                                    		VerticalAlign = VerticalAlign.Middle,
                                    		Wrap = false
                                    	};
        	var action = new Literal
        	             	{
        	             		Text =
        	             			"<span id='maximumExceeded'></span><a id='attachments_more' href='javascript:AddAttachment();'>Attach a file</a>"
        	             	};
        	cellAddAttachment.Controls.Add(action);

            actionRow.Cells.Add(cellAddAttachment);

            table.Rows.Add(actionRow);

            Controls.Add(table);
            Controls.Add(new LiteralControl("<br />"));

            _submitButton = new Button {Text = "Add Attachment(s)", CssClass = "button"};
        	_submitButton.Style.Add(HtmlTextWriterStyle.Display, "none");
            Controls.Add(_submitButton);

            var script = new Literal
                         	{
                         		Text = string.Format(
                         		                    	"<script language='javascript'>{0}{1}var max_attachments_count = fileUploads.length;var _submitButtonID = '{2}';</script>",
                         		                    	fileUploadIds, fileDescriptions, _submitButton.ClientID)
                         	};

        	Controls.Add(script);

            _controlsCreated = true;
        }

        public delegate void AttachmentsUploadedDelegate(object sender);

        public event AttachmentsUploadedDelegate AttachmentsUploaded;

        public bool ShowSubmitButton
        {
            get { return _showSubmitButton; }
            set { _showSubmitButton = value; }
        }

        private void button_Click(object sender, EventArgs e)
        {
            if (AttachmentsUploaded != null)
            {
                AttachmentsUploaded(this);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            JavaScriptRegistrator.RegisterEmbeddedScript(Page, GetType(), "MultiAttachment");
            base.OnPreRender(e);
        }
    }
}