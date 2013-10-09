using System;
using System.Web.UI;
using Hd.Portal;
using Hd.Web.Extensions;

public partial class Controls_RelatedEntities : UserControl, ITabControl
{
	public void LoadContent(object argument)
	{
		Request request = Hd.Portal.Request.Retrieve(argument as int?);

		if (request == null)
			throw new Exception("The request could not be found");

		grid.DataSource = request.RelatedEntities;
		grid.DataBind();
	}
}