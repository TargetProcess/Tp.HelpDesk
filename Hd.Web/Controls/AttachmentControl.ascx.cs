﻿// 
// Copyright (c) 2005-2013 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using System.Collections.Generic;
using System.Web.UI;
using Hd.Portal;

namespace Hd.Web.Controls
{
	public partial class AttachmentControl : UserControl
	{
		private int? GeneralID
		{
			get
			{
				int result;
				return Int32.TryParse(Request.QueryString["RequestID"], out result) ? (int?) result : null;
			}
		}

		public List<FileAttachment> Attachments
		{
			get { return attachments.Attachments; }
		}

		public event EventHandler AttachmentsAdding;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (GeneralID != null)
				LoadAttachments(GeneralID);
		}

		private void LoadAttachments(object argument)
		{
			const string hql = "from Attachment as a where a.General.GeneralID = ?";
			List<Portal.Attachment> list = DataPortal.Instance.Retrieve<Portal.Attachment>(hql, argument);
			lstAttachment.DataSource = list;
			lstAttachment.DataBind();
		}

		protected void OnAddAttachments(object sender)
		{
		}
	}
}