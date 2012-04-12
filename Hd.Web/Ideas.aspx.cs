using System;
using Hd.Web.Extensions;

namespace Hd.Web
{
	public partial class Ideas : PersisterBasePage
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			controller.RefreshGrid();
		}
	}
}