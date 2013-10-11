// 
// Copyright (c) 2005-2013 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hd.Web.Controls
{
	public partial class ProductDropDown : UserControl
	{
		private int? _projectId;

		public int? ProjectId
		{
			get { return int.Parse(lstProductList.SelectedValue); }
			set { _projectId = value; }
		}

		public override void DataBind()
		{
			base.DataBind();

			if (_projectId.HasValue && lstProductList.Items.FindByValue(_projectId.ToString()) == null)
			{
				lstProductList.Items.Add(new ListItem(string.Empty, _projectId.ToString()));
				Hide();
			}
			lstProductList.SelectedValue = _projectId.ToString();
		}

		private void Hide()
		{
			vldProduct.Enabled = false;
			pnlProductList.Visible = false;
		}
	}
}