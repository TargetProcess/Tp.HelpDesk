using System;
using Hd.Web.Extensions;

public partial class Issues : PersisterBasePage
{
	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		controller.RefreshGrid();
	}
}