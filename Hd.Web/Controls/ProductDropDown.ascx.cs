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